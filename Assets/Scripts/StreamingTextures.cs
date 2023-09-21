
using System.Collections;

using UnityEngine;

public class StreamingTextures : MonoBehaviour
{
	public static StreamedTexture seriesLogoStreamedTexture;

	public static StreamedTexture[] carNumberStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] carStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] helmetStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] driverStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] memberImageStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];
	public static StreamedTexture[] memberClubRegionStreamedTexture = new StreamedTexture[ LiveData.MaxNumDrivers ];

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
			memberImageStreamedTexture[ driverIndex ] = new StreamedTexture();
			memberClubRegionStreamedTexture[ driverIndex ] = new StreamedTexture();
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
					yield return memberImageStreamedTexture[ driverIndex ].Fetch();
					yield return memberClubRegionStreamedTexture[ driverIndex ].Fetch();
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

			if ( memberImageStreamedTexture[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].memberImageUrl )
			{
				memberImageStreamedTexture[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].memberImageUrl );

				requestsPending = true;
			}

			if ( memberClubRegionStreamedTexture[ driverIndex ].textureUrl != LiveData.Instance.liveDataDrivers[ driverIndex ].memberClubRegionUrl )
			{
				memberClubRegionStreamedTexture[ driverIndex ].ChangeTexture( LiveData.Instance.liveDataDrivers[ driverIndex ].memberClubRegionUrl );

				requestsPending = true;
			}
		}

		if ( requestsPending )
		{
			StreamingTextures.requestsPending = true;
		}
	}
}
