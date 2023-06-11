
using System;

using UnityEngine;

using TMPro;

using irsdkSharp.Serialization.Enums.Fastest;

public class OverlayRaceStatus : MonoBehaviour
{
	public GameObject sessionName;
	public GameObject lapsRemaining;
	public GameObject blackLight;
	public GameObject greenLight;
	public GameObject whiteLight;
	public GameObject yellowLight;
	public GameObject units;
	public GameObject currentLap;

	[NonSerialized] public TextMeshProUGUI sessionName_Text;
	[NonSerialized] public TextMeshProUGUI lapsRemaining_Text;
	[NonSerialized] public TextMeshProUGUI units_Text;
	[NonSerialized] public TextMeshProUGUI currentLap_Text;

	public void Awake()
	{
		sessionName_Text = sessionName.GetComponent<TextMeshProUGUI>();
		lapsRemaining_Text = lapsRemaining.GetComponent<TextMeshProUGUI>();
		units_Text = units.GetComponent<TextMeshProUGUI>();
		currentLap_Text = currentLap.GetComponent<TextMeshProUGUI>();
	}

	public void Start()
	{
		transform.localPosition = new Vector2( Settings.overlay.raceStatusOverlayPosition.x, -Settings.overlay.raceStatusOverlayPosition.y );

		if ( !Settings.overlay.showRaceStatusOverlay )
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

		// session name

		sessionName_Text.text = IRSDK.normalizedSession.sessionName;

		// laps remaining

		if ( IRSDK.normalizedData.isInTimedRace || !IRSDK.normalizedSession.isInRaceSession )
		{
			lapsRemaining_Text.text = GetTimeString( IRSDK.normalizedData.sessionTimeRemaining, false );
		}
		else if ( IRSDK.normalizedData.sessionLapsRemaining == 0 )
		{
			lapsRemaining_Text.text = Settings.overlay.GetTranslation( "FinalLap", "FINAL LAP" );
		}
		else
		{
			var lapsRemaining = Math.Min( IRSDK.normalizedData.sessionLapsTotal, IRSDK.normalizedData.sessionLapsRemaining + 1 );

			lapsRemaining_Text.text = lapsRemaining.ToString() + " " + Settings.overlay.GetTranslation( "ToGo", "TO GO" );
		}

		// lights

		GameObject activeLight;

		if ( IRSDK.normalizedData.isUnderCaution )
		{
			activeLight = yellowLight;
		}
		else if ( IRSDK.normalizedSession.isInRaceSession && ( IRSDK.normalizedData.sessionState != SessionState.StateRacing ) )
		{
			activeLight = blackLight;
		}
		else if ( IRSDK.normalizedSession.isInRaceSession && ( ( IRSDK.normalizedData.sessionLapsRemaining == 0 ) || ( ( IRSDK.normalizedData.sessionFlags & (uint) SessionFlags.White ) != 0 ) ) )
		{
			activeLight = whiteLight;
		}
		else
		{
			activeLight = greenLight;
		}

		blackLight.SetActive( activeLight == blackLight );
		greenLight.SetActive( activeLight == greenLight );
		whiteLight.SetActive( activeLight == whiteLight );
		yellowLight.SetActive( activeLight == yellowLight );

		// lap / time string

		if ( IRSDK.normalizedData.isInTimedRace || !IRSDK.normalizedSession.isInRaceSession )
		{
			units_Text.text = Settings.overlay.GetTranslation( "Time", "TIME" );
		}
		else
		{
			units_Text.text = Settings.overlay.GetTranslation( "Lap", "LAP" );
		}

		// current lap

		if ( IRSDK.normalizedData.isInTimedRace || !IRSDK.normalizedSession.isInRaceSession )
		{
			currentLap_Text.text = GetTimeString( IRSDK.normalizedData.sessionTimeTotal - IRSDK.normalizedData.sessionTimeRemaining, false ) + " | " + GetTimeString( IRSDK.normalizedData.sessionTimeTotal, false );
		}
		else
		{
			currentLap_Text.text = IRSDK.normalizedData.currentLap.ToString() + " | " + IRSDK.normalizedData.sessionLapsTotal.ToString();
		}
	}

	public string GetTimeString( double timeInSeconds, bool includeMilliseconds )
	{
		TimeSpan time = TimeSpan.FromSeconds( timeInSeconds );

		if ( time.Hours > 0 )
		{
			return time.ToString( @"h\:mm\:ss" );
		}
		else if ( includeMilliseconds )
		{
			if ( time.Minutes > 0 )
			{
				return time.ToString( @"m\:ss\.fff" );
			}
			else
			{
				return time.ToString( @"ss\.fff" );
			}
		}
		else
		{
			return time.ToString( @"m\:ss" );
		}
	}
}
