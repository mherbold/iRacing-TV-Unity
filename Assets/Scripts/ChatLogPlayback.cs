
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

public static class ChatLogPlayback
{
	public static string chatLogPath = Program.documentsFolderSTT + "ChatLogs";

	public static List<ChatLogData> chatLogList = new();

	public static string chatLogFileName = string.Empty;

	public static string GetChatLogFileName()
	{
		return $"{chatLogPath}\\{IRSDK.normalizedSession.sessionId}-{IRSDK.normalizedSession.subSessionId}.csv";
	}

	public static void LoadRecording()
	{
		var newChatLogFileName = GetChatLogFileName();

		if ( newChatLogFileName != chatLogFileName )
		{
			chatLogFileName = newChatLogFileName;

			chatLogList.Clear();

			if ( File.Exists( chatLogFileName ) )
			{
				var streamReader = File.OpenText( chatLogFileName );

				var startSessionTime = 0.0;

				while ( true )
				{
					var line = streamReader.ReadLine();

					if ( line == null )
					{
						break;
					}

					var match = Regex.Match( line, "([^,]*),([^,]*),([^,]*),([^,]*)(,([^,]*))?(,\"([^\"]*)\")?" );

					if ( match.Success )
					{
						var sessionNumber = int.Parse( match.Groups[ 1 ].Value );
						var sessionTime = float.Parse( match.Groups[ 2 ].Value, CultureInfo.InvariantCulture.NumberFormat );
						var eventId = int.Parse( match.Groups[ 3 ].Value );

						if ( eventId == 5 )
						{
							if ( startSessionTime == 0 )
							{
								startSessionTime = sessionTime;
							}
						}
						else if ( eventId == 6 )
						{
							if ( startSessionTime > 0 )
							{
								chatLogList.Add( new ChatLogData( sessionNumber, startSessionTime - 1.6, sessionTime + 2, match.Groups[ 8 ].Value ) );

								startSessionTime = 0;
							}
						}
					}
				}
			}

			chatLogList.Reverse();
		}
	}

	public static void Close()
	{
		chatLogFileName = string.Empty;

		chatLogList.Clear();
	}

	public static string Playback( int sessionNumber, double sessionTime )
	{
		if ( chatLogList != null )
		{
			foreach ( var chatLogData in chatLogList )
			{
				if ( ( sessionNumber == chatLogData.sessionNumber ) && ( sessionTime >= chatLogData.startSessionTime ) && ( sessionTime < chatLogData.endSessionTime ) )
				{
					return chatLogData.text;
				}
			}
		}

		return null;
	}
}
