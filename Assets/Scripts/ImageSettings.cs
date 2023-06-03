
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class ImageSettings : MonoBehaviour
{
	public string id;

	private RectTransform rectTransform;
	private RawImage rawImage;

	public SettingsData.ImageSettingsData imageSettingsData;

	public bool carIdxIsValid = false;
	public int carIdx;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		rawImage = GetComponent<RawImage>();
	}

	public void Start()
	{
		rawImage.enabled = false;

		if ( id != string.Empty )
		{
			imageSettingsData = Settings.data.GetImageSettingsData( id );

			if ( imageSettingsData.imageType == SettingsData.ImageType.Custom )
			{
				var imageFileName = imageSettingsData.fileName;

				if ( imageFileName != string.Empty )
				{
					if ( !File.Exists( imageFileName ) )
					{
						imageFileName = Program.documentsFolder + imageFileName;
					}

					if ( File.Exists( imageFileName ) )
					{
						var bytes = File.ReadAllBytes( imageFileName );

						var texture = new Texture2D( 1, 1 );

						texture.LoadImage( bytes );

						SetTexture( texture );
					}
					else
					{
						enabled = false;
					}
				}
				else
				{
					enabled = false;
				}
			}
		}
		else
		{
			enabled = false;
		}
	}

	public void Update()
	{
		if ( !rawImage.enabled )
		{
			Texture2D texture = null;

			switch ( imageSettingsData.imageType )
			{
				case SettingsData.ImageType.SeriesLogo:
					texture = IRSDK.normalizedSession.seriesTexture;
					break;

				case SettingsData.ImageType.CarNumber:

					if ( carIdxIsValid )
					{
						texture = IRSDK.normalizedData.normalizedCars[ carIdx ].carNumberTexture;
					}

					break;

				case SettingsData.ImageType.Car:

					if ( carIdxIsValid )
					{
						texture = IRSDK.normalizedData.normalizedCars[ carIdx ].carTexture;
					}

					break;
			}

			SetTexture( texture );
		}
	}

	public void SetCarIdx( int carIdx )
	{
		this.carIdx = carIdx;

		carIdxIsValid = true;

		Texture2D texture = null;

		switch ( imageSettingsData.imageType )
		{
			case SettingsData.ImageType.CarNumber:
				texture = IRSDK.normalizedData.normalizedCars[ carIdx ].carNumberTexture;
				break;

			case SettingsData.ImageType.Car:
				texture = IRSDK.normalizedData.normalizedCars[ carIdx ].carTexture;
				break;
		}

		SetTexture( texture );
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
			rawImage.color = imageSettingsData.tintColor;

			if ( imageSettingsData.size == Vector2.zero )
			{
				rectTransform.localPosition = new Vector2( imageSettingsData.position.x, -imageSettingsData.position.y );
				rectTransform.sizeDelta = new Vector2( texture.width, texture.height );
			}
			else
			{
				var widthRatio = imageSettingsData.size.x / texture.width;
				var heightRatio = imageSettingsData.size.y / texture.height;

				var width = texture.width * widthRatio;
				var height = texture.height * widthRatio;

				if ( height > imageSettingsData.size.y )
				{
					width = texture.width * heightRatio;
					height = texture.height * heightRatio;
				}

				var offset = new Vector2( ( imageSettingsData.size.x - width ) / 2, ( imageSettingsData.size.y - height ) / 2 );

				rectTransform.localPosition = new Vector2( imageSettingsData.position.x, -imageSettingsData.position.y ) + offset;
				rectTransform.sizeDelta = new Vector2( width, height );
			}
		}
	}
}
