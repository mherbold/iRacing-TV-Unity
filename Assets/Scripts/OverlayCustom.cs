
using UnityEngine;

public class OverlayCustom : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;

	public void Update()
	{
		enable.SetActive( ipc.isConnected && LiveData.Instance.isConnected );
	}
}
