
using System;

using UnityEngine;

public class OverlayStartLights : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject readyLights;
	public GameObject setLights;
	public GameObject goLights;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.startLightsOn && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.startLightsPosition.x, -Settings.overlay.startLightsPosition.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			readyLights.SetActive( LiveData.Instance.liveDataStartLights.showReady );
			setLights.SetActive( LiveData.Instance.liveDataStartLights.showSet );
			goLights.SetActive( LiveData.Instance.liveDataStartLights.showGo );
		}
	}
}
