
using System;
using System.IO;
using System.Text.RegularExpressions;

using UnityEngine;

using irsdkSharp.Serialization.Models.Session.DriverInfo;

public class NormalizedCar
{
	public int carIdx;
	public int driverIdx;

	public string userName;
	public string abbrevName;

	public string carNumber;
	public int carNumberRaw;

	public Color classColor;

	public bool includeInLeaderboard;
	public bool visibleOnLeaderboard;
	public bool hasCrossedStartLine;
	public bool isOnPitRoad;
	public bool isOutOfCar;

	public int officialPosition;
	public int leaderboardPosition;

	public float lapDistPctDelta;
	public float lapDistPct;

	public int lapPositionErrorCount;
	public float lapPosition;
	public float lapPositionRelativeToLeader;

	public int checkpointIdx;
	public double[] checkpoints;

	public float f2Time;

	public int qualifyingPosition;
	public float qualifyingTime;

	public float heat;
	public float distanceToCarInFrontInMeters;
	public float distanceToCarBehindInMeters;

	public float distanceMovedInMeters;
	public float speedInMetersPerSecond;

	public bool leaderboardPlacePositionOffsetIsValid;
	public Vector2 leaderboardPlacePositionOffset;
	public float leaderboardPlacePositionVelocity;

	public Texture2D carNumberTexture;
	public Texture2D carTexture;

	public NormalizedCar( int carIdx )
	{
		this.carIdx = carIdx;

		checkpoints = new double[ Settings.data.numberOfCheckpoints ];

		Reset();
	}

	public void Reset()
	{
		driverIdx = -1;

		userName = string.Empty;
		abbrevName = string.Empty;

		carNumber = string.Empty;
		carNumberRaw = 0;

		classColor = Color.white;

		includeInLeaderboard = false;
		visibleOnLeaderboard = false;
		hasCrossedStartLine = false;
		isOnPitRoad = false;
		isOutOfCar = false;

		officialPosition = int.MaxValue;
		leaderboardPosition = int.MaxValue;

		lapDistPctDelta = 0;
		lapDistPct = 0;

		lapPositionErrorCount = 0;
		lapPosition = 0;
		lapPositionRelativeToLeader = 0;

		checkpointIdx = -1;

		f2Time = 0;

		qualifyingPosition = int.MaxValue;
		qualifyingTime = 0;

		heat = 0;
		distanceToCarInFrontInMeters = float.MaxValue;
		distanceToCarBehindInMeters = float.MaxValue;

		distanceMovedInMeters = 0;
		speedInMetersPerSecond = 0;

		leaderboardPlacePositionOffsetIsValid = false;
		leaderboardPlacePositionOffset = Vector2.zero;
		leaderboardPlacePositionVelocity = 0;

		carNumberTexture = null;
		carTexture = null;
	}

	public void SessionChange()
	{
		var car = IRSDK.data.Cars[ carIdx ];

		hasCrossedStartLine = false;

		lapPositionErrorCount = 0;
		lapPosition = 0;

		lapDistPct = Math.Max( 0, car.CarIdxLapDistPct );

		checkpointIdx = -1;

		leaderboardPlacePositionOffsetIsValid = false;
		leaderboardPlacePositionOffset = Vector2.zero;
		leaderboardPlacePositionVelocity = 0;
	}

