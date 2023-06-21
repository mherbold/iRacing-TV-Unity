
using System;

using UnityEngine;

public class OverlayLeaderboard : MonoBehaviour
{
	public GameObject enable;
	public GameObject leaderboardBackground;
	public GameObject positionSplitter;
	public GameObject placeTemplate;

	[NonSerialized] public GameObject[] places;

	[NonSerialized] public OverlayPlace[] overlayPlaces;

	public void Awake()
	{
		placeTemplate.SetActive( false );

		places = new GameObject[ LiveDataLeaderboard.MaxNumPlaces ];

		overlayPlaces = new OverlayPlace[ LiveDataLeaderboard.MaxNumPlaces ];

		for ( var placeIndex = 0; placeIndex < places.Length; placeIndex++ )
		{
			places[ placeIndex ] = Instantiate( placeTemplate );

			places[ placeIndex ].transform.SetParent( placeTemplate.transform.parent, false );

			places[ placeIndex ].SetActive( true );

			overlayPlaces[ placeIndex ] = places[ placeIndex ].GetComponent<OverlayPlace>();

			overlayPlaces[ placeIndex ].carNumber_ImageSettings.carIdx = placeIndex;
		}
	}

	public void Start()
	{
		SettingsUpdated();
	}

	public void SettingsUpdated()
	{
		transform.localPosition = new Vector2( Settings.overlay.leaderboardOverlayPosition.x, -Settings.overlay.leaderboardOverlayPosition.y );

		enable.SetActive( Settings.overlay.leaderboardOverlayEnabled );
	}

	public void LiveDataUpdated()
	{
		enable.SetActive( Settings.overlay.leaderboardOverlayEnabled && LiveData.Instance.liveDataLeaderboard.show );

		// leaderboard

		for ( var placeIndex = 0; placeIndex < LiveDataLeaderboard.MaxNumPlaces; placeIndex++ )
		{
			var liveDataPlace = LiveData.Instance.liveDataLeaderboard.liveDataPlaces[ placeIndex ];

			var overlayPlace = overlayPlaces[ placeIndex ];

			if ( !liveDataPlace.show )
			{
				overlayPlace.gameObject.SetActive( false );
			}
			else
			{
				overlayPlace.gameObject.SetActive( true );
			}

			// update place position

			overlayPlace.transform.localPosition = liveDataPlace.position;

			// update place text

			overlayPlace.place_Text.text = liveDataPlace.placeText;

			// driver name

			overlayPlace.driverName_Text.text = liveDataPlace.driverNameText;
			overlayPlace.driverName_Text.color = liveDataPlace.driverNameColor;

			// telemetry

			overlayPlace.telemetry_Text.text = liveDataPlace.telemetryText;
			overlayPlace.telemetry_Text.color = liveDataPlace.telemetryColor;

			// highlight

			overlayPlace.highlight.SetActive( liveDataPlace.showHighlight );
			overlayPlace.speed.SetActive( liveDataPlace.showHighlight );

			overlayPlace.speed_Text.text = liveDataPlace.speedText;
		}

		// splitter

		positionSplitter.SetActive( LiveData.Instance.liveDataLeaderboard.showSplitter );
	}
}
