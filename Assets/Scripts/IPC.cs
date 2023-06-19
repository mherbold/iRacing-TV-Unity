
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Xml.Serialization;

using UnityEngine;

public class IPC : MonoBehaviour
{
	public const int MAX_MEMORY_MAPPED_FILE_SIZE = 1 * 1024 * 1024;

	[NonSerialized] public MemoryMappedFile memoryMappedFile;

	[NonSerialized] public long index = 0;

	public void Awake()
	{
		memoryMappedFile = MemoryMappedFile.CreateOrOpen( Program.IpcName, MAX_MEMORY_MAPPED_FILE_SIZE );
	}

	public void Update()
	{
		var viewAccessor = memoryMappedFile.CreateViewAccessor( 0, MAX_MEMORY_MAPPED_FILE_SIZE );

		var index = viewAccessor.ReadInt64( 0 );

		viewAccessor.Dispose();

		if ( index == this.index )
		{
			viewAccessor.Dispose();
		}
		else
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

			var size = viewAccessor.ReadUInt32( 8 );

			var buffer = new byte[ size ];

			viewAccessor.ReadArray( 12, buffer, 0, buffer.Length );

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
