
using System;

using UnityEngine;

using TMPro;

public class OverlayLeaderboard : MonoBehaviour
{
	public IPC ipc;

	public int classIndex;

	public GameObject enable;
	public GameObject background;
	public GameObject layer1;
	public GameObject layer2;
	public GameObject positionSplitter;
	public GameObject textLayer1;
	public GameObject textLayer2;
	public GameObject slotTemplate;

	[NonSerialized] public ImageSettings background_ImageSettings;
	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings positionSplitter_ImageSettings;

	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;

	[NonSerialized] public GameObject[] slots;

	[NonSerialized] public OverlayLeaderboardSlot[] overlayLeaderboardSlots;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		slotTemplate.SetActive( false );

		background_ImageSettings = background.GetComponent<ImageSettings>();
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		positionSplitter_ImageSettings = positionSplitter.GetComponent<ImageSettings>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();

		slots = new GameObject[ LiveData.MaxNumDrivers ];

		overlayLeaderboardSlots = new OverlayLeaderboardSlot[ LiveData.MaxNumDrivers ];

		for ( var slotIndex = 0; slotIndex < slots.Length; slotIndex++ )
		{
			slots[ slotIndex ] = Instantiate( slotTemplate );

			slots[ slotIndex ].transform.SetParent( slotTemplate.transform.parent, false );

			slots[ slotIndex ].SetActive( true );

			overlayLeaderboardSlots[ slotIndex ] = slots[ slotIndex ].GetComponent<OverlayLeaderboardSlot>();

			overlayLeaderboardSlots[ slotIndex ].preferredCar_ImageSettings.carIdx = slotIndex;
			overlayLeaderboardSlots[ slotIndex ].layer1_ImageSettings.carIdx = slotIndex;
			overlayLeaderboardSlots[ slotIndex ].layer2_ImageSettings.carIdx = slotIndex;
			overlayLeaderboardSlots[ slotIndex ].layer3_ImageSettings.carIdx = slotIndex;
			overlayLeaderboardSlots[ slotIndex ].currentTarget_ImageSettings.carIdx = slotIndex;
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

			positionSplitter_ImageSettings.SetPosition( new Vector2( liveDataLeaderboard.splitterPosition.x, -liveDataLeaderboard.splitterPosition.y ) );
			positionSplitter_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );

			textLayer1_Text.text = liveDataLeaderboard.textLayer1;
			textLayer2_Text.text = liveDataLeaderboard.textLayer2;

			// leaderboard

			for ( var slotIndex = 0; slotIndex < LiveData.MaxNumDrivers; slotIndex++ )
			{
				var liveDataLeaderboardSlot = liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ];

				var overlayLeaderboardSlot = overlayLeaderboardSlots[ slotIndex ];

				if ( !liveDataLeaderboardSlot.show )
				{
					overlayLeaderboardSlot.gameObject.SetActive( false );
				}
				else
				{
					overlayLeaderboardSlot.gameObject.SetActive( true );
				}

				// class colors

				overlayLeaderboardSlot.currentTarget_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );
				overlayLeaderboardSlot.layer1_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );
				overlayLeaderboardSlot.layer2_ImageSettings.SetClassColor( liveDataLeaderboard.classColor );

				// update slot offset

				overlayLeaderboardSlot.transform.localPosition = liveDataLeaderboardSlot.offset;

				//

				overlayLeaderboardSlot.textLayer1_Text.text = liveDataLeaderboardSlot.textLayer1;
				overlayLeaderboardSlot.textLayer1.SetColor( liveDataLeaderboardSlot.textLayer1Color );

				overlayLeaderboardSlot.textLayer2_Text.text = liveDataLeaderboardSlot.textLayer2;
				overlayLeaderboardSlot.textLayer2.SetColor( liveDataLeaderboardSlot.textLayer2Color );

				overlayLeaderboardSlot.textLayer3_Text.text = liveDataLeaderboardSlot.textLayer3;
				overlayLeaderboardSlot.textLayer3.SetColor( liveDataLeaderboardSlot.textLayer3Color );

				overlayLeaderboardSlot.textLayer4_Text.text = liveDataLeaderboardSlot.textLayer4;
				overlayLeaderboardSlot.textLayer4.SetColor( liveDataLeaderboardSlot.textLayer4Color );

				// current target

				overlayLeaderboardSlot.currentTarget.SetActive( liveDataLeaderboardSlot.showCurrentTarget );

				overlayLeaderboardSlot.currentTargetTextLayer1_Text.text = liveDataLeaderboardSlot.currentTargetTextLayer1;

				// preferred car

				overlayLeaderboardSlot.preferredCar.SetActive( liveDataLeaderboardSlot.showPreferredCar );
			}

			// splitter

			positionSplitter.SetActive( LiveData.Instance.liveDataLeaderboards[ classIndex ].showSplitter );
		}
	}
}
