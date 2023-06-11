
using System;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class ImageSettings : MonoBehaviour
{
	public string id;

	[NonSerialized] public RectTransform rectTransform;
	[NonSerialized] public RawImage rawImage;

	[NonSerialized] public SettingsImage settings;

	[NonSerialized] public bool carIdxIsValid = false;
	[NonSerialized] public int carIdx;

	[NonSerialized] public bool waitingForStreamedTexture;
	[NonSerialized] public SettingsImage.ImageType imageType;
	[NonSerialized] public string imageFilePath;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		rawImage = GetComponent<RawImage>();
	}

	public void Start()
	{
		OverlayUpdated();
	}

	public void Update()
	{
		if ( waitingForStreamedTexture )
		{
			Texture2D texture = null;

			switch ( settings.imageType )
			{
				case SettingsImage.ImageType.SeriesLogo:
					texture = IRSDK.normalizedSession.seriesTexture;
					break;

				case SettingsImage.ImageType.CarNumber:

					if ( carIdxIsValid )
					{
						texture = IRSDK.normalizedData.normalizedCars[ carIdx ].carNumberTexture;
					}

					break;

				case SettingsImage.ImageType.Car:

					if ( carIdxIsValid )
					{
						texture = IRSDK.normalizedData.normalizedCars[ carIdx ].carTexture;
					}

					break;
			}

			if ( texture != null )
			{
				waitingForStreamedTexture = false;

				SetTexture( texture );
			}
		}
	}

	public void SetCarIdx( int carIdx )
	{
		if ( this.carIdx != carIdx )
		{
			this.carIdx = carIdx;

			carIdxIsValid = true;

			if ( ( settings.imageType == SettingsImage.ImageType.CarNumber ) || ( settings.imageType == SettingsImage.ImageType.Car ) )
			{
				waitingForStreamedTexture = true;
			}
		}
	}

	public void SetTexture( Texture2D texture )
	{
		if ( texture == null )
		{
			rawImage.enabled = false;
			rawImage.texture = null;
		}
		else
		{
			rawImage.enabled = true;
			rawImage.texture = texture;
			rawImage.color = settings.tintColor;

			if ( settings.size == Vector2.zero )
			{
				rectTransform.localPosition = new Vector2( settings.position.x, -settings.position.y );
				rectTransform.sizeDelta = new Vector2( texture.width, texture.height );
			}
			else
			{
				var widthRatio = settings.size.x / texture.width;
				var heightRatio = settings.size.y / texture.height;

				var width = texture.width * widthRatio;
				var height = texture.height * widthRatio;

				if ( height > settings.size.y )
				{
					width = texture.width * heightRatio;
					height = texture.height * heightRatio;
				}

				var offset = new Vector2( ( settings.size.x - width ) / 2, ( settings.size.y - height ) / -2 );

				rectTransform.localPosition = new Vector2( settings.position.x, -settings.position.y ) + offset;
				rectTransform.sizeDelta = new Vector2( width, height );
			}
		}
	}

	public void OverlayUpdated()
	{
		if ( id == string.Empty )
		{
			enabled = false;
		}
		else
		{
			settings = Settings.overlay.GetImageSettings( id );

			if ( imageType != settings.imageType )
			{
				imageType = settings.imageType;

				imageFilePath = string.Empty;

				SetTexture( null );
			}

			switch ( settings.imageType )
			{
				case SettingsImage.ImageType.None:

					SetTexture( null );

					break;

				case SettingsImage.ImageType.ImageFile:

					var texture = ( rawImage.texture != null ) ? (Texture2D) rawImage.texture : null;

					if ( imageFilePath != settings.filePath )
					{
						imageFilePath = settings.filePath;

						texture = null;

						if ( settings.filePath != string.Empty )
						{
							if ( File.Exists( settings.filePath ) )
							{
								var bytes = File.ReadAllBytes( settings.filePath );

								texture = new Texture2D( 1, 1 );

								texture.LoadImage( bytes );
							}
						}
					}

					SetTexture( texture );

					break;

				case SettingsImage.ImageType.SeriesLogo:
				case SettingsImage.ImageType.CarNumber:
				case SettingsImage.ImageType.Car:

					waitingForStreamedTexture = true;

					break;
			}
		}
	}
}
