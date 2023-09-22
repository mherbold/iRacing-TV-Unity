
using System;

using UnityEngine;
using UnityEngine.UI;

public class Border : MonoBehaviour
{
	public IPC ipc;

	[NonSerialized] public static GameObject border_GameObject;

	[NonSerialized] public Image border_Image;

	[NonSerialized] public long indexSettings;

	public void Awake()
	{
		if ( border_GameObject == null )
		{
			border_GameObject = gameObject;

			gameObject.SetActive( false );
		}
		else
		{
			border_Image = GetComponent<Image>();

			border_Image.enabled = false;
		}
	}

	public void Update()
	{
		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			border_Image.enabled = Settings.overlay.showBorders;
		}
	}
}
