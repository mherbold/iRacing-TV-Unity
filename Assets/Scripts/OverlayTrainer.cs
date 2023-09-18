
using System;

using UnityEngine;

using TMPro;

public class OverlayTrainer : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject graphA;
	public GameObject graphB;
	public GameObject mask;
	public GameObject message;

	[NonSerialized] public LineRenderer graphA_lineRenderer;
	[NonSerialized] public LineRenderer graphB_lineRenderer;
	[NonSerialized] public RectTransform mask_RectTransform;
	[NonSerialized] public TextMeshProUGUI message_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		graphA_lineRenderer = graphA.GetComponent<LineRenderer>();
		graphB_lineRenderer = graphB.GetComponent<LineRenderer>();
		mask_RectTransform = mask.GetComponent<RectTransform>();
		message_Text = message.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && Settings.overlay.trainerEnabled && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.trainerPosition.x, -Settings.overlay.trainerPosition.y );

			mask_RectTransform.localPosition = new Vector2( Settings.overlay.trainerSize.x, -Settings.overlay.trainerSize.y ) * 0.5f;
			mask_RectTransform.localScale = new Vector2( Settings.overlay.trainerSize.x, Settings.overlay.trainerSize.y - 16 );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			if ( LiveData.Instance.liveDataTrainer.drawVectorListA != null )
			{
				graphA_lineRenderer.positionCount = LiveData.Instance.liveDataTrainer.drawVectorListA.Length;

				graphA_lineRenderer.SetPositions( LiveData.Instance.liveDataTrainer.drawVectorListA );
			}

			if ( LiveData.Instance.liveDataTrainer.drawVectorListB != null )
			{
				graphB_lineRenderer.positionCount = LiveData.Instance.liveDataTrainer.drawVectorListB.Length;

				graphB_lineRenderer.SetPositions( LiveData.Instance.liveDataTrainer.drawVectorListB );
			}

			message_Text.text = LiveData.Instance.liveDataTrainer.message;
		}
	}
}