	public async void SessionUpdate()
	{
		if ( driverIdx == -1 )
		{
			includeInLeaderboard = false;

			DriverModel driver = null;

			for ( var driverIdx = 0; driverIdx < IRSDK.session.DriverInfo.Drivers.Count; driverIdx++ )
			{
				driver = IRSDK.session.DriverInfo.Drivers[ driverIdx ];

				if ( driver.CarIdx == carIdx )
				{
					this.driverIdx = driverIdx;
					break;
				}
			}

			if ( driverIdx != -1 )
			{
				userName = Regex.Replace( driver.UserName, @"[\d]", string.Empty );

				GenerateAbbrevName( false );

				carNumber = driver.CarNumber;
				carNumberRaw = driver.CarNumberRaw;

				ColorUtility.TryParseHtmlString( $"#{driver.CarClassColor[ 2.. ]}", out classColor );

				includeInLeaderboard = ( driver.IsSpectator == 0 ) && ( driver.CarIsPaceCar == "0" );

				if ( includeInLeaderboard )
				{
					foreach ( var session in IRSDK.session.SessionInfo.Sessions )
					{
						if ( ( session.SessionName == "QUALIFY" ) && ( session.ResultsPositions != null ) )
						{
							foreach ( var position in session.ResultsPositions )
							{
								if ( position.CarIdx == carIdx )
								{
									qualifyingPosition = position.Position;
									qualifyingTime = position.Time;
									break;
								}
							}
						}
					}

					var numberDesignMatch = Regex.Match( driver.CarNumberDesignStr, "(\\d+),(\\d+),(.{6}),(.{6}),(.{6})" );

					if ( numberDesignMatch.Success )
					{
						var imageSettingsData = Settings.data.GetImageSettingsData( "CarNumber" );

						var colorA = ( Settings.data.carNumberColorOverrideA != string.Empty ) ? Settings.data.carNumberColorOverrideA : numberDesignMatch.Groups[ 3 ].Value;
						var colorB = ( Settings.data.carNumberColorOverrideB != string.Empty ) ? Settings.data.carNumberColorOverrideB : numberDesignMatch.Groups[ 4 ].Value;
						var colorC = ( Settings.data.carNumberColorOverrideC != string.Empty ) ? Settings.data.carNumberColorOverrideC : numberDesignMatch.Groups[ 5 ].Value;

						var pattern = ( Settings.data.carNumberPatternOverride != string.Empty ) ? Settings.data.carNumberPatternOverride : numberDesignMatch.Groups[ 1 ].Value;
						var slant = ( Settings.data.carNumberSlantOverride != string.Empty ) ? Settings.data.carNumberSlantOverride : numberDesignMatch.Groups[ 2 ].Value;

						var url = $"http://localhost:32034/pk_number.png?size={imageSettingsData.size.y}&view=0&number={carNumber}&numPat={pattern}&numCol={colorA},{colorB},{colorC}&numSlnt={slant}";

						carNumberTexture = await RemoteTexture.Get( url );
					}

					var carDesignMatch = Regex.Match( driver.CarDesignStr, "(\\d+),(.{6}),(.{6}),(.{6}),?(.{6})?" );

					if ( numberDesignMatch.Success && carDesignMatch.Success )
					{
						var licColor = driver.LicColor[ 2.. ];
						var carPath = driver.CarPath.Replace( " ", "%5C" );
						var customCarTgaFileName = $"{Settings.data.customPaintsDirectory}\\{driver.CarPath}\\car_{driver.UserID}.tga".Replace( " ", "%5C" );

						if ( !File.Exists( customCarTgaFileName ) )
						{
							customCarTgaFileName = string.Empty;
						}

						var url = $"http://localhost:32034/pk_car.png?size=2&view=1&licCol={licColor}&club={driver.ClubID}&sponsors={driver.CarSponsor_1},{driver.CarSponsor_2}&numPat={numberDesignMatch.Groups[ 1 ].Value}&numCol={numberDesignMatch.Groups[ 3 ].Value},{numberDesignMatch.Groups[ 4 ].Value},{numberDesignMatch.Groups[ 5 ].Value}&numSlnt={numberDesignMatch.Groups[ 2 ].Value}&number={carNumber}&carPath={carPath}&carPat={carDesignMatch.Groups[ 1 ].Value}&carCol={carDesignMatch.Groups[ 2 ].Value},{carDesignMatch.Groups[ 3 ].Value},{carDesignMatch.Groups[ 4 ].Value}&carRimType=2&carRimCol={carDesignMatch.Groups[ 5 ].Value}&carCustPaint={customCarTgaFileName}";

						carTexture = await RemoteTexture.Get( url );
					}
				}
			}
		}
	}

