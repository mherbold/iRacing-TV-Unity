
using System;
using System.IO;

using UnityEngine;

public class ImageSettings : MonoBehaviour
{
	public string id;

	[NonSerialized] public RectTransform rectTransform;
	[NonSerialized] public SpriteRenderer spriteRenderer;

	[NonSerialized] public SettingsImage settings;

	[NonSerialized] public int carIdx = 0;

	[NonSerialized] public string imageFilePath;
	[NonSerialized] public Texture2D texture;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void OnEnable()
	{
		SettingsUpdated();
	}

	public void Update()
	{
		if ( settings.imageType > SettingsImage.ImageType.ImageFile )
		{
			SetStreamedTexture();
		}
	}

	public void SetPosition( Vector2 position )
	{
		position += settings.position;

		rectTransform.localPosition = new Vector3( position.x, -position.y, rectTransform.localPosition.z );
	}

	public void SetSize( Vector2 size )
	{
		size += settings.size;

		spriteRenderer.size = new Vector2( size.x, -size.y );
	}

	public void SetTexture( Texture2D newTexture, bool forceUpate = false )
	{
		if ( newTexture == null )
		{
			if ( spriteRenderer.enabled || ( texture != null ) )
			{
				texture = null;

				spriteRenderer.enabled = false;
				spriteRenderer.sprite = null;
			}
		}
		else if ( ( texture != newTexture ) || forceUpate )
		{
			if ( texture != newTexture )
			{
				texture = newTexture;

				var sprite = Sprite.Create( texture, new Rect( 0.0f, 0.0f, texture.width, texture.height ), Vector2.zero, 1, 0, SpriteMeshType.FullRect, settings.border, false );

				spriteRenderer.enabled = true;
				spriteRenderer.sprite = sprite;

				texture.wrapMode = TextureWrapMode.Clamp;
				texture.filterMode = FilterMode.Trilinear;
				texture.anisoLevel = 16;
			}

			spriteRenderer.color = settings.tintColor;

			if ( settings.size == Vector2.zero )
			{
				rectTransform.localPosition = new Vector3( settings.position.x, -settings.position.y, rectTransform.localPosition.z );

				spriteRenderer.size = new Vector2( texture.width, -texture.height );
			}
			else
			{
				if ( settings.border == Vector4.zero )
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

					var offset = new Vector3( ( settings.size.x - width ) / 2, ( settings.size.y - height ) / -2, 0 );

					rectTransform.localPosition = new Vector3( settings.position.x, -settings.position.y, rectTransform.localPosition.z ) + offset;

					spriteRenderer.size = new Vector2( width, -height );
				}
				else
				{
					rectTransform.localPosition = new Vector3( settings.position.x, -settings.position.y, rectTransform.localPosition.z );

					spriteRenderer.size = new Vector2( settings.size.x, -settings.size.y );
				}
			}
		}
	}

	public void SetStreamedTexture( bool forceUpdate = false )
	{
		Texture2D newTexture = null;

		switch ( settings.imageType )
		{
			case SettingsImage.ImageType.SeriesLogo:
				newTexture = StreamingTextures.seriesLogoStreamedTexture.GetTexture();
				break;

			case SettingsImage.ImageType.CarNumber:
				newTexture = StreamingTextures.carNumberStreamedTexture[ carIdx ].GetTexture();
				break;

			case SettingsImage.ImageType.Car:
				newTexture = StreamingTextures.carStreamedTexture[ carIdx ].GetTexture();
				break;

			case SettingsImage.ImageType.Helmet:
				newTexture = StreamingTextures.helmetStreamedTexture[ carIdx ].GetTexture();
				break;
		}

		SetTexture( newTexture, forceUpdate );
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

			if ( settings.imageType == SettingsImage.ImageType.None )
			{
				imageFilePath = string.Empty;

				SetTexture( null );
			}
			else if ( settings.imageType == SettingsImage.ImageType.ImageFile )
			{
				var newTexture = texture;

				if ( imageFilePath != settings.filePath )
				{
					imageFilePath = settings.filePath;

					newTexture = null;

					if ( settings.filePath != string.Empty )
					{
						if ( File.Exists( settings.filePath ) )
						{
							var bytes = File.ReadAllBytes( settings.filePath );

							newTexture = new Texture2D( 1, 1 );

							newTexture.LoadImage( bytes );
						}
					}
				}

				SetTexture( newTexture, true );
			}
			else
			{
				imageFilePath = string.Empty;

				SetStreamedTexture( true );
			}
		}
	}
}
