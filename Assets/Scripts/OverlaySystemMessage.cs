
using System;

using UnityEngine;

using TMPro;

public class OverlaySystemMessage : MonoBehaviour
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
				if ( LiveData.Instance.systemMessage == string.Empty )
				{
					enable.SetActive( false );
				}
				else
				{
					message_Text.text = LiveData.Instance.systemMessage;

					enable.SetActive( true );
				}
			}
			else
			{
				message_Text.text = "iRacing not running";

				enable.SetActive( true );
			}
		}
		else
		{
			message_Text.text = "iRacing-TV Controller is not running";

			enable.SetActive( true );
		}
	}
}
