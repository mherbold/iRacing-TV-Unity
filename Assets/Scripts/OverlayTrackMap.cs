
using System;
using System.IO;

using UnityEngine;

public class OverlayTrackMap : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject trackMap;
	public GameObject startFinishLine;
	public GameObject carTemplate;

	[NonSerialized] public RectTransform rectTransform;
	[NonSerialized] public LineRenderer lineRenderer;

	[NonSerialized] public GameObject[] cars;

	[NonSerialized] public OverlayTrackMapCar[] overlayTrackMapCars;

	[NonSerialized] public long indexSettings;

	[NonSerialized] public int trackID = 0;
	[NonSerialized] public string textureFilePath = string.Empty;
	[NonSerialized] public float scale = 1;

	public void Awake()
	{
		carTemplate.SetActive( false );

		rectTransform = GetComponent<RectTransform>();
		lineRenderer = trackMap.GetComponent<LineRenderer>();

		lineRenderer.material = Instantiate( lineRenderer.material );

		cars = new GameObject[ LiveDataTrackMap.MaxNumCars ];

		overlayTrackMapCars = new OverlayTrackMapCar[ LiveDataTrackMap.MaxNumCars ];

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

			transform.localPosition = new Vector2( Settings.overlay.trackMapPosition.x, -Settings.overlay.trackMapPosition.y );

			if ( ( LiveData.Instance.liveDataTrackMap.width > 0 ) && ( LiveData.Instance.liveDataTrackMap.height > 0 ) )
			{
				var widthRatio = Settings.overlay.trackMapSize.x / LiveData.Instance.liveDataTrackMap.width;
				var heightRatio = Settings.overlay.trackMapSize.y / LiveData.Instance.liveDataTrackMap.height;

				var width = LiveData.Instance.liveDataTrackMap.width * widthRatio;
				var height = LiveData.Instance.liveDataTrackMap.height * widthRatio;

				scale = width;

				if ( ( Settings.overlay.trackMapSize.x == 0 ) || ( height > Settings.overlay.trackMapSize.y ) )
				{
					width = LiveData.Instance.liveDataTrackMap.width * heightRatio;
					height = LiveData.Instance.liveDataTrackMap.height * heightRatio;

					scale = height;
				}

				var offset = new Vector3( ( Settings.overlay.trackMapSize.x - width ) / 2, ( Settings.overlay.trackMapSize.y - height ) / -2, 0 );

				rectTransform.localPosition = new Vector3( Settings.overlay.trackMapPosition.x, -Settings.overlay.trackMapPosition.y, rectTransform.localPosition.z ) + offset;
				rectTransform.localScale = new Vector2( scale, scale );
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
					if ( File.Exists( Settings.overlay.trackMapTextureFilePath ) )
					{
						var bytes = File.ReadAllBytes( Settings.overlay.trackMapTextureFilePath );

						newTexture = new Texture2D( 1, 1 );

						newTexture.LoadImage( bytes );

						newTexture.filterMode = FilterMode.Trilinear;
						newTexture.anisoLevel = 16;
					}
				}

				lineRenderer.material.SetTexture( "_MainTex", newTexture );
			}
		}

		for ( var carIndex = 0; carIndex < LiveDataTrackMap.MaxNumCars; carIndex++ )
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

				overlayTrackMapCar.transform.localPosition = liveDataTrackMapCar.offset;
				overlayTrackMapCar.transform.localScale = new Vector2( 1.0f / scale, 1.0f / scale );

				overlayTrackMapCar.highlight.SetActive( liveDataTrackMapCar.showHighlight );
			}
		}

		startFinishLine.transform.localPosition = LiveData.Instance.liveDataTrackMap.startFinishLine;
		startFinishLine.transform.localScale = new Vector2( 1.0f / scale, 1.0f / scale );
	}
}
