
using System;

using UnityEngine;

using TMPro;

public class OverlayVoiceOf : MonoBehaviour
{
	public GameObject animationRoot;
	public GameObject voiceOf;
	public GameObject driverName;
	public GameObject car;

	[NonSerialized] public Animator animationRoot_Animator;
	[NonSerialized] public TextMeshProUGUI voiceOf_Text;
	[NonSerialized] public TextMeshProUGUI driverName_Text;
	[NonSerialized] public ImageSettings car_ImageSettings;

	[NonSerialized] public int radioTransmitCarIdx = -1;

	public void Awake()
	{
		animationRoot_Animator = animationRoot.GetComponent<Animator>();
		voiceOf_Text = voiceOf.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		car_ImageSettings = car.GetComponent<ImageSettings>();
	}

	public void Start()
	{
		transform.localPosition = new Vector2( Settings.overlay.voiceOfOverlayPosition.x, -Settings.overlay.voiceOfOverlayPosition.y );

		if ( !Settings.overlay.showVoiceOfOverlay )
		{
			gameObject.SetActive( false );
		}
	}

	public void Update()
	{
		if ( radioTransmitCarIdx != IRSDK.normalizedData.radioTransmitCarIdx )
		{
			radioTransmitCarIdx = IRSDK.normalizedData.radioTransmitCarIdx;

			animationRoot_Animator.SetBool( "Show", radioTransmitCarIdx != -1 );

			var normalizedCar = IRSDK.normalizedData.FindNormalizedCarByCarIdx( radioTransmitCarIdx );

			if ( normalizedCar != null )
			{
				voiceOf_Text.text = Settings.overlay.GetTranslation( "Voice of", "VOICE OF" );
				driverName_Text.text = normalizedCar.userName;

				car_ImageSettings.SetCarIdx( radioTransmitCarIdx );
			}
		}
	}
}
