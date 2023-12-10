
using Unity.RenderStreaming;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
	public RawImage rawImage;
	public SignalingManager signalingManager;

	private WebCamTexture webCamTexture;

	void Start()
	{
		var devices = WebCamTexture.devices;

		webCamTexture = new WebCamTexture( devices[ 0 ].name );

		rawImage.texture = webCamTexture;

		webCamTexture.Play();
	}

	void Update()
	{
		if ( LiveData.Instance.liveDataWebcamStreaming.enabled )
		{
			var signalingSettings = new WebSocketSignalingSettings( LiveData.Instance.liveDataWebcamStreaming.webserverURL );

			signalingManager.SetSignalingSettings( signalingSettings );

			signalingManager.Run();
		}
	}
}
