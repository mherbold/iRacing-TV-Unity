
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayNotConnected : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject message;

	[NonSerialized] public TextMeshProUGUI message_Text;

	public void Awake()
	{
		message_Text = message.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		if ( ipc.isConnected )
		{
			if ( LiveData.Instance.isConnected )
			{
				enable.SetActive( false );
			}
			else
			{
				message_Text.text = "iRacing not running.";

				enable.SetActive( true );
			}
		}
		else
		{
			message_Text.text = "iRacing TV Controller is not running.";

			enable.SetActive( true );
		}
	}
}
