
using System;

using UnityEngine;

public class OverlayLeaderboard : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject leaderboardBackground;
	public GameObject positionSplitter;
	public GameObject placeTemplate;

	[NonSerialized] public ImageSettings leaderboardBackground_ImageSettings;
	[NonSerialized] public ImageSettings positionSplitter_ImageSettings;

	[NonSerialized] public GameObject[] places;

	[NonSerialized] public OverlayLeaderboardPlace[] overlayLeaderboardPlaces;

	public void Awake()
	{
		placeTemplate.SetActive( false );

		leaderboardBackground_ImageSettings = leaderboardBackground.GetComponent<ImageSettings>();
		positionSplitter_ImageSettings = positionSplitter.GetComponent<ImageSettings>();

		places = new GameObject[ LiveDataLeaderboard.MaxNumPlaces ];

		overlayLeaderboardPlaces = new OverlayLeaderboardPlace[ LiveDataLeaderboard.MaxNumPlaces ];

		for ( var placeIndex = 0; placeIndex < places.Length; placeIndex++ )
		{
			places[ placeIndex ] = Instantiate( placeTemplate );

			places[ placeIndex ].transform.SetParent( placeTemplate.transform.parent, false );

			places[ placeIndex ].SetActive( true );

			overlayLeaderboardPlaces[ placeIndex ] = places[ placeIndex ].GetComponent<OverlayLeaderboardPlace>();

			overlayLeaderboardPlaces[ placeIndex ].carNumber_ImageSettings.carIdx = placeIndex;
		}
	}

	public void Start()
	{
		SettingsUpdated();
	}

	public void Update()
	{
		enable.SetActive( Settings.overlay.leaderboardEnabled && LiveData.Instance.liveDataLeaderboard.show && ipc.isConnected && LiveData.Instance.isConnected );
	}

	public void SettingsUpdated()
	{
		transform.localPosition = new Vector2( Settings.overlay.leaderboardPosition.x, -Settings.overlay.leaderboardPosition.y );
	}

	public void LiveDataUpdated()
	{
		leaderboardBackground_ImageSettings.SetSize( LiveData.Instance.liveDataLeaderboard.backgroundSize );
		positionSplitter_ImageSettings.SetPosition( LiveData.Instance.liveDataLeaderboard.splitterPosition );

		// leaderboard

		for ( var placeIndex = 0; placeIndex < LiveDataLeaderboard.MaxNumPlaces; placeIndex++ )
		{
			var liveDataPlace = LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ];

			var overlayLeaderboardPlace = overlayLeaderboardPlaces[ placeIndex ];

			if ( !liveDataPlace.show )
			{
				overlayLeaderboardPlace.gameObject.SetActive( false );
			}
			else
			{
				overlayLeaderboardPlace.gameObject.SetActive( true );
			}

			// update place position

			overlayLeaderboardPlace.transform.localPosition = liveDataPlace.position;

			// update place text

			overlayLeaderboardPlace.place_Text.text = liveDataPlace.placeText;

			// driver name

			overlayLeaderboardPlace.driverName_Text.text = liveDataPlace.driverNameText;
			overlayLeaderboardPlace.driverName_Text.color = liveDataPlace.driverNameColor;

			// telemetry

			overlayLeaderboardPlace.telemetry_Text.text = liveDataPlace.telemetryText;
			overlayLeaderboardPlace.telemetry_Text.color = liveDataPlace.telemetryColor;

			// highlight

			overlayLeaderboardPlace.highlight.SetActive( liveDataPlace.showHighlight );
			overlayLeaderboardPlace.speed.SetActive( liveDataPlace.showHighlight );

			overlayLeaderboardPlace.speed_Text.text = liveDataPlace.speedText;
		}

		// splitter

		positionSplitter.SetActive( LiveData.Instance.liveDataLeaderboard.showSplitter );
	}
}
