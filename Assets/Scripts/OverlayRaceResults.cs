
using System;

using UnityEngine;

using TMPro;

public class OverlayRaceResults : MonoBehaviour
{
	public IPC ipc;

	public int classIndex;

	public GameObject enable;
	public GameObject background;
	public GameObject layer1;
	public GameObject layer2;
	public GameObject className;
	public GameObject shortClassName;
	public GameObject slotTemplate;

	[NonSerialized] public ImageSettings background_ImageSettings;
	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;

	[NonSerialized] public TextMeshProUGUI className_Text;
	[NonSerialized] public TextMeshProUGUI shortClassName_Text;

	[NonSerialized] public GameObject[] slots;

	[NonSerialized] public OverlayRaceResultsSlot[] overlayRaceResultsSlots;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		slotTemplate.SetActive( false );

		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();

		className_Text = className.GetComponent<TextMeshProUGUI>();
		shortClassName_Text = shortClassName.GetComponent<TextMeshProUGUI>();

		slots = new GameObject[ LiveData.MaxNumDrivers ];

		overlayRaceResultsSlots = new OverlayRaceResultsSlot[ LiveData.MaxNumDrivers ];

		for ( var slotIndex = 0; slotIndex < slots.Length; slotIndex++ )
		{
			slots[ slotIndex ] = Instantiate( slotTemplate );

			slots[ slotIndex ].transform.SetParent( slotTemplate.transform.parent, false );

			slots[ slotIndex ].SetActive( true );

			overlayRaceResultsSlots[ slotIndex ] = slots[ slotIndex ].GetComponent<OverlayRaceResultsSlot>();

			overlayRaceResultsSlots[ slotIndex ].layer1_ImageSettings.carIdx = slotIndex;
			overlayRaceResultsSlots[ slotIndex ].layer2_ImageSettings.carIdx = slotIndex;
		}
	}

	public void Update()
	{
		if ( ( LiveData.Instance.liveDataLeaderboards == null ) || ( classIndex >= LiveData.Instance.liveDataLeaderboards.Length ) )
		{
			enable.SetActive( false );

			return;
		}

		var liveDataLeaderboard = LiveData.Instance.liveDataLeaderboards[ classIndex ];

		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.leaderboardOn && liveDataLeaderboard.show && !LiveData.Instance.liveDataIntro.show && ipc.isConnected && LiveData.Instance.isConnected );

		if ( ( indexSettings != IPC.indexSettings ) || ( indexLiveData != IPC.indexLiveData ) )
		{
			indexSettings = IPC.indexSettings;
			indexLiveData = IPC.indexLiveData;

			transform.localPosition = new Vector2( Settings.overlay.leaderboardPosition.x + liveDataLeaderboard.offset.x, -( Settings.overlay.leaderboardPosition.y + liveDataLeaderboard.offset.y ) );

			background_ImageSettings.SetSize( liveDataLeaderboard.backgroundSize );
			background_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );

			layer1_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );
			layer2_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );

			className_Text.text = liveDataLeaderboard.textLayer1;
			shortClassName_Text.text = liveDataLeaderboard.textLayer2;

			// leaderboard

			for ( var slotIndex = 0; slotIndex < LiveData.MaxNumDrivers; slotIndex++ )
			{
				var liveDataLeaderboardSlot = liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ];

				var overlayLeaderboardSlot = overlayRaceResultsSlots[ slotIndex ];

				if ( !liveDataLeaderboardSlot.show )
				{
					overlayLeaderboardSlot.gameObject.SetActive( false );
				}
				else
				{
					overlayLeaderboardSlot.gameObject.SetActive( true );
				}

				// class colors

				overlayLeaderboardSlot.layer1_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );
				overlayLeaderboardSlot.layer2_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );

				// update slot offset

				overlayLeaderboardSlot.transform.localPosition = liveDataLeaderboardSlot.offset;

				// update position text

				overlayLeaderboardSlot.position_Text.text = liveDataLeaderboardSlot.textLayer1;
				overlayLeaderboardSlot.position.SetColor( liveDataLeaderboardSlot.textLayer1Color );

				// driver name

				overlayLeaderboardSlot.driverName_Text.text = liveDataLeaderboardSlot.textLayer3;
				overlayLeaderboardSlot.driverName.SetColor( liveDataLeaderboardSlot.textLayer3Color );

				// telemetry

				overlayLeaderboardSlot.telemetry_Text.text = liveDataLeaderboardSlot.textLayer4;
				overlayLeaderboardSlot.telemetry.SetColor( liveDataLeaderboardSlot.textLayer4Color );
			}
		}
	}
}
