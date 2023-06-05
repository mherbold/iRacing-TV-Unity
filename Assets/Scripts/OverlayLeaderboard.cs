
using System;

using UnityEngine;

using irsdkSharp.Serialization.Enums.Fastest;

public class OverlayLeaderboard : MonoBehaviour
{
	public GameObject leaderboardBackground;
	public GameObject positionSplitter;

	public GameObject placeTemplate;

	private GameObject[] places;

	private OverlayPlace[] overlayPlaces;

	public void Awake()
	{
		placeTemplate.SetActive( false );

		places = new GameObject[ Settings.data.leaderboardPlaceCount ];

		overlayPlaces = new OverlayPlace[ Settings.data.leaderboardPlaceCount ];

		for ( var placeIndex = 0; placeIndex < places.Length; placeIndex++ )
		{
			places[ placeIndex ] = Instantiate( placeTemplate );

			places[ placeIndex ].transform.SetParent( placeTemplate.transform.parent, false );

			places[ placeIndex ].SetActive( true );

			overlayPlaces[ placeIndex ] = places[ placeIndex ].GetComponent<OverlayPlace>();
		}
	}

	public void Start()
	{
		transform.localPosition = new Vector2( Settings.data.leaderboardOverlayPosition.x, -Settings.data.leaderboardOverlayPosition.y );

		if ( !Settings.data.showLeaderboardOverlay )
		{
			gameObject.SetActive( false );
		}
	}

