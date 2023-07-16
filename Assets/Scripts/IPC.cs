
using System;
using System.Diagnostics;
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

	[NonSerialized] public static long indexSettings = 0;
	[NonSerialized] public static long indexLiveData = 0;

	[NonSerialized] public bool isConnected = false;
	[NonSerialized] public long lastUpdateMilliseconds = 0;
	[NonSerialized] public Stopwatch stopwatch;

	[NonSerialized] public Task updateStreamingTexturesTask;

	public void Awake()
	{
		memoryMappedFileSettings = MemoryMappedFile.CreateOrOpen( Program.IpcNameSettings, MAX_MEMORY_MAPPED_FILE_SIZE );
		memoryMappedFileLiveData = MemoryMappedFile.CreateOrOpen( Program.IpcNameLiveData, MAX_MEMORY_MAPPED_FILE_SIZE );

		stopwatch = new Stopwatch();

		stopwatch.Start();
	}

	public void Update()
	{
		var settingsUpdated = UpdateSettings();
		var liveDataUpdated = UpdateLiveData();

		if ( !settingsUpdated && !liveDataUpdated )
		{
			if ( ( stopwatch.ElapsedMilliseconds - lastUpdateMilliseconds > 1000 ) )
			{
				isConnected = false;
			}
		}
		else
		{
			isConnected = true;
			lastUpdateMilliseconds = stopwatch.ElapsedMilliseconds;
		}
	}

	public bool UpdateSettings()
	{
		var viewAccessor = memoryMappedFileSettings.CreateViewAccessor( 0, MAX_MEMORY_MAPPED_FILE_SIZE );

		var index = viewAccessor.ReadInt64( 0 );

		if ( index == indexSettings )
		{
			viewAccessor.Dispose();

			return false;
		}
		else
		{
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

			indexSettings = index;

			return true;
		}
	}

	public bool UpdateLiveData()
	{
		var viewAccessor = memoryMappedFileLiveData.CreateViewAccessor( 0, MAX_MEMORY_MAPPED_FILE_SIZE );

		var index = viewAccessor.ReadInt64( 0 );

		if ( index == indexLiveData )
		{
			viewAccessor.Dispose();

			return false;
		}
		else
		{
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

			indexLiveData = index;

			return true;
		}
	}
}
