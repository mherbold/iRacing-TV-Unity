
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

public static class SessionFlagsPlayback
{
	public static string sessionFlagsPath = Program.documentsFolder + "SessionFlags";

	public static List<SessionFlagsData> sessionFlagsDataList = null;

	public static string sessionFlagsFileName = string.Empty;
	public static StreamWriter streamWriter = null;

	public static string GetSessionFlagsFileName()
	{
		return $"{sessionFlagsPath}\\{IRSDK.normalizedSession.sessionId}-{IRSDK.normalizedSession.subSessionId}.csv";
	}

	public static void OpenForRecording()
	{
		var newSessionFlagsFileName = GetSessionFlagsFileName();

		if ( newSessionFlagsFileName != sessionFlagsFileName )
		{
			sessionFlagsFileName = newSessionFlagsFileName;

			sessionFlagsDataList = new List<SessionFlagsData>();

			if ( streamWriter != null )
			{
				streamWriter.Close();

				streamWriter = null;
			}

			Directory.CreateDirectory( sessionFlagsPath );

			streamWriter = File.AppendText( sessionFlagsFileName );
		}
	}

	public static void LoadRecording()
	{
		var newSessionFlagsFileName = GetSessionFlagsFileName();

		if ( newSessionFlagsFileName != sessionFlagsFileName )
		{
			sessionFlagsFileName = newSessionFlagsFileName;

			sessionFlagsDataList = new List<SessionFlagsData>();

			if ( File.Exists( sessionFlagsFileName ) )
			{
				var streamReader = File.OpenText( sessionFlagsFileName );

				while ( true )
				{
					var line = streamReader.ReadLine();

					if ( line == null )
					{
						break;
					}

					var match = Regex.Match( line, "(.*),(.*),0x(.*)" );

					if ( match.Success )
					{
						sessionFlagsDataList.Add( new SessionFlagsData( int.Parse( match.Groups[ 1 ].Value ), float.Parse( match.Groups[ 2 ].Value, CultureInfo.InvariantCulture.NumberFormat ), uint.Parse( match.Groups[ 3 ].Value, NumberStyles.HexNumber ) ) );
					}
				}
			}

			sessionFlagsDataList.Reverse();
		}
	}

	public static void Close()
	{
		sessionFlagsFileName = string.Empty;
		sessionFlagsDataList = new List<SessionFlagsData>();
	}

	public static void Record( int sessionNumber, double sessionTime, uint sessionFlags )
	{
		if ( sessionFlags != IRSDK.normalizedData.sessionFlags )
		{
			if ( streamWriter != null )
			{
				var sessionFlagsAsHex = sessionFlags.ToString( "X8" );

				streamWriter.WriteLine( $"{sessionNumber},{sessionTime:0.000},0x{sessionFlagsAsHex}" );

				streamWriter.Flush();
			}
		}
	}

	public static uint Playback( int sessionNumber, double sessionTime )
	{
		if ( sessionFlagsDataList != null )
		{
			foreach ( var sessionFlagData in sessionFlagsDataList )
			{
				if ( ( sessionNumber == sessionFlagData.sessionNumber ) && ( sessionTime >= sessionFlagData.sessionTime ) )
				{
					return sessionFlagData.sessionFlags;
				}
			}
		}

		return 0;
	}
}
