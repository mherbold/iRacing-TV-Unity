
using System;

using UnityEngine;

public class OverlayPitLane : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject carTemplate;

	[NonSerialized] public RectTransform rectTransform;

	[NonSerialized] public GameObject[] cars;

	[NonSerialized] public OverlayPitLaneCar[] overlayPitLaneCars;

	[NonSerialized] public long indexSettings;

	public void Awake()
	{
		carTemplate.SetActive( false );

		rectTransform = GetComponent<RectTransform>();

		cars = new GameObject[ LiveData.MaxNumDrivers ];

		overlayPitLaneCars = new OverlayPitLaneCar[ LiveData.MaxNumDrivers ];

		for ( var carIndex = 0; carIndex < cars.Length; carIndex++ )
		{
			cars[ carIndex ] = Instantiate( carTemplate );

			cars[ carIndex ].transform.SetParent( carTemplate.transform.parent, false );

			cars[ carIndex ].SetActive( true );

			overlayPitLaneCars[ carIndex ] = cars[ carIndex ].GetComponent<OverlayPitLaneCar>();

			overlayPitLaneCars[ carIndex ].layer1_ImageSettings.carIdx = carIndex;
			overlayPitLaneCars[ carIndex ].layer2_ImageSettings.carIdx = carIndex;
		}
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.pitLaneOn && LiveData.Instance.liveDataPitLane.show && !LiveData.Instance.liveDataIntro.show && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			rectTransform.localPosition = new Vector2( Settings.overlay.pitLanePosition.x, -Settings.overlay.pitLanePosition.y );
		}

		for ( var carIndex = 0; carIndex < LiveData.MaxNumDrivers; carIndex++ )
		{
			var liveDataPitLaneCar = LiveData.Instance.liveDataPitLane.liveDataPitLaneCars[ carIndex ];

			if ( liveDataPitLaneCar != null )
			{
				var overlayPitLaneCar = overlayPitLaneCars[ carIndex ];

				if ( !liveDataPitLaneCar.show )
				{
					overlayPitLaneCar.gameObject.SetActive( false );
				}
				else
				{
					overlayPitLaneCar.gameObject.SetActive( true );
				}

				overlayPitLaneCar.transform.localPosition = liveDataPitLaneCar.offset;

				overlayPitLaneCar.currentTarget.SetActive( liveDataPitLaneCar.showHighlight );

				overlayPitLaneCar.textLayer1_Text.text = liveDataPitLaneCar.textLayer1;
			}
		}
	}
}
