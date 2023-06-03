
using UnityEngine;

using irsdkSharp;
using irsdkSharp.Serialization.Models.Data;
using irsdkSharp.Serialization.Models.Session;
using irsdkSharp.Serialization;

public class IRSDK : MonoBehaviour
{
	public static readonly IRacingSDK iRacingSdk = new();

	public static bool isConnected = false;
	public static bool wasConnected = false;

	public static int sessionInfoUpdate = -1;

	public static IRacingSessionModel session = null;
	public static DataModel data = null;

	public static NormalizedSession normalizedSession = new();
	public static NormalizedData normalizedData = new();

	public void Update()
	{
		isConnected = iRacingSdk.IsConnected();

		if ( isConnected )
		{
			data = iRacingSdk.GetSerializedData().Data;

			if ( ( session == null ) || ( iRacingSdk.Header.SessionInfoUpdate != sessionInfoUpdate ) )
			{
				session = iRacingSdk.GetSerializedSessionInfo();

				normalizedSession.SessionUpdate();

				normalizedData.SessionUpdate();

				sessionInfoUpdate = iRacingSdk.Header.SessionInfoUpdate;
			}
			
			if ( data.SessionNum != normalizedSession.sessionNumber )
			{
				normalizedSession.SessionChange();

				normalizedData.SessionChange();
			}

			if ( data.SessionNum >= 0 )
			{
				normalizedData.Update();
			}
		}
		else if ( wasConnected )
		{
			normalizedData.Reset();
			normalizedSession.Reset();
		}

		wasConnected = isConnected;
	}
}
