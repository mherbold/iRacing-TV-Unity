
using System;

using UnityEngine;

using TMPro;

public class OverlayRaceResult : MonoBehaviour
{
	public IPC ipc;

	public int classIndex;

	public GameObject enable;
	public GameObject background;
	public GameObject layer1;
	public GameObject layer2;
	public GameObject textLayer1;
	public GameObject textLayer2;
	public GameObject slotTemplate;

	[NonSerialized] public ImageSettings background_ImageSettings;
	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;

	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;

	[NonSerialized] public GameObject[] slots;

	[NonSerialized] public OverlayRaceResultSlot[] overlayRaceResultsSlots;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		slotTemplate.SetActive( false );

		background_ImageSettings = background.GetComponent<ImageSettings>();
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();

		slots = new GameObject[ LiveData.MaxNumDrivers ];

		overlayRaceResultsSlots = new OverlayRaceResultSlot[ LiveData.MaxNumDrivers ];

		for ( var slotIndex = 0; slotIndex < slots.Length; slotIndex++ )
		{
			slots[ slotIndex ] = Instantiate( slotTemplate );

			slots[ slotIndex ].transform.SetParent( slotTemplate.transform.parent, false );

			slots[ slotIndex ].SetActive( true );

			overlayRaceResultsSlots[ slotIndex ] = slots[ slotIndex ].GetComponent<OverlayRaceResultSlot>();

			overlayRaceResultsSlots[ slotIndex ].preferredCar_ImageSettings.carIdx = slotIndex;
			overlayRaceResultsSlots[ slotIndex ].layer1_ImageSettings.carIdx = slotIndex;
			overlayRaceResultsSlots[ slotIndex ].layer2_ImageSettings.carIdx = slotIndex;
			overlayRaceResultsSlots[ slotIndex ].layer3_ImageSettings.carIdx = slotIndex;
		}
	}

	public void Update()
	{
		var liveDataRaceResult = LiveData.Instance.liveDataRaceResult;

		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.raceResultOn && liveDataRaceResult.show && ipc.isConnected && LiveData.Instance.isConnected );

		if ( ( indexSettings != IPC.indexSettings ) || ( indexLiveData != IPC.indexLiveData ) )
		{
			indexSettings = IPC.indexSettings;
			indexLiveData = IPC.indexLiveData;

			transform.localPosition = new Vector2( Settings.overlay.raceResultPosition.x, -Settings.overlay.raceResultPosition.y );

			background_ImageSettings.SetSize( liveDataRaceResult.backgroundSize );
			background_ImageSettings.SetClassColor( liveDataRaceResult.classColor );

			layer1_ImageSettings.SetClassColor( liveDataRaceResult.classColor );
			layer2_ImageSettings.SetClassColor( liveDataRaceResult.classColor );

			textLayer1_Text.text = liveDataRaceResult.textLayer1;
			textLayer2_Text.text = liveDataRaceResult.textLayer2;

			// leaderboard

			for ( var slotIndex = 0; slotIndex < LiveData.MaxNumDrivers; slotIndex++ )
			{
				var liveDataRaceResultSlot = liveDataRaceResult.liveDataRaceResultSlots[ slotIndex ];

				var overlayRaceResultSlot = overlayRaceResultsSlots[ slotIndex ];

				if ( !liveDataRaceResultSlot.show )
				{
					overlayRaceResultSlot.gameObject.SetActive( false );
				}
				else
				{
					overlayRaceResultSlot.gameObject.SetActive( true );
				}

				// class colors

				overlayRaceResultSlot.layer1_ImageSettings.SetClassColor( liveDataRaceResult.classColor );
				overlayRaceResultSlot.layer2_ImageSettings.SetClassColor( liveDataRaceResult.classColor );

				// update slot offset

				overlayRaceResultSlot.transform.localPosition = liveDataRaceResultSlot.offset;

				//

				overlayRaceResultSlot.textLayer1_Text.text = liveDataRaceResultSlot.textLayer1;
				overlayRaceResultSlot.textLayer1.SetColor( liveDataRaceResultSlot.textLayer1Color );

				overlayRaceResultSlot.textLayer2_Text.text = liveDataRaceResultSlot.textLayer2;
				overlayRaceResultSlot.textLayer2.SetColor( liveDataRaceResultSlot.textLayer2Color );

				overlayRaceResultSlot.textLayer3_Text.text = liveDataRaceResultSlot.textLayer3;
				overlayRaceResultSlot.textLayer3.SetColor( liveDataRaceResultSlot.textLayer3Color );

				overlayRaceResultSlot.textLayer4_Text.text = liveDataRaceResultSlot.textLayer4;
				overlayRaceResultSlot.textLayer4.SetColor( liveDataRaceResultSlot.textLayer4Color );

				// preferred car

				overlayRaceResultSlot.preferredCar.SetActive( liveDataRaceResultSlot.showPreferredCar );
			}
		}
	}
}
