
using System.Collections;

using UnityEngine;

public class StreamingTextures : MonoBehaviour
{
	public static StreamedTexture seriesLogoStreamedTexture;

	public static StreamedTexture[] carNumberStreamedTexture = new StreamedTexture[ LiveDataLeaderboard.MaxNumPlaces ];
	public static StreamedTexture[] carStreamedTexture = new StreamedTexture[ LiveDataLeaderboard.MaxNumPlaces ];
	public static StreamedTexture[] helmetStreamedTexture = new StreamedTexture[ LiveDataLeaderboard.MaxNumPlaces ];
	public static StreamedTexture[] driverStreamedTexture = new StreamedTexture[ LiveDataLeaderboard.MaxNumPlaces ];

	public static bool requestsPending;

	static StreamingTextures()
	{
		seriesLogoStreamedTexture = new StreamedTexture();

		for ( int placeIndex = 0; placeIndex < LiveDataLeaderboard.MaxNumPlaces; placeIndex++ )
		{
			carNumberStreamedTexture[ placeIndex ] = new StreamedTexture();
			carStreamedTexture[ placeIndex ] = new StreamedTexture();
			helmetStreamedTexture[ placeIndex ] = new StreamedTexture();
			driverStreamedTexture[ placeIndex ] = new StreamedTexture();
		}

		requestsPending = false;
	}

	public IEnumerator Start()
	{
		yield return StartCoroutine( UpdateTexturesCoroutine() );
	}

	public IEnumerator UpdateTexturesCoroutine()
	{
		while ( true )
		{
			if ( requestsPending )
			{
				yield return seriesLogoStreamedTexture.Fetch();

				for ( int placeIndex = 0; placeIndex < LiveDataLeaderboard.MaxNumPlaces; placeIndex++ )
				{
					yield return carNumberStreamedTexture[ placeIndex ].Fetch();
					yield return carStreamedTexture[ placeIndex ].Fetch();
					yield return helmetStreamedTexture[ placeIndex ].Fetch();
					yield return driverStreamedTexture[ placeIndex ].Fetch();
				}
			}

			yield return null;
		}
	}

	public static void CheckForUpdates()
	{
		var requestsPending = false;

		if ( seriesLogoStreamedTexture.textureUrl != LiveData.Instance.seriesLogoTextureUrl )
		{
			seriesLogoStreamedTexture.ChangeTexture( LiveData.Instance.seriesLogoTextureUrl );

			requestsPending = true;
		}

		for ( var placeIndex = 0; placeIndex < LiveDataLeaderboard.MaxNumPlaces; placeIndex++ )
		{
			if ( carNumberStreamedTexture[ placeIndex ].textureUrl != LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ].carNumberTextureUrl )
			{
				carNumberStreamedTexture[ placeIndex ].ChangeTexture( LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ].carNumberTextureUrl );

				requestsPending = true;
			}

			if ( carStreamedTexture[ placeIndex ].textureUrl != LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ].carTextureUrl )
			{
				carStreamedTexture[ placeIndex ].ChangeTexture( LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ].carTextureUrl );

				requestsPending = true;
			}

			if ( helmetStreamedTexture[ placeIndex ].textureUrl != LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ].helmetTextureUrl )
			{
				helmetStreamedTexture[ placeIndex ].ChangeTexture( LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ].helmetTextureUrl );

				requestsPending = true;
			}

			if ( driverStreamedTexture[ placeIndex ].textureUrl != LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ].driverTextureUrl )
			{
				driverStreamedTexture[ placeIndex ].ChangeTexture( LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardPlaces[ placeIndex ].driverTextureUrl );

				requestsPending = true;
			}
		}

		if ( requestsPending )
		{
			StreamingTextures.requestsPending = true;
		}
	}
}
