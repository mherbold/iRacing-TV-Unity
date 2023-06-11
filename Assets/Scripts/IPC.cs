
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Xml.Serialization;

using UnityEngine;

public class IPC : MonoBehaviour
{
	[NonSerialized] public MemoryMappedFile memoryMappedFile;

	[NonSerialized] public uint index = 0;

	public void Awake()
	{
		memoryMappedFile = MemoryMappedFile.CreateOrOpen( Program.IpcName, 1 * 1024 * 1024 );
	}

	public void Update()
	{
		var viewAccessor = memoryMappedFile.CreateViewAccessor( 0, 4 );

		var index = viewAccessor.ReadUInt32( 0 );

		viewAccessor.Dispose();

		if ( index != this.index )
		{
			this.index = index;

			if ( Mutex.TryOpenExisting( Program.MutexName, out var mutex ) )
			{
				mutex.WaitOne();
			}
			else
			{
				mutex = new Mutex( true, Program.MutexName, out var createdNew );

				if ( !createdNew )
				{
					mutex.WaitOne();
				}
			}

			viewAccessor = memoryMappedFile.CreateViewAccessor( 4, 4 );

			var size = viewAccessor.ReadUInt32( 0 );

			viewAccessor.Dispose();

			viewAccessor = memoryMappedFile.CreateViewAccessor( 8, size );

			var buffer = new byte[ size ];

			viewAccessor.ReadArray( 0, buffer, 0, buffer.Length );

			viewAccessor.Dispose();

			mutex.ReleaseMutex();

			var xmlSerializer = new XmlSerializer( typeof( SettingsOverlay ) );

			var memoryStream = new MemoryStream( buffer );

			Settings.overlay = (SettingsOverlay) xmlSerializer.Deserialize( memoryStream );

			var rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

			foreach (  var rootGameObject in rootGameObjects )
			{
				rootGameObject.BroadcastMessage( "OverlayUpdated", SendMessageOptions.DontRequireReceiver );
			}
		}
	}
}
