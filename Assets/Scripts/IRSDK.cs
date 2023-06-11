
using System;

using UnityEngine;

using irsdkSharp;
using irsdkSharp.Serialization.Models.Data;
using irsdkSharp.Serialization.Models.Session;
using irsdkSharp.Serialization;

public class IRSDK : MonoBehaviour
{
	[NonSerialized] public static readonly IRacingSDK iRacingSdk = new();

	[NonSerialized] public static bool isConnected = false;
	[NonSerialized] public static bool wasConnected = false;

	[NonSerialized] public static int sessionInfoUpdate = -1;

	[NonSerialized] public static IRacingSessionModel session = null;
	[NonSerialized] public static DataModel data = null;

	[NonSerialized] public static NormalizedSession normalizedSession = new();
	[NonSerialized] public static NormalizedData normalizedData = new();

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
