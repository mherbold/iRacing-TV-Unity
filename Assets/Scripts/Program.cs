
using System;
using System.IO;

public static class Program
{
	public const string AppName = "iRacing-TV";

	public const string IpcNameSettings = "iRacing-TV IPC Settings";
	public const string IpcNameLiveData = "iRacing-TV IPC Live Data";

	public const string MutexNameSettings = "iRacing-TV Mutex Settings";
	public const string MutexNameLiveData = "iRacing-TV Mutex Live Data";

	public static readonly string documentsFolder = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + $"\\{AppName}\\";

	public static string GetFullPath( string relativePath )
	{
		if ( Path.IsPathFullyQualified( relativePath ) )
		{
			return relativePath;
		}
		else
		{
			return Path.GetFullPath( relativePath, documentsFolder );
		}
	}
}