	public void Update()
	{
		if ( !IRSDK.isConnected )
		{
			return;
		}

		// leaderboard splits

		var bottomSplitCount = Settings.data.leaderboardPlaceCount / 2;
		var bottomSplitLastPosition = Settings.data.leaderboardPlaceCount;

		if ( IRSDK.normalizedSession.isInRaceSession && ( IRSDK.normalizedData.sessionState == SessionState.StateRacing ) )
		{
			foreach ( var normalizedCar in IRSDK.normalizedData.leaderboardSortedNormalizedCars )
			{
				if ( !normalizedCar.includeInLeaderboard )
				{
					break;
				}

				if ( IRSDK.normalizedData.camCarIdx == normalizedCar.carIdx )
				{
					if ( normalizedCar.leaderboardPosition > bottomSplitLastPosition )
					{
						while ( bottomSplitLastPosition < normalizedCar.leaderboardPosition )
						{
							bottomSplitLastPosition += bottomSplitCount;
						}

						if ( bottomSplitLastPosition > IRSDK.normalizedData.numLeaderboardCars )
						{
							bottomSplitLastPosition = IRSDK.normalizedData.numLeaderboardCars;
						}

						break;
					}
				}
			}
		}

		var topSplitFirstPosition = 1;
		var topSplitLastPosition = Settings.data.leaderboardPlaceCount - bottomSplitCount;
		var bottomSplitFirstPosition = bottomSplitLastPosition - bottomSplitCount + 1;

		// reset cars

		bool showLeaderboardBackground = false;

		foreach ( var normalizedCar in IRSDK.normalizedData.leaderboardSortedNormalizedCars )
		{
			normalizedCar.visibleOnLeaderboard = false;
		}

		// reset places

		foreach ( var overlayPlace in overlayPlaces )
		{
			overlayPlace.shouldBeVisible = false;
		}

		// leaderboard

		var carInFrontLapPosition = 0.0f;
		var leadCarF2Time = 0.0f;

		NormalizedCar normalizedCarInFront = null;

		foreach ( var normalizedCar in IRSDK.normalizedData.leaderboardSortedNormalizedCars )
		{
			// stop when we run out of cars to show

			if ( !normalizedCar.includeInLeaderboard )
			{
				break;
			}

			// lead car f2 time

			if ( leadCarF2Time == 0 )
			{
				leadCarF2Time = normalizedCar.f2Time;
			}

			// skip cars not visible on the leaderboard

			if ( ( ( normalizedCar.leaderboardPosition < topSplitFirstPosition ) || ( normalizedCar.leaderboardPosition > topSplitLastPosition ) ) && ( ( normalizedCar.leaderboardPosition < bottomSplitFirstPosition ) || ( normalizedCar.leaderboardPosition > bottomSplitLastPosition ) ) )
			{
				continue;
			}

			// stop when we get to cars that have not qualified yet (only during qualifying)

			if ( IRSDK.normalizedSession.isInQualifyingSession )
			{
				if ( normalizedCar.f2Time == 0 )
				{
					break;
				}
			}

			// place index

			var placeIndex = normalizedCar.leaderboardPosition - ( ( normalizedCar.leaderboardPosition >= bottomSplitFirstPosition ) ? bottomSplitFirstPosition - topSplitLastPosition : topSplitFirstPosition );

			// this car is visible on the leaderboard

			normalizedCar.visibleOnLeaderboard = true;

			// we want to show the leaderboard background

			showLeaderboardBackground = true;

			// make place active

			overlayPlaces[ placeIndex ].shouldBeVisible = true;

			// compute place offset

			var targetOffsetPosition = new Vector2( 0.0f, Settings.data.leaderboardPlaceSpacing * placeIndex );

			if ( !normalizedCar.leaderboardPlacePositionOffsetIsValid )
			{
				normalizedCar.leaderboardPlacePositionOffsetIsValid = true;
				normalizedCar.leaderboardPlacePositionOffset = targetOffsetPosition;
				normalizedCar.leaderboardPlacePositionVelocity = 0;
			}
			else if ( normalizedCar.leaderboardPlacePositionOffset != targetOffsetPosition )
			{
				var offsetVector = targetOffsetPosition - normalizedCar.leaderboardPlacePositionOffset;

				var remainingDistance = Vector2.Distance( Vector2.zero, offsetVector );

				var acceleration = ( remainingDistance - 10.0f ) * 0.025f;

				normalizedCar.leaderboardPlacePositionVelocity += acceleration;

				normalizedCar.leaderboardPlacePositionVelocity = Math.Min( Math.Max( Math.Min( normalizedCar.leaderboardPlacePositionVelocity, 10.0f ), 0.1f ), remainingDistance );

				normalizedCar.leaderboardPlacePositionOffset += offsetVector.normalized * normalizedCar.leaderboardPlacePositionVelocity;
			}

			// update place position

			overlayPlaces[ placeIndex ].transform.localPosition = new Vector2( Settings.data.leaderboardFirstPlacePosition.x, -Settings.data.leaderboardFirstPlacePosition.y ) - normalizedCar.leaderboardPlacePositionOffset;

			// update place text

			overlayPlaces[ placeIndex ].place_Text.text = normalizedCar.leaderboardPosition.ToString();

			// car number

			overlayPlaces[ placeIndex ].carNumber_ImageSettings.SetCarIdx( normalizedCar.carIdx );

			// driver name

			overlayPlaces[ placeIndex ].driverName_Text.text = normalizedCar.abbrevName;

			if ( Settings.data.useClassColorsForDriverNames )
			{
				if ( overlayPlaces[ placeIndex ].driverName_TextSettings.textSettingsData != null )
				{
					overlayPlaces[ placeIndex ].driverName_Text.color = Color.Lerp( overlayPlaces[ placeIndex ].driverName_TextSettings.textSettingsData.tintColor, normalizedCar.classColor, Settings.data.classColorStrength );
				}
			}

			// telemetry

			var telemetryString = string.Empty;
			var telemetryColor = Color.white;

			if ( IRSDK.normalizedSession.isInQualifyingSession )
			{
				if ( leadCarF2Time == normalizedCar.f2Time )
				{
					telemetryString = $"{leadCarF2Time:0.000}";
				}
				else
				{
					var deltaTime = normalizedCar.f2Time - leadCarF2Time;

					telemetryString = $"-{deltaTime:0.000}";
				}

				telemetryColor = Settings.data.telemetryTextColor;
			}
			else if ( normalizedCar.isOnPitRoad )
			{
				telemetryString = Settings.data.GetTranslation( "Pit", "PIT" );
				telemetryColor = Settings.data.pitTextColor;
			}
			else if ( normalizedCar.isOutOfCar )
			{
				telemetryString = Settings.data.GetTranslation( "Out", "OUT" );
				telemetryColor = Settings.data.outTextColor;
			}
			else if ( IRSDK.normalizedSession.isInRaceSession )
			{
				if ( ( IRSDK.normalizedData.sessionState == SessionState.StateRacing ) && normalizedCar.hasCrossedStartLine )
				{
					if ( !Settings.data.telemetryIsBetweenCars && normalizedCar.lapPositionRelativeToLeader >= 1.0f )
					{
						var wholeLapsDown = Math.Floor( normalizedCar.lapPositionRelativeToLeader );

						telemetryString = $"-{wholeLapsDown:0} {Settings.data.GetTranslation( "LapsAbbreviation", "L" )}";
					}
					else if ( !IRSDK.normalizedData.isUnderCaution )
					{
						var lapPosition = Settings.data.telemetryIsBetweenCars ? carInFrontLapPosition - normalizedCar.lapPosition : normalizedCar.lapPositionRelativeToLeader;

						if ( lapPosition > 0 )
						{
							if ( Settings.data.telemetryShowLaps )
							{
								telemetryString = $"-{lapPosition:0.000} {Settings.data.GetTranslation( "LapsAbbreviation", "L" )}";
							}
							else if ( Settings.data.telemetryShowDistance )
							{
								var distance = lapPosition * IRSDK.normalizedSession.trackLengthInMeters;

								if ( IRSDK.normalizedData.displayIsMetric )
								{
									var distanceString = $"{distance:0}";

									if ( distanceString != "0" )
									{
										telemetryString = $"-{distanceString} {Settings.data.GetTranslation( "MetersAbbreviation", "M" )}";
									}
								}
								else
								{
									distance *= 3.28084f;

									var distanceString = $"{distance:0}";

									if ( distanceString != "0" )
									{
										telemetryString = $"-{distanceString} {Settings.data.GetTranslation( "FeetAbbreviation", "FT" )}";
									}
								}
							}
							else
							{
								if ( ( normalizedCarInFront != null ) && ( normalizedCarInFront.checkpoints[ normalizedCar.checkpointIdx ] > 0 ) )
								{
									var deltaTime = normalizedCarInFront.checkpoints[ normalizedCar.checkpointIdx ] - normalizedCar.checkpoints[ normalizedCar.checkpointIdx ];

									var timeString = $"{deltaTime:0.00}";

									telemetryString = timeString;
								}
							}
						}
					}

					telemetryColor = Settings.data.telemetryTextColor;
				}
			}

			carInFrontLapPosition = normalizedCar.lapPosition;

			overlayPlaces[ placeIndex ].telemetry_Text.text = telemetryString;
			overlayPlaces[ placeIndex ].telemetry_Text.color = telemetryColor;

			// current target and speed

			if ( IRSDK.normalizedSession.isInRaceSession && ( IRSDK.normalizedData.sessionState == SessionState.StateRacing ) )
			{
				if ( IRSDK.normalizedData.camCarIdx == normalizedCar.carIdx )
				{
					overlayPlaces[ placeIndex ].highlight.SetActive( true );
					overlayPlaces[ placeIndex ].speed.SetActive( true );

					overlayPlaces[ placeIndex ].speed_Text.text = $"{normalizedCar.speedInMetersPerSecond * ( IRSDK.normalizedData.displayIsMetric ? 3.6f : 2.23694f ):0} {( IRSDK.normalizedData.displayIsMetric ? Settings.data.GetTranslation( "KPH", "KPH" ) : Settings.data.GetTranslation( "MPH", "MPH" ) )}";
				}
				else
				{
					overlayPlaces[ placeIndex ].highlight.SetActive( false );
					overlayPlaces[ placeIndex ].speed.SetActive( false );
				}
			}
			else
			{
				overlayPlaces[ placeIndex ].highlight.SetActive( false );
				overlayPlaces[ placeIndex ].speed.SetActive( false );
			}

			//

			if ( normalizedCarInFront == null || Settings.data.telemetryIsBetweenCars )
			{
				normalizedCarInFront = normalizedCar;
			}
		}

		// leaderboard background

		if ( showLeaderboardBackground )
		{
			leaderboardBackground.SetActive( true );

			if ( ( topSplitLastPosition + 1 ) != bottomSplitFirstPosition )
			{
				positionSplitter.SetActive( true );
			}
			else
			{
				positionSplitter.SetActive( false );
			}
		}
		else
		{
			leaderboardBackground.SetActive( false );
			positionSplitter.SetActive( false );
		}

		// reset cars

		foreach ( var normalizedCar in IRSDK.normalizedData.leaderboardSortedNormalizedCars )
		{
			if ( !normalizedCar.visibleOnLeaderboard )
			{
				normalizedCar.leaderboardPlacePositionOffsetIsValid = false;
				normalizedCar.leaderboardPlacePositionOffset = Vector2.zero;
				normalizedCar.leaderboardPlacePositionVelocity = 0;
			}
		}

		// reset places

		foreach ( var overlayPlace in overlayPlaces )
		{
			overlayPlace.gameObject.SetActive( overlayPlace.shouldBeVisible );
		}
	}
}
