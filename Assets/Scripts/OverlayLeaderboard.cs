
using System;

using UnityEngine;

public class OverlayLeaderboard : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject leaderboardBackground;
	public GameObject positionSplitter;
	public GameObject slotTemplate;

	[NonSerialized] public ImageSettings leaderboardBackground_ImageSettings;
	[NonSerialized] public ImageSettings positionSplitter_ImageSettings;

	[NonSerialized] public GameObject[] slots;

	[NonSerialized] public OverlayLeaderboardSlot[] overlayLeaderboardSlots;

	public void Awake()
	{
		slotTemplate.SetActive( false );

		leaderboardBackground_ImageSettings = leaderboardBackground.GetComponent<ImageSettings>();
		positionSplitter_ImageSettings = positionSplitter.GetComponent<ImageSettings>();

		slots = new GameObject[ LiveDataLeaderboard.MaxNumSlots ];

		overlayLeaderboardSlots = new OverlayLeaderboardSlot[ LiveDataLeaderboard.MaxNumSlots ];

		for ( var slotIndex = 0; slotIndex < slots.Length; slotIndex++ )
		{
			slots[ slotIndex ] = Instantiate( slotTemplate );

			slots[ slotIndex ].transform.SetParent( slotTemplate.transform.parent, false );

			slots[ slotIndex ].SetActive( true );

			overlayLeaderboardSlots[ slotIndex ] = slots[ slotIndex ].GetComponent<OverlayLeaderboardSlot>();

			overlayLeaderboardSlots[ slotIndex ].carNumber_ImageSettings.carIdx = slotIndex;
		}
	}

	public void Start()
	{
		SettingsUpdated();
	}

	public void Update()
	{
		enable.SetActive( Settings.overlay.leaderboardEnabled && LiveData.Instance.liveDataLeaderboard.show && ipc.isConnected && LiveData.Instance.isConnected );
	}

	public void SettingsUpdated()
	{
		transform.localPosition = new Vector2( Settings.overlay.leaderboardPosition.x, -Settings.overlay.leaderboardPosition.y );
	}

	public void LiveDataUpdated()
	{
		leaderboardBackground_ImageSettings.SetSize( LiveData.Instance.liveDataLeaderboard.backgroundSize );
		positionSplitter_ImageSettings.SetPosition( LiveData.Instance.liveDataLeaderboard.splitterPosition );

		// leaderboard

		for ( var slotIndex = 0; slotIndex < LiveDataLeaderboard.MaxNumSlots; slotIndex++ )
		{
			var liveDataLeaderboardSlot = LiveData.Instance.liveDataLeaderboard.liveDataLeaderboardSlots[ slotIndex ];

			var overlayLeaderboardSlot = overlayLeaderboardSlots[ slotIndex ];

			if ( !liveDataLeaderboardSlot.show )
			{
				overlayLeaderboardSlot.gameObject.SetActive( false );
			}
			else
			{
				overlayLeaderboardSlot.gameObject.SetActive( true );
			}

			// update slot offset

			overlayLeaderboardSlot.transform.localPosition = liveDataLeaderboardSlot.offset;

			// update position text

			overlayLeaderboardSlot.position_Text.text = liveDataLeaderboardSlot.positionText;
			overlayLeaderboardSlot.position_Text.color = liveDataLeaderboardSlot.positionColor;

			// driver name

			overlayLeaderboardSlot.driverName_Text.text = liveDataLeaderboardSlot.driverNameText;
			overlayLeaderboardSlot.driverName_Text.color = liveDataLeaderboardSlot.driverNameColor;

			// telemetry

			overlayLeaderboardSlot.telemetry_Text.text = liveDataLeaderboardSlot.telemetryText;
			overlayLeaderboardSlot.telemetry_Text.color = liveDataLeaderboardSlot.telemetryColor;

			// highlight

			overlayLeaderboardSlot.highlight.SetActive( liveDataLeaderboardSlot.showHighlight );
			overlayLeaderboardSlot.speed.SetActive( liveDataLeaderboardSlot.showHighlight );

			overlayLeaderboardSlot.speed_Text.text = liveDataLeaderboardSlot.speedText;
		}

		// splitter

		positionSplitter.SetActive( LiveData.Instance.liveDataLeaderboard.showSplitter );
	}
}
