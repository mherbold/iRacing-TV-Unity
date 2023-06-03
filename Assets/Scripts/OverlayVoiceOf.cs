
using UnityEngine;

using TMPro;

public class OverlayVoiceOf : MonoBehaviour
{
	public GameObject animationRoot;
	public GameObject voiceOf;
	public GameObject driverName;
	public GameObject car;

	private Animator animationRoot_Animator;
	private TextMeshProUGUI voiceOf_Text;
	private TextMeshProUGUI driverName_Text;
	private ImageSettings car_ImageSettings;

	private int radioTransmitCarIdx = -1;

	public void Awake()
	{
		animationRoot_Animator = animationRoot.GetComponent<Animator>();
		voiceOf_Text = voiceOf.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		car_ImageSettings = car.GetComponent<ImageSettings>();
	}

	public void Start()
	{
		transform.localPosition = new Vector2( Settings.data.voiceOfOverlayPosition.x, -Settings.data.voiceOfOverlayPosition.y );
	}

	public void Update()
	{
		if ( radioTransmitCarIdx != IRSDK.normalizedData.radioTransmitCarIdx )
		{
			radioTransmitCarIdx = IRSDK.normalizedData.radioTransmitCarIdx;

			var normalizedCar = IRSDK.normalizedData.FindNormalizedCarByCarIdx( radioTransmitCarIdx );

			animationRoot_Animator.SetBool( "Show", radioTransmitCarIdx != -1 );

			voiceOf_Text.text = Settings.data.GetTranslation( "Voice of", "VOICE OF" );
			driverName_Text.text = normalizedCar.userName;

			car_ImageSettings.SetCarIdx( radioTransmitCarIdx );
		}
	}
}
