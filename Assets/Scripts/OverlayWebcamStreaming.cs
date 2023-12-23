
using System;

using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class OverlayWebcamStreaming : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject localWebcam;
	public GameObject remoteWebcam;

	public BidirectionalStreaming bidirectionalStreaming;

	[NonSerialized] public RectTransform localWebcam_RectTransform;
	[NonSerialized] public RectTransform remoteWebcam_RectTransform;

	[NonSerialized] public long indexSettings;

	public void Awake()
	{
		localWebcam_RectTransform = localWebcam.GetComponent<RectTransform>();
		remoteWebcam_RectTransform = remoteWebcam.GetComponent<RectTransform>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && Settings.overlay.hudEnabled && ipc.isConnected && LiveData.Instance.isConnected && bidirectionalStreaming.started );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			localWebcam_RectTransform.localPosition = new Vector2( Settings.overlay.hudLocalWebcamPosition.x, -Settings.overlay.hudLocalWebcamPosition.y );
			localWebcam_RectTransform.sizeDelta = new Vector2( Settings.overlay.hudLocalWebcamSize.x, Settings.overlay.hudLocalWebcamSize.y );

			remoteWebcam_RectTransform.localPosition = new Vector2( Settings.overlay.hudRemoteWebcamPosition.x, -Settings.overlay.hudRemoteWebcamPosition.y );
			remoteWebcam_RectTransform.sizeDelta = new Vector2( Settings.overlay.hudRemoteWebcamSize.x, Settings.overlay.hudRemoteWebcamSize.y );
		}
	}
}
