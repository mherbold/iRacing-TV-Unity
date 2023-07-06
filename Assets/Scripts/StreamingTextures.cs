
using System.Collections;

using UnityEngine;

public class StreamingTextures : MonoBehaviour
{
	public static StreamedTexture seriesLogoStreamedTexture;

	public static StreamedTexture[] carNumberStreamedTexture = new StreamedTexture[ LiveDataLeaderboard.MaxNumSlots ];
	public static StreamedTexture[] carStreamedTexture = new StreamedTexture[ LiveDataLeaderboard.MaxNumSlots ];
	public static StreamedTexture[] helmetStreamedTexture = new StreamedTexture[ LiveDataLeaderboard.MaxNumSlots ];
	public static StreamedTexture[] driverStreamedTexture = new StreamedTexture[ LiveDataLeaderboard.MaxNumSlots ];

	public static bool requestsPending;

	static StreamingTextures()
	{
		seriesLogoStreamedTexture = new StreamedTexture();

		for ( int slotIndex = 0; slotIndex < LiveDataLeaderboard.MaxNumSlots; slotIndex++ )
		{
			carNumberStreamedTexture[ slotIndex ] = new StreamedTexture();
			carStreamedTexture[ slotIndex ] = new StreamedTexture();
			helmetStreamedTexture[ slotIndex ] = new StreamedTexture();
			driverStreamedTexture[ slotIndex ] = new StreamedTexture();
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

				for ( int slotIndex = 0; slotIndex < LiveDataLeaderboard.MaxNumSlots; slotIndex++ )
				{
					yield return carNumberStreamedTexture[ slotIndex ].Fetch();
					yield return carStreamedTexture[ slotIndex ].Fetch();
					yield return helmetStreamedTexture[ slotIndex ].Fetch();
					yield return driverStreamedTexture[ slotIndex ].Fetch();
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

		for ( var slotIndex = 0; slotIndex < LiveDataLeaderboard.MaxNumSlots; slotIndex++ )
		{
			if ( carNumberStreamedTexture[ slotIndex ].textureUrl != LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ].carNumberTextureUrl )
			{
				carNumberStreamedTexture[ slotIndex ].ChangeTexture( LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ].carNumberTextureUrl );

				requestsPending = true;
			}

			if ( carStreamedTexture[ slotIndex ].textureUrl != LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ].carTextureUrl )
			{
				carStreamedTexture[ slotIndex ].ChangeTexture( LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ].carTextureUrl );

				requestsPending = true;
			}

			if ( helmetStreamedTexture[ slotIndex ].textureUrl != LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ].helmetTextureUrl )
			{
				helmetStreamedTexture[ slotIndex ].ChangeTexture( LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ].helmetTextureUrl );

				requestsPending = true;
			}

			if ( driverStreamedTexture[ slotIndex ].textureUrl != LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ].driverTextureUrl )
			{
				driverStreamedTexture[ slotIndex ].ChangeTexture( LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ].driverTextureUrl );

				requestsPending = true;
			}
		}

		if ( requestsPending )
		{
			StreamingTextures.requestsPending = true;
		}
	}
}
