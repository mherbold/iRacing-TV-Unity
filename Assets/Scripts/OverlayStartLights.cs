
using System;

using UnityEngine;

using TMPro;

public class OverlayStartLights : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject readyLights;
	public GameObject setLights;
	public GameObject goLights;

	public void Start()
	{
		SettingsUpdated();
	}

	public void Update()
	{
		enable.SetActive( Settings.overlay.startLightsEnabled && ipc.isConnected && LiveData.Instance.isConnected );
	}

	public void SettingsUpdated()
	{
		transform.localPosition = new Vector2( Settings.overlay.startLightsPosition.x, -Settings.overlay.startLightsPosition.y );
	}

	public void LiveDataUpdated()
	{
		readyLights.SetActive( LiveData.Instance.liveDataStartLights.showReady );
		setLights.SetActive( LiveData.Instance.liveDataStartLights.showSet );
		goLights.SetActive( LiveData.Instance.liveDataStartLights.showGo );
	}
}
