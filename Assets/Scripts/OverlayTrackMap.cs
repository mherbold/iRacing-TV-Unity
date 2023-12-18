
using System;
using System.IO;

using UnityEngine;

public class OverlayTrackMap : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject trackMap;
	public GameObject startFinishLine;
	public GameObject paceCar;
	public GameObject carTemplate;

	[NonSerialized] public RectTransform rectTransform;
	[NonSerialized] public LineRenderer lineRenderer;

	[NonSerialized] public GameObject[] cars;

	[NonSerialized] public OverlayTrackMapCar[] overlayTrackMapCars;

	[NonSerialized] public long indexSettings;

	[NonSerialized] public int trackID = 0;
	[NonSerialized] public string textureFilePath = string.Empty;
	[NonSerialized] public float scale = 1;

	[NonSerialized] public Vector3 positionOffset = Vector3.zero;

	[NonSerialized] public GameObject border = null;
	[NonSerialized] public RectTransform border_RectTransform = null;

	public void Awake()
	{
		carTemplate.SetActive( false );

		rectTransform = GetComponent<RectTransform>();
		lineRenderer = trackMap.GetComponent<LineRenderer>();

		lineRenderer.material = Instantiate( lineRenderer.material );

		cars = new GameObject[ LiveData.MaxNumDrivers ];

		overlayTrackMapCars = new OverlayTrackMapCar[ LiveData.MaxNumDrivers ];

		for ( var carIndex = 0; carIndex < cars.Length; carIndex++ )
		{
			cars[ carIndex ] = Instantiate( carTemplate );

			cars[ carIndex ].transform.SetParent( carTemplate.transform.parent, false );

			cars[ carIndex ].SetActive( true );

			overlayTrackMapCars[ carIndex ] = cars[ carIndex ].GetComponent<OverlayTrackMapCar>();

			overlayTrackMapCars[ carIndex ].layer1_ImageSettings.carIdx = carIndex;
			overlayTrackMapCars[ carIndex ].layer2_ImageSettings.carIdx = carIndex;
		}
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.trackMapOn && LiveData.Instance.liveDataTrackMap.show && !LiveData.Instance.liveDataIntro.show && ipc.isConnected && LiveData.Instance.isConnected );

		if ( border == null )
		{
			border = Instantiate( Border.border_GameObject );

			border.name = $"{transform.name} {Border.border_GameObject.name}";

			border.transform.SetParent( trackMap.transform.parent );

			border.SetActive( true );

			border_RectTransform = border.GetComponent<RectTransform>();
			border_RectTransform.pivot = rectTransform.pivot;
		}

		var forceUpdate = false;

		if ( trackID != LiveData.Instance.liveDataTrackMap.trackID )
		{
			trackID = LiveData.Instance.liveDataTrackMap.trackID;

			lineRenderer.positionCount = LiveData.Instance.liveDataTrackMap.drawVectorList.Count;

			lineRenderer.SetPositions( LiveData.Instance.liveDataTrackMap.drawVectorList.ToArray() );

			forceUpdate = true;
		}

		if ( ( indexSettings != IPC.indexSettings ) || forceUpdate )
		{
			indexSettings = IPC.indexSettings;

			rectTransform.localPosition = new Vector2( Settings.overlay.trackMapPosition.x, -Settings.overlay.trackMapPosition.y );

			if ( ( LiveData.Instance.liveDataTrackMap.width > 0 ) && ( LiveData.Instance.liveDataTrackMap.height > 0 ) )
			{
				var liveDataTrackMapWidthHeightRatio = LiveData.Instance.liveDataTrackMap.height / LiveData.Instance.liveDataTrackMap.width;

				if ( Settings.overlay.trackMapSize.x * liveDataTrackMapWidthHeightRatio <= Settings.overlay.trackMapSize.y )
				{
					scale = Settings.overlay.trackMapSize.x;
				}
				else
				{
					scale = Settings.overlay.trackMapSize.y / liveDataTrackMapWidthHeightRatio;
				}

				var width = scale;
				var height = scale * liveDataTrackMapWidthHeightRatio;

				var offsetX = ( Settings.overlay.trackMapSize.x - width ) / 2;
				var offsetY = ( Settings.overlay.trackMapSize.y - height ) / -2;

				positionOffset = new Vector3( offsetX, offsetY, 0 );

				trackMap.transform.localPosition = positionOffset;
				trackMap.transform.localScale = new Vector2( scale, scale );

				border_RectTransform.localPosition = Vector3.zero;
				border_RectTransform.sizeDelta = Settings.overlay.trackMapSize;
			}

			lineRenderer.startWidth = Settings.overlay.trackMapLineThickness;
			lineRenderer.endWidth = Settings.overlay.trackMapLineThickness;

			lineRenderer.startColor = Settings.overlay.trackMapLineColor;
			lineRenderer.endColor = Settings.overlay.trackMapLineColor;

			if ( textureFilePath != Settings.overlay.trackMapTextureFilePath )
			{
				textureFilePath = Settings.overlay.trackMapTextureFilePath;

				Texture2D newTexture = null;

				if ( Settings.overlay.trackMapTextureFilePath != string.Empty )
				{
					var fullFilePath = Program.GetFullPath( Settings.overlay.trackMapTextureFilePath );

					if ( File.Exists( fullFilePath ) )
					{
						var bytes = File.ReadAllBytes( fullFilePath );

						newTexture = new Texture2D( 1, 1 );

						newTexture.LoadImage( bytes );

						newTexture.filterMode = FilterMode.Trilinear;
						newTexture.anisoLevel = 16;
					}
				}

				lineRenderer.material.SetTexture( "_MainTex", newTexture );
			}
		}

		for ( var carIndex = 0; carIndex < LiveData.MaxNumDrivers; carIndex++ )
		{
			var liveDataTrackMapCar = LiveData.Instance.liveDataTrackMap.liveDataTrackMapCars[ carIndex ];

			if ( liveDataTrackMapCar != null )
			{
				var overlayTrackMapCar = overlayTrackMapCars[ carIndex ];

				if ( !liveDataTrackMapCar.show )
				{
					overlayTrackMapCar.gameObject.SetActive( false );
				}
				else
				{
					overlayTrackMapCar.gameObject.SetActive( true );
				}

				overlayTrackMapCar.transform.localPosition = liveDataTrackMapCar.offset * scale + positionOffset;

				overlayTrackMapCar.currentTarget.SetActive( liveDataTrackMapCar.showHighlight );

				overlayTrackMapCar.textLayer1_Text.text = liveDataTrackMapCar.textLayer1;
			}
		}

		startFinishLine.transform.localPosition = LiveData.Instance.liveDataTrackMap.startFinishLine * scale + positionOffset;

		if ( LiveData.Instance.liveDataTrackMap.showPaceCar )
		{
			paceCar.gameObject.SetActive( true );

			paceCar.transform.localPosition = LiveData.Instance.liveDataTrackMap.paceCarOffset * scale + positionOffset;
		}
		else
		{
			paceCar.gameObject.SetActive( false );
		}
	}
}
