
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

	[NonSerialized] public GameObject[] drivers;

	[NonSerialized] public OverlayIntroDriver[] overlayIntroDrivers;

	[NonSerialized] public Animator[] animators;

	[NonSerialized] public bool wasShown = false;

	public void Awake()
	{
		driverTemplate.SetActive( false );

		drivers = new GameObject[ LiveDataLeaderboard.MaxNumPlaces ];

		overlayIntroDrivers = new OverlayIntroDriver[ LiveDataLeaderboard.MaxNumPlaces ];

		animators = new Animator[ LiveDataLeaderboard.MaxNumPlaces ];

		for ( var driverIndex = 0; driverIndex < LiveDataLeaderboard.MaxNumPlaces; driverIndex++ )
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
	}

	public void LiveDataUpdated()
	{
		if ( !LiveData.Instance.liveDataIntro.show )
		{
			background.SetActive( false );
			driversA.SetActive( false );
			driversB.SetActive( false );

			wasShown = false;
		}
		else
		{
			background.SetActive( true );
			driversA.SetActive( true );
			driversB.SetActive( true );

			for ( var driverIndex = 0; driverIndex < LiveDataLeaderboard.MaxNumPlaces; driverIndex++ )
			{
				var liveDataIntroDriver = LiveData.Instance.liveDataIntro.liveDataIntroDrivers[ driverIndex ];

				if ( !wasShown )
				{
					var overlayIntroDriver = overlayIntroDrivers[ driverIndex ];

					overlayIntroDriver.background_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.suit_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.car_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.helmet_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.statsBackground_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.carNumber_ImageSettings.carIdx = liveDataIntroDriver.carIdx;

					overlayIntroDriver.position_Text.text = liveDataIntroDriver.placeText;
					overlayIntroDriver.driverName_Text.text = liveDataIntroDriver.driverNameText;
					overlayIntroDriver.qualifyingTime_Text.text = liveDataIntroDriver.qualifyingTimeText;

					animators[ driverIndex ].Play( "Idle" );
				}

				var row = (int) Math.Floor( driverIndex / 2.0 ) + 1;

				if ( row == LiveData.Instance.liveDataIntro.currentRow )
				{
					if ( liveDataIntroDriver.show )
					{
						animators[ driverIndex ].SetBool( "Start", true );
					}
				}
			}

			wasShown = true;
		}
	}
}
