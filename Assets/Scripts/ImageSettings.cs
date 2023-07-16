
using System;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class ImageSettings : MonoBehaviour
{
	public string id;

	[NonSerialized] public RectTransform rectTransform;
	[NonSerialized] public Image image;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public SettingsImage settings;

	[NonSerialized] public int carIdx = 0;

	[NonSerialized] public string imageFilePath;
	[NonSerialized] public Texture2D texture;

	[NonSerialized] public Vector2 previousSize = Vector2.zero;
	[NonSerialized] public Vector2 previousPosition = Vector2.zero;
	[NonSerialized] public float showBorderTimer = 0;
	[NonSerialized] public GameObject border = null;
	[NonSerialized] public Image border_Image = null;
	[NonSerialized] public RectTransform border_RectTransform = null;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		image = GetComponent<Image>();
	}

	public void Update()
	{
		if ( border == null )
		{
			border = Instantiate( Border.border_GameObject );

			border.name = $"{transform.name} {Border.border_GameObject.name}";

			border.transform.SetParent( transform.parent );

			border.SetActive( true );

			border_Image = border.GetComponent<Image>();
			border_RectTransform = border.GetComponent<RectTransform>();
		}

        if ( indexSettings != IPC.indexSettings )
        {
			indexSettings = IPC.indexSettings;

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

        if ( settings != null )
		{
			if ( settings.imageType > SettingsImage.ImageType.ImageFile )
			{
				SetStreamedTexture();
			}

			if ( showBorderTimer > 0 )
			{
				showBorderTimer = Math.Max( 0, showBorderTimer - Time.deltaTime );

				border_Image.enabled = ( showBorderTimer > 0 );
			}
		}
	}

	public void SetPosition( Vector2 position )
	{
		if ( settings != null )
		{
			position += settings.position;

			rectTransform.localPosition = new Vector3( position.x, -position.y, rectTransform.localPosition.z );
		}
	}

	public void SetSize( Vector2 size )
	{
		if ( settings != null )
		{
			size += settings.size;

			rectTransform.sizeDelta = size;
		}
	}

	public void SetTexture( Texture2D newTexture, bool forceUpate = false )
	{
		if ( newTexture == null )
		{
			if ( image.enabled || ( texture != null ) )
			{
				texture = null;

				image.enabled = false;
				image.sprite = null;
			}
		}
		else if ( ( texture != newTexture ) || forceUpate )
		{
			if ( texture != newTexture )
			{
				texture = newTexture;

				var sprite = Sprite.Create( texture, new Rect( 0.0f, 0.0f, texture.width, texture.height ), Vector2.zero, 1, 0, SpriteMeshType.FullRect, settings.border, false );

				image.enabled = true;
				image.sprite = sprite;

				texture.wrapMode = TextureWrapMode.Clamp;
				texture.filterMode = FilterMode.Trilinear;
				texture.anisoLevel = 16;
			}
			else
			{
				if ( ( previousSize != settings.size ) || ( previousPosition != settings.position ) )
				{
					showBorderTimer = 3.0f;
				}
			}

			image.color = settings.tintColor;

			if ( settings.size == Vector2.zero )
			{
				rectTransform.localPosition = new Vector3( settings.position.x, -settings.position.y, rectTransform.localPosition.z );
				rectTransform.sizeDelta = new Vector2( texture.width, texture.height );

				border_RectTransform.localPosition = rectTransform.localPosition;
				border_RectTransform.sizeDelta = rectTransform.sizeDelta;
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
					rectTransform.sizeDelta = new Vector2( width, height );
				}
				else
				{
					rectTransform.localPosition = new Vector3( settings.position.x, -settings.position.y, rectTransform.localPosition.z );
					rectTransform.sizeDelta = settings.size;
				}

				border_RectTransform.localPosition = new Vector3( settings.position.x, -settings.position.y, rectTransform.localPosition.z );
				border_RectTransform.sizeDelta = settings.size;
			}

			previousSize = settings.size;
			previousPosition = settings.position;
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

			case SettingsImage.ImageType.Driver:
				newTexture = StreamingTextures.driverStreamedTexture[ carIdx ].GetTexture();
				break;
		}

		SetTexture( newTexture, forceUpdate );
	}
}
