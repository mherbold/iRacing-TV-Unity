
using System;

using UnityEngine;

public class OverlayIntro : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject background;
	public GameObject driversA;
	public GameObject driversB;
	public GameObject driverTemplate;

	[NonSerialized] public RectTransform driversA_RectTransform;
	[NonSerialized] public RectTransform driversB_RectTransform;

	[NonSerialized] public GameObject[] drivers;
	[NonSerialized] public OverlayIntroDriver[] overlayIntroDrivers;
	[NonSerialized] public Animator[] animators;

	[NonSerialized] public bool wasActive = true;
	[NonSerialized] public bool wasShown = false;

	public void Awake()
	{
		driverTemplate.SetActive( false );

		driversA_RectTransform = driversA.GetComponent<RectTransform>();
		driversB_RectTransform = driversB.GetComponent<RectTransform>();

		drivers = new GameObject[ LiveDataLeaderboard.MaxNumSlots ];
		overlayIntroDrivers = new OverlayIntroDriver[ LiveDataLeaderboard.MaxNumSlots ];
		animators = new Animator[ LiveDataLeaderboard.MaxNumSlots ];

		for ( var driverIndex = 0; driverIndex < LiveDataLeaderboard.MaxNumSlots; driverIndex++ )
		{
			drivers[ driverIndex ] = Instantiate( driverTemplate );

			var parent = ( ( driverIndex & 1 ) == 0 ) ? driversA : driversB;

			drivers[ driverIndex ].transform.SetParent( parent.transform, false );
			drivers[ driverIndex ].transform.SetAsFirstSibling();
			drivers[ driverIndex ].SetActive( true );

			overlayIntroDrivers[ driverIndex ] = drivers[ driverIndex ].GetComponent<OverlayIntroDriver>();

			animators[ driverIndex ] = drivers[ driverIndex ].GetComponent<Animator>();
		}
	}

	public void Start()
	{
		SettingsUpdated();
	}

	public void Update()
	{
		enable.SetActive( Settings.overlay.introEnabled && ipc.isConnected && LiveData.Instance.isConnected );
	}

	public void SettingsUpdated()
	{
		driversA_RectTransform.localPosition = new Vector2( Settings.overlay.introLeftPosition.x, -Settings.overlay.introLeftPosition.y );
		driversB_RectTransform.localPosition = new Vector2( Settings.overlay.introRightPosition.x, -Settings.overlay.introRightPosition.y );

		driversA_RectTransform.localScale = new Vector3( Settings.overlay.introLeftScale, Settings.overlay.introLeftScale, 1 );
		driversB_RectTransform.localScale = new Vector3( Settings.overlay.introRightScale, Settings.overlay.introRightScale, 1 );
	}

	public void LiveDataUpdated()
	{
		if ( !LiveData.Instance.liveDataIntro.show )
		{
			if ( wasActive )
			{
				wasActive = false;

				background.SetActive( false );
				driversA.SetActive( false );
				driversB.SetActive( false );
			}

			if ( wasShown )
			{
				wasShown = false;
			}
		}
		else
		{
			if ( !wasActive )
			{
				wasActive = true;

				background.SetActive( true );
				driversA.SetActive( true );
				driversB.SetActive( true );
			}

			if ( !wasShown )
			{
				wasShown = true;

				for ( var qualifyingPosition = 0; qualifyingPosition < LiveDataLeaderboard.MaxNumSlots; qualifyingPosition++ )
				{
					var liveDataIntroDriver = LiveData.Instance.liveDataIntro.liveDataIntroDrivers[ qualifyingPosition ];

					var overlayIntroDriver = overlayIntroDrivers[ qualifyingPosition ];

					overlayIntroDriver.background_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.suit_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.car_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.helmet_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.statsBackground_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.carNumber_ImageSettings.carIdx = liveDataIntroDriver.carIdx;

					overlayIntroDriver.position_Text.text = liveDataIntroDriver.positionText;
					overlayIntroDriver.driverName_Text.text = liveDataIntroDriver.driverNameText;
					overlayIntroDriver.qualifyingTime_Text.text = liveDataIntroDriver.qualifyingTimeText;
				}
			}

			for ( var qualifyingPosition = 0; qualifyingPosition < LiveDataLeaderboard.MaxNumSlots; qualifyingPosition++ )
			{
				var liveDataIntroDriver = LiveData.Instance.liveDataIntro.liveDataIntroDrivers[ qualifyingPosition ];

				if ( liveDataIntroDriver.show )
				{
					drivers[ qualifyingPosition ].SetActive( true );

					animators[ qualifyingPosition ].SetBool( "Start", true );
					animators[ qualifyingPosition ].SetInteger( "Animation", Settings.overlay.introAnimationNumber );
					animators[ qualifyingPosition ].SetFloat( "Speed", Settings.overlay.introAnimationSpeed );
				}
				else
				{
					drivers[ qualifyingPosition ].SetActive( false );
				}
			}
		}
	}
}
