
using System.Collections;

using UnityEngine;

public class StreamingTextures : MonoBehaviour
{
	public static StreamedTexture seriesLogoStreamedTexture;

	public static StreamedTexture[] carNumberStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] carStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] helmetStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] driverStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] memberClubRegionStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] memberIdStreamedTexture_A = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] memberIdStreamedTexture_B = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] memberIdStreamedTexture_C = new StreamedTexture[ LiveData.MaxNumDrivers ];

	public static bool requestsPending;

	static StreamingTextures()
	{
		seriesLogoStreamedTexture = new StreamedTexture();

		for ( int driverIndex = 0; driverIndex < LiveData.MaxNumDrivers; driverIndex++ )
		{
			carNumberStreamedTexture[ driverIndex ] = new StreamedTexture();
			carStreamedTexture[ driverIndex ] = new StreamedTexture();
			helmetStreamedTexture[ driverIndex ] = new StreamedTexture();
			driverStreamedTexture[ driverIndex ] = new StreamedTexture();
			memberClubRegionStreamedTexture[ driverIndex ] = new StreamedTexture();
			memberIdStreamedTexture_A[ driverIndex ] = new StreamedTexture();
			memberIdStreamedTexture_B[ driverIndex ] = new StreamedTexture();
			memberIdStreamedTexture_C[ driverIndex ] = new StreamedTexture();
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

				for ( int driverIndex = 0; driverIndex < LiveData.MaxNumDrivers; driverIndex++ )
				{
					yield return carNumberStreamedTexture[ driverIndex ].Fetch();
					yield return carStreamedTexture[ driverIndex ].Fetch();
					yield return helmetStreamedTexture[ driverIndex ].Fetch();
					yield return driverStreamedTexture[ driverIndex ].Fetch();
					yield return memberClubRegionStreamedTexture[ driverIndex ].Fetch();
					yield return memberIdStreamedTexture_A[ driverIndex ].Fetch();
					yield return memberIdStreamedTexture_B[ driverIndex ].Fetch();
					yield return memberIdStreamedTexture_C[ driverIndex ].Fetch();
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

		for ( var driverIndex = 0; driverIndex < LiveData.MaxNumDrivers; driverIndex++ )
		{
			if ( carNumberStreamedTexture[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].carNumberTextureUrl )
			{
				carNumberStreamedTexture[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].carNumberTextureUrl );

				requestsPending = true;
			}

			if ( carStreamedTexture[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].carTextureUrl )
			{
				carStreamedTexture[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].carTextureUrl );

				requestsPending = true;
			}

			if ( helmetStreamedTexture[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].helmetTextureUrl )
			{
				helmetStreamedTexture[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].helmetTextureUrl );

				requestsPending = true;
			}

			if ( driverStreamedTexture[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].driverTextureUrl )
			{
				driverStreamedTexture[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].driverTextureUrl );

				requestsPending = true;
			}

			if ( memberClubRegionStreamedTexture[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].memberClubRegionTextureUrl )
			{
				memberClubRegionStreamedTexture[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].memberClubRegionTextureUrl );

				requestsPending = true;
			}

			if ( memberIdStreamedTexture_A[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].memberIdTextureUrl_A )
			{
				memberIdStreamedTexture_A[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].memberIdTextureUrl_A );

				requestsPending = true;
			}

			if ( memberIdStreamedTexture_B[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].memberIdTextureUrl_B )
			{
				memberIdStreamedTexture_B[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].memberIdTextureUrl_B );

				requestsPending = true;
			}

			if ( memberIdStreamedTexture_C[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].memberIdTextureUrl_C )
			{
				memberIdStreamedTexture_C[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].memberIdTextureUrl_C );

				requestsPending = true;
			}
		}

		if ( requestsPending )
		{
			StreamingTextures.requestsPending = true;
		}
	}
}
