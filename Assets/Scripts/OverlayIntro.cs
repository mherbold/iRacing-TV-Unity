
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

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		driverTemplate.SetActive( false );

		driversA_RectTransform = driversA.GetComponent<RectTransform>();
		driversB_RectTransform = driversB.GetComponent<RectTransform>();

		drivers = new GameObject[ LiveData.MaxNumDrivers ];
		overlayIntroDrivers = new OverlayIntroDriver[ LiveData.MaxNumDrivers ];
		animators = new Animator[ LiveData.MaxNumDrivers ];

		for ( var driverIndex = 0; driverIndex < LiveData.MaxNumDrivers; driverIndex++ )
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

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.introOn && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			driversA_RectTransform.localPosition = new Vector2( Settings.overlay.introLeftPosition.x, -Settings.overlay.introLeftPosition.y );
			driversB_RectTransform.localPosition = new Vector2( Settings.overlay.introRightPosition.x, -Settings.overlay.introRightPosition.y );

			driversA_RectTransform.localScale = new Vector3( Settings.overlay.introLeftScale, Settings.overlay.introLeftScale, Settings.overlay.introLeftScale );
			driversB_RectTransform.localScale = new Vector3( Settings.overlay.introRightScale, Settings.overlay.introRightScale, Settings.overlay.introRightScale );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			if ( !LiveData.Instance.liveDataIntro.show )
			{
				if ( wasActive )
				{
					wasActive = false;

					background.SetActive( false );
				}

				if ( wasShown )
				{
					wasShown = false;
				}

				for ( var driverIndex = 0; driverIndex < LiveData.MaxNumDrivers; driverIndex++ )
				{
					drivers[ driverIndex ].SetActive( false );
				}
			}
			else
			{
				for ( var driverIndex = 0; driverIndex < LiveData.MaxNumDrivers; driverIndex++ )
				{
					var liveDataIntroDriver = LiveData.Instance.liveDataIntro.liveDataIntroDrivers[ driverIndex ];

					var overlayIntroDriver = overlayIntroDrivers[ driverIndex ];

					overlayIntroDriver.layer1_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.layer2_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.layer3_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.layer4_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.layer5_ImageSettings.carIdx = liveDataIntroDriver.carIdx;
					overlayIntroDriver.layer6_ImageSettings.carIdx = liveDataIntroDriver.carIdx;

					overlayIntroDriver.textLayer1_Text.text = liveDataIntroDriver.textLayer1;
					overlayIntroDriver.textLayer3_Text.text = liveDataIntroDriver.textLayer2;
					overlayIntroDriver.textLayer2_Text.text = liveDataIntroDriver.textLayer3;
					overlayIntroDriver.textLayer4_Text.text = liveDataIntroDriver.textLayer4;
					overlayIntroDriver.textLayer5_Text.text = liveDataIntroDriver.textLayer5;
					overlayIntroDriver.textLayer6_Text.text = liveDataIntroDriver.textLayer6;
				}

				if ( !wasActive )
				{
					wasActive = true;

					background.SetActive( true );
				}

				if ( !wasShown )
				{
					wasShown = true;

					for ( var driverIndex = 0; driverIndex < LiveData.MaxNumDrivers; driverIndex++ )
					{
						drivers[ driverIndex ].transform.localPosition = Vector3.zero;
						drivers[ driverIndex ].transform.localRotation = Quaternion.identity;
					}
				}

				for ( var driverIndex = 0; driverIndex < LiveData.MaxNumDrivers; driverIndex++ )
				{
					var liveDataIntroDriver = LiveData.Instance.liveDataIntro.liveDataIntroDrivers[ driverIndex ];

					if ( liveDataIntroDriver.show )
					{
						drivers[ driverIndex ].SetActive( true );

						animators[ driverIndex ].SetBool( "Start", true );
						animators[ driverIndex ].SetInteger( "InAnimation", ( ( driverIndex & 1 ) == 0 ) ? Settings.overlay.introLeftInAnimationNumber : Settings.overlay.introRightInAnimationNumber );
						animators[ driverIndex ].SetInteger( "OutAnimation", ( ( driverIndex & 1 ) == 0 ) ? Settings.overlay.introLeftOutAnimationNumber : Settings.overlay.introRightOutAnimationNumber );
						animators[ driverIndex ].SetFloat( "InTime", 1.0f / Settings.overlay.introInTime );
						animators[ driverIndex ].SetFloat( "HoldTime", 1.0f / Settings.overlay.introHoldTime );
						animators[ driverIndex ].SetFloat( "OutTime", 1.0f / Settings.overlay.introOutTime );
					}
					else
					{
						drivers[ driverIndex ].SetActive( false );

						drivers[ driverIndex ].transform.localPosition = Vector3.zero;
						drivers[ driverIndex ].transform.localRotation = Quaternion.identity;
					}
				}
			}
		}
	}
}
