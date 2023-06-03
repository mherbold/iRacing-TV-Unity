
using System;
using System.Collections.Generic;

using irsdkSharp.Serialization.Enums.Fastest;

public class NormalizedData
{
	public const int MaxNumCars = 63;

	public double sessionTimeDelta;
	public double sessionTime;
	public uint sessionFlags;

	public bool displayIsMetric;

	public bool isInTimedRace;
	public bool isUnderCaution;

	public SessionState sessionState;

	public double sessionTimeTotal;
	public double sessionTimeRemaining;

	public int sessionLapsTotal;
	public int sessionLapsRemaining;

	public int currentLap;
	public int numLeaderboardCars;

	public int camCarIdx;
	public int camGroupNumber;
	public int camCameraNumber;

	public int radioTransmitCarIdx;

	public NormalizedCar[] normalizedCars = new NormalizedCar[ MaxNumCars ];
	public List<NormalizedCar> leaderboardSortedNormalizedCars = new( MaxNumCars );
	public List<NormalizedCar> heatSortedNormalizedCars = new( MaxNumCars );

	public NormalizedData()
	{
		for ( var i = 0; i < MaxNumCars; i++ )
		{
			normalizedCars[ i ] = new NormalizedCar( i );

			leaderboardSortedNormalizedCars.Add( normalizedCars[ i ] );
			heatSortedNormalizedCars.Add( normalizedCars[ i ] );
		}

		Reset();
	}

	public void Reset()
	{
		sessionTimeDelta = 0;
		sessionTime = 0;
		sessionFlags = 0;

		displayIsMetric = false;

		isInTimedRace = false;
		isUnderCaution = false;

		sessionState = SessionState.StateInvalid;

		sessionTimeTotal = 0;
		sessionTimeRemaining = 0;

		sessionLapsTotal = 0;
		sessionLapsRemaining = 0;

		currentLap = 0;
		numLeaderboardCars = 0;

		camCarIdx = 0;
		camGroupNumber = 0;
		camCameraNumber = 0;

		radioTransmitCarIdx = -1;

		foreach ( var normalizedCar in normalizedCars )
		{
			normalizedCar.Reset();
		}
	}

	public void SessionChange()
	{
		foreach ( var normalizedCar in normalizedCars )
		{
			normalizedCar.SessionChange();
		}
	}

	public void SessionUpdate()
	{
		numLeaderboardCars = 0;

		foreach ( var normalizedCar in normalizedCars )
		{
			normalizedCar.SessionUpdate();

			if ( normalizedCar.includeInLeaderboard )
			{
				numLeaderboardCars++;
			}
		}
	}

