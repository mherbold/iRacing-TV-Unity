
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using UnityEngine;

public class IPC : MonoBehaviour
{
	public const int MAX_MEMORY_MAPPED_FILE_SIZE = 1 * 1024 * 1024;

	[NonSerialized] public MemoryMappedFile memoryMappedFileSettings;
	[NonSerialized] public MemoryMappedFile memoryMappedFileLiveData;

	[NonSerialized] public long indexSettings = 0;
	[NonSerialized] public long indexLiveData = 0;

	[NonSerialized] public Task updateStreamingTexturesTask;

	public void Awake()
	{
		memoryMappedFileSettings = MemoryMappedFile.CreateOrOpen( Program.IpcNameSettings, MAX_MEMORY_MAPPED_FILE_SIZE );
		memoryMappedFileLiveData = MemoryMappedFile.CreateOrOpen( Program.IpcNameLiveData, MAX_MEMORY_MAPPED_FILE_SIZE );
	}

	public void Update()
	{
		UpdateSettings();
		UpdateLiveData();
	}

	public void UpdateSettings()
	{
		var viewAccessor = memoryMappedFileSettings.CreateViewAccessor( 0, MAX_MEMORY_MAPPED_FILE_SIZE );

		var index = viewAccessor.ReadInt64( 0 );

		if ( index == this.indexSettings )
		{
			viewAccessor.Dispose();
		}
		else
		{
			this.indexSettings = index;

			if ( Mutex.TryOpenExisting( Program.MutexNameSettings, out var mutex ) )
			{
				mutex.WaitOne();
			}
			else
			{
				mutex = new Mutex( true, Program.MutexNameSettings, out var createdNew );

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

			foreach ( var rootGameObject in rootGameObjects )
			{
				rootGameObject.BroadcastMessage( "SettingsUpdated", SendMessageOptions.DontRequireReceiver );
			}
		}
	}

	public void UpdateLiveData()
	{
		var viewAccessor = memoryMappedFileLiveData.CreateViewAccessor( 0, MAX_MEMORY_MAPPED_FILE_SIZE );

		var index = viewAccessor.ReadInt64( 0 );

		if ( index == this.indexLiveData )
		{
			viewAccessor.Dispose();
		}
		else
		{
			this.indexLiveData = index;

			if ( Mutex.TryOpenExisting( Program.MutexNameLiveData, out var mutex ) )
			{
				mutex.WaitOne();
			}
			else
			{
				mutex = new Mutex( true, Program.MutexNameLiveData, out var createdNew );

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

			var xmlSerializer = new XmlSerializer( typeof( LiveData ) );

			var memoryStream = new MemoryStream( buffer );

			var liveData = (LiveData) xmlSerializer.Deserialize( memoryStream );

			LiveData.Instance.Update( liveData );

			StreamingTextures.CheckForUpdates();

			var rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

			foreach ( var rootGameObject in rootGameObjects )
			{
				rootGameObject.BroadcastMessage( "LiveDataUpdated", SendMessageOptions.DontRequireReceiver );
			}
		}
	}
}
