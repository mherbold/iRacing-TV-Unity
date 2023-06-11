
using System;

public static class Program
{
	public const string IpcName = "iRacing-TV IPC";
	public const string MutexName = "iRacing-TV Mutex";

	public const string AppName = "iRacing-TV";
	public const string AppNameSTT = "iRacing-STT-VR";

	public static readonly string documentsFolder = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + $"\\{AppName}\\";
	public static readonly string documentsFolderSTT = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + $"\\{AppNameSTT}\\";
}
