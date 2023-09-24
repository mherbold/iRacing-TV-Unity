
using System;

using UnityEngine;

using TMPro;

public class OverlayTrainer : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject[] graph;
	public GameObject mask;
	public GameObject message;

	[NonSerialized] public LineRenderer[] graph_lineRenderer;
	[NonSerialized] public RectTransform mask_RectTransform;
	[NonSerialized] public TextMeshProUGUI message_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		graph_lineRenderer = new LineRenderer[ graph.Length ];

		for ( var i = 0; i < graph.Length; i++ )
		{
			graph_lineRenderer[ i ] = graph[ i ].GetComponent<LineRenderer>();
		}

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

			if ( LiveData.Instance.liveDataTrainer.drawVectorList != null )
			{
				var numLists = LiveData.Instance.liveDataTrainer.drawVectorList.Length;

				for ( var i = 0; i < numLists; i++ )
				{
					graph_lineRenderer[ i ].positionCount = LiveData.Instance.liveDataTrainer.drawVectorList[ i ].Length;

					graph_lineRenderer[ i ].SetPositions( LiveData.Instance.liveDataTrainer.drawVectorList[ i ] );
				}
			}

			message_Text.text = LiveData.Instance.liveDataTrainer.message;
		}
	}
}
