
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

	[NonSerialized] public int carIdx = 0;

	[NonSerialized] public SettingsImage.ImageType imageType;
	[NonSerialized] public string imageFilePath;
	[NonSerialized] public Texture2D texture;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		rawImage = GetComponent<RawImage>();
	}

	public void Start()
	{
		SettingsUpdated();
	}

	public void Update()
	{
		Texture2D texture = null;

		switch ( settings.imageType )
		{
			case SettingsImage.ImageType.ImageFile:
				texture = (Texture2D) rawImage.texture;
				break;

			case SettingsImage.ImageType.SeriesLogo:
				texture = StreamingTextures.seriesLogoStreamedTexture.GetTexture();
				break;

			case SettingsImage.ImageType.CarNumber:
				texture = StreamingTextures.carNumberStreamedTexture[ carIdx ].GetTexture();
				break;

			case SettingsImage.ImageType.Car:
				texture = StreamingTextures.carStreamedTexture[ carIdx ].GetTexture();
				break;

			case SettingsImage.ImageType.Helmet:
				texture = StreamingTextures.helmetStreamedTexture[ carIdx ].GetTexture();
				break;
		}

		SetTexture( texture );
	}

	public void SetTexture( Texture2D texture, bool forceUpate = false )
	{
		if ( texture == null )
		{
			if ( rawImage.enabled || ( this.texture != null ) )
			{
				this.texture = null;

				rawImage.enabled = false;
				rawImage.texture = null;
			}
		}
		else if ( ( texture != this.texture ) || forceUpate )
		{
			this.texture = texture;

			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Trilinear;
			texture.anisoLevel = 16;

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

	public void SettingsUpdated()
	{
		if ( id == string.Empty )
		{
			enabled = false;
		}
		else
		{
			settings = Settings.overlay.GetImageSettings( id );

			Texture2D texture;

			if ( imageType != settings.imageType )
			{
				imageType = settings.imageType;

				imageFilePath = string.Empty;
			}

			if ( imageType == SettingsImage.ImageType.ImageFile )
			{
				texture = ( rawImage.texture != null ) ? (Texture2D) rawImage.texture : null;

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
			}
			else
			{
				texture = null;
			}

			SetTexture( texture, true );
		}
	}
}