	public void Update()
	{
		sessionTimeDelta = Math.Round( ( IRSDK.data.SessionTime - sessionTime ) / ( 1.0f / 60.0f ) ) * ( 1.0f / 60.0f );
		sessionTime = IRSDK.data.SessionTime;

		if ( IRSDK.normalizedSession.isReplay )
		{
			sessionFlags = SessionFlagsPlayback.Playback( IRSDK.normalizedSession.sessionNumber, IRSDK.normalizedData.sessionTime );
		}
		else
		{
			sessionFlags = (uint) IRSDK.data.SessionFlags;

			SessionFlagsPlayback.Record( IRSDK.normalizedSession.sessionNumber, IRSDK.normalizedData.sessionTime, sessionFlags );
		}

		displayIsMetric = IRSDK.data.DisplayUnits == 1;

		isInTimedRace = IRSDK.data.SessionLapsTotal == 32767;
		isUnderCaution = ( sessionFlags & ( (uint) SessionFlags.CautionWaving | (uint) SessionFlags.Caution | (uint) SessionFlags.YellowWaving | (uint) SessionFlags.Yellow ) ) != 0;

		if ( IRSDK.normalizedSession.isInRaceSession )
		{
			sessionState = (SessionState) IRSDK.data.SessionState;
		}
		else
		{
			sessionState = SessionState.StateInvalid;
		}

		sessionTimeTotal = IRSDK.data.SessionTimeTotal;
		sessionTimeRemaining = Math.Max( 0, IRSDK.data.SessionTimeRemain );

		sessionLapsTotal = IRSDK.data.SessionLapsTotal;
		sessionLapsRemaining = Math.Max( 0, IRSDK.data.SessionLapsRemain );

		currentLap = sessionLapsTotal - sessionLapsRemaining;

		camGroupNumber = IRSDK.data.CamGroupNumber;
		camCameraNumber = IRSDK.data.CamCameraNumber;
		camCarIdx = IRSDK.data.CamCarIdx;

		radioTransmitCarIdx = IRSDK.data.RadioTransmitCarIdx;

		if ( sessionTimeDelta > 0 )
		{
			foreach ( var normalizedCar in normalizedCars )
			{
				normalizedCar.Update();
			}

			foreach ( var normalizedCar in normalizedCars )
			{
				normalizedCar.heat = 0;
				normalizedCar.distanceToCarInFrontInMeters = float.MaxValue;
				normalizedCar.distanceToCarBehindInMeters = float.MaxValue;

				foreach ( var otherNormalizedCar in normalizedCars )
				{
					if ( normalizedCar != otherNormalizedCar )
					{
						if ( normalizedCar.includeInLeaderboard && !normalizedCar.isOnPitRoad && !normalizedCar.isOutOfCar )
						{
							if ( otherNormalizedCar.includeInLeaderboard && !otherNormalizedCar.isOnPitRoad && !otherNormalizedCar.isOutOfCar )
							{
								var signedDistanceToOtherCar = otherNormalizedCar.lapDistPct - normalizedCar.lapDistPct;

								if ( signedDistanceToOtherCar > 0.5f )
								{
									signedDistanceToOtherCar -= 1.0f;
								}
								else if ( signedDistanceToOtherCar < -0.5f )
								{
									signedDistanceToOtherCar += 1.0f;
								}

								var signedDistanceToOtherCarInMeters = signedDistanceToOtherCar * IRSDK.normalizedSession.trackLengthInMeters;

								var x = Math.Abs( signedDistanceToOtherCarInMeters / 100.0f );

								x = Math.Min( x, 1 );

								x = x * -2 + 1;

								x *= (float) Math.PI / 3.5f;

								var y = (float) Math.Tan( x );

								y /= 2.5079206753254076751418219566729f;

								y += 0.5f;

								normalizedCar.heat += y;

								if ( signedDistanceToOtherCarInMeters >= 0.0f )
								{
									normalizedCar.distanceToCarInFrontInMeters = Math.Min( normalizedCar.distanceToCarInFrontInMeters, signedDistanceToOtherCarInMeters );
								}
								else
								{
									normalizedCar.distanceToCarBehindInMeters = Math.Min( normalizedCar.distanceToCarBehindInMeters, -signedDistanceToOtherCarInMeters );
								}
							}
						}
					}
				}
			}

			leaderboardSortedNormalizedCars.Sort( NormalizedCar.LapPositionComparison );

			heatSortedNormalizedCars.Sort( NormalizedCar.HeatComparison );

			var leaderboardPosition = 1;
			var leaderLapPosition = leaderboardSortedNormalizedCars[ 0 ].lapPosition;

			foreach ( var normalizedCar in leaderboardSortedNormalizedCars )
			{
				if ( !IRSDK.normalizedSession.isInRaceSession || isUnderCaution || ( sessionState == SessionState.StateCheckered ) )
				{
					if ( normalizedCar.officialPosition >= 1 )
					{
						normalizedCar.leaderboardPosition = normalizedCar.officialPosition;
					}
					else
					{
						normalizedCar.leaderboardPosition = int.MaxValue;
					}
				}
				else if ( normalizedCar.hasCrossedStartLine )
				{
					normalizedCar.leaderboardPosition = leaderboardPosition++;
				}
				else if ( sessionState == SessionState.StateRacing )
				{
					if ( normalizedCar.officialPosition > 0 )
					{
						normalizedCar.leaderboardPosition = normalizedCar.officialPosition;
					}
					else
					{
						normalizedCar.leaderboardPosition = int.MaxValue;
					}
				}
				else
				{
					normalizedCar.leaderboardPosition = normalizedCar.qualifyingPosition;
				}

				normalizedCar.lapPositionRelativeToLeader = leaderLapPosition - normalizedCar.lapPosition;
			}

			leaderboardSortedNormalizedCars.Sort( NormalizedCar.LeaderboardPositionComparison );

			leaderboardPosition = 1;

			foreach ( var normalizedCar in leaderboardSortedNormalizedCars )
			{
				normalizedCar.leaderboardPosition = leaderboardPosition++;
			}
		}
	}

	public NormalizedCar FindNormalizedCarByCarIdx( int carIdx )
	{
		foreach ( var normalizedCar in normalizedCars )
		{
			if ( normalizedCar.carIdx == carIdx )
			{
				return normalizedCar;
			}
		}

		return null;
	}
}
