
using UnityEngine;

using Valve.VR;

public class SteamVROverlay : MonoBehaviour
{
	public const string OverlayKey = "iRacing-TV";
	public const string OverlayName = "iRacing-TV";

	public new Camera camera;
	public RenderTexture renderTexture;

	public ulong overlayHandle = 0;
	public Texture_t texture;

	public bool activated = false;
	public bool restartRequired = false;

	public void Activate( bool activate )
	{
		if ( restartRequired || ( activated == activate ) )
		{
			return;
		}

		if ( activate )
		{
			camera.targetTexture = renderTexture;

			EVRInitError evrInitError = EVRInitError.None;

			OpenVR.Init( ref evrInitError, EVRApplicationType.VRApplication_Overlay );

			if ( evrInitError != EVRInitError.None )
			{
				restartRequired = true;

				Debug.Log( $"OpenVR.Init failed with error: {evrInitError}" );

				return;
			}

			var evrOverlayError = OpenVR.Overlay.FindOverlay( OverlayKey, ref overlayHandle );

			if ( evrOverlayError != EVROverlayError.None )
			{
				if ( evrOverlayError == EVROverlayError.UnknownOverlay )
				{
					evrOverlayError = OpenVR.Overlay.CreateOverlay( OverlayKey, OverlayName, ref overlayHandle );

					if ( evrOverlayError != EVROverlayError.None )
					{
						restartRequired = true;

						Debug.Log( $"OpenVR.Overlay.CreateOverlay failed with error: {evrOverlayError}" );

						return;
					}
				}
				else
				{
					restartRequired = true;

					Debug.Log( $"OpenVR.Overlay.FindOverlay failed with error: {evrOverlayError}" );

					return;
				}
			}

			evrOverlayError = OpenVR.Overlay.ShowOverlay( overlayHandle );

			if ( evrOverlayError != EVROverlayError.None )
			{
				restartRequired = true;

				Debug.Log( $"OpenVR.Overlay.ShowOverlay failed with error: {evrOverlayError}" );

				return;
			}

			texture = new Texture_t
			{
				handle = renderTexture.GetNativeTexturePtr(),
				eType = ETextureType.DirectX,
				eColorSpace = EColorSpace.Gamma
			};
		}
		else
		{
			camera.targetTexture = null;
		}

		activated = activate;
	}

	public void Update()
	{
		Activate( LiveData.Instance.liveDataSteamVr.enabled );

		if ( activated && !restartRequired )
		{
			var evrOverlayError = OpenVR.Overlay.SetOverlayWidthInMeters( overlayHandle, LiveData.Instance.liveDataSteamVr.width );

			if ( evrOverlayError != EVROverlayError.None )
			{
				restartRequired = true;

				Debug.Log( $"OpenVR.Overlay.SetOverlayWidthInMeters failed with error: {evrOverlayError}" );
			}
			else
			{
				evrOverlayError = OpenVR.Overlay.SetOverlayTexture( overlayHandle, ref texture );

				if ( evrOverlayError != EVROverlayError.None )
				{
					restartRequired = true;

					Debug.Log( $"OpenVR.Overlay.SetOverlayTexture failed with error: {evrOverlayError}" );
				}
				else
				{
					var vrTextureBounds = new VRTextureBounds_t
					{
						uMin = 0.0f,
						uMax = 1.0f,

						vMin = 1.0f,
						vMax = 0.0f
					};

					evrOverlayError = OpenVR.Overlay.SetOverlayTextureBounds( overlayHandle, ref vrTextureBounds );

					if ( evrOverlayError != EVROverlayError.None )
					{
						restartRequired = true;

						Debug.Log( $"OpenVR.Overlay.SetOverlayTextureBounds failed with error: {evrOverlayError}" );
					}
					else
					{
						var hmdMatrix = new HmdMatrix34_t
						{
							m0 = 1.0f,
							m1 = 0.0f,
							m2 = 0.0f,
							m3 = LiveData.Instance.liveDataSteamVr.position.x,

							m4 = 0.0f,
							m5 = 1.0f,
							m6 = 0.0f,
							m7 = LiveData.Instance.liveDataSteamVr.position.y,

							m8 = 0.0f,
							m9 = 0.0f,
							m10 = 1.0f,
							m11 = LiveData.Instance.liveDataSteamVr.position.z
						};

						evrOverlayError = OpenVR.Overlay.SetOverlayTransformAbsolute( overlayHandle, ETrackingUniverseOrigin.TrackingUniverseSeated, ref hmdMatrix );

						if ( evrOverlayError != EVROverlayError.None )
						{
							restartRequired = true;

							Debug.Log( $"OpenVR.Overlay.SetOverlayTransformAbsolute failed with error: {evrOverlayError}" );
						}
						else
						{
							evrOverlayError = OpenVR.Overlay.SetOverlayCurvature( overlayHandle, LiveData.Instance.liveDataSteamVr.curvature );

							if ( evrOverlayError != EVROverlayError.None )
							{
								restartRequired = true;

								Debug.Log( $"OpenVR.Overlay.SetOverlayCurvature failed with error: {evrOverlayError}" );
							}
						}
					}
				}
			}
		}
	}
}