	public void Update()
	{
		if ( !includeInLeaderboard )
		{
			return;
		}

		var car = IRSDK.data.Cars[ carIdx ];

		isOnPitRoad = car.CarIdxOnPitRoad;
		isOutOfCar = car.CarIdxLapDistPct == -1;

		if ( !isOutOfCar )
		{
			f2Time = Math.Max( 0, car.CarIdxF2Time );

			var newCarIdxLapDistPct = Math.Max( 0, car.CarIdxLapDistPct );

			lapDistPctDelta = newCarIdxLapDistPct - lapDistPct;
			lapDistPct = newCarIdxLapDistPct;

			if ( lapDistPctDelta > 0.5f )
			{
				lapDistPctDelta -= 1.0f;
			}
			else if ( lapDistPctDelta < -0.5f )
			{
				lapDistPctDelta += 1.0f;
			}

			if ( !hasCrossedStartLine )
			{
				if ( ( car.CarIdxLap >= 2 ) || ( ( car.CarIdxLap >= 1 ) && ( newCarIdxLapDistPct > 0 ) && ( newCarIdxLapDistPct < 0.5f ) ) )
				{
					hasCrossedStartLine = true;
				}
			}

			if ( hasCrossedStartLine )
			{
				var newLapPosition = car.CarIdxLap + car.CarIdxLapDistPct - 1;

				lapPositionErrorCount++;

				if ( ( lapPositionErrorCount >= 10 ) || ( Math.Abs( newLapPosition - lapPosition ) < 0.05f ) )
				{
					lapPositionErrorCount = 0;
					lapPosition = newLapPosition;
				}
				else
				{
					lapPosition += lapDistPctDelta;
				}

				var checkpointIdx = (int) Math.Floor( lapDistPct * Settings.data.numberOfCheckpoints ) % Settings.data.numberOfCheckpoints;

				if ( checkpointIdx != this.checkpointIdx )
				{
					this.checkpointIdx = checkpointIdx;

					checkpoints[ checkpointIdx ] = IRSDK.normalizedData.sessionTime;
				}
			}
			else
			{
				lapPosition = 0;
			}

			officialPosition = car.CarIdxPosition;

			distanceMovedInMeters = lapDistPctDelta * IRSDK.normalizedSession.trackLengthInMeters;
			speedInMetersPerSecond = distanceMovedInMeters / (float) IRSDK.normalizedData.sessionTimeDelta;
		}
	}

	public void GenerateAbbrevName( bool includeFirstNameInitial )
	{
		var userNameParts = userName.Split( " " );

		if ( userNameParts.Length == 0 )
		{
			abbrevName = "---";
		}
		else if ( userNameParts.Length == 1 )
		{
			abbrevName = userNameParts[ 0 ];
		}
		else if ( includeFirstNameInitial )
		{
			abbrevName = $"{userNameParts[ 0 ][ ..1 ]}. {userNameParts[ ^1 ]}";
		}
		else
		{
			abbrevName = userNameParts[ ^1 ];
		}
	}

	public static Comparison<NormalizedCar> LapPositionComparison = delegate ( NormalizedCar object1, NormalizedCar object2 )
	{
		if ( object1.includeInLeaderboard && object2.includeInLeaderboard )
		{
			if ( object1.lapPosition == object2.lapPosition )
			{
				return object1.carIdx.CompareTo( object2.carIdx );
			}
			else
			{
				return object2.lapPosition.CompareTo( object1.lapPosition );
			}
		}
		else if ( object1.includeInLeaderboard )
		{
			return -1;
		}
		else if ( object2.includeInLeaderboard )
		{
			return 1;
		}
		else
		{
			return 0;
		}
	};

	public static Comparison<NormalizedCar> HeatComparison = delegate ( NormalizedCar object1, NormalizedCar object2 )
	{
		if ( object1.includeInLeaderboard && object2.includeInLeaderboard )
		{
			if ( object1.heat == object2.heat )
			{
				return object1.carIdx.CompareTo( object2.carIdx );
			}
			else
			{
				return object2.heat.CompareTo( object1.heat );
			}
		}
		else if ( object1.includeInLeaderboard )
		{
			return -1;
		}
		else if ( object2.includeInLeaderboard )
		{
			return 1;
		}
		else
		{
			return 0;
		}
	};

	public static Comparison<NormalizedCar> LeaderboardPositionComparison = delegate ( NormalizedCar object1, NormalizedCar object2 )
	{
		if ( object1.includeInLeaderboard && object2.includeInLeaderboard )
		{
			if ( object1.leaderboardPosition == object2.leaderboardPosition )
			{
				return object1.carIdx.CompareTo( object2.carIdx );
			}
			else
			{
				return object1.leaderboardPosition.CompareTo( object2.leaderboardPosition );
			}
		}
		else if ( object1.includeInLeaderboard )
		{
			return -1;
		}
		else if ( object2.includeInLeaderboard )
		{
			return 1;
		}
		else
		{
			return 0;
		}
	};
}
