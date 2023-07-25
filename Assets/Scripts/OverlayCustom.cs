
using UnityEngine;

public class OverlayCustom : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject[] layers;

	public void Update()
	{
		enable.SetActive( ipc.isConnected && LiveData.Instance.isConnected );

		for ( var i = 0; i < layers.Length; i++ )
		{
			layers[ i ].SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.customLayerOn[ i ] );
		}
	}
}
