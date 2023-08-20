
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
	[NonSerialized] public Color classColor = Color.white;

	[NonSerialized] public string imageFilePath;
	[NonSerialized] public Texture2D texture;

	[NonSerialized] public Vector2 positionOffset = Vector2.zero;
	[NonSerialized] public Vector2? previousSize = null;
	[NonSerialized] public Vector2? previousPosition = null;
	[NonSerialized] public Vector4? previousBorder = null;
	[NonSerialized] public float showBorderTimer = 0;
	[NonSerialized] public GameObject border = null;
	[NonSerialized] public Image border_Image = null;
	[NonSerialized] public RectTransform border_RectTransform = null;

	[NonSerialized] public float currentFrame = 0;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		image = GetComponent<Image>();

		image.material = Instantiate( image.material );
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
			border_RectTransform.pivot = rectTransform.pivot;
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
							var fullFilePath = Program.GetFullPath( imageFilePath );

							if ( File.Exists( fullFilePath ) )
							{
								var bytes = File.ReadAllBytes( fullFilePath );

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

				image.type = ( settings.border == Vector4.zero ) ? Image.Type.Simple : Image.Type.Sliced;

				if ( ( previousSize == null ) || ( previousPosition == null ) )
				{
					previousSize = settings.size;
					previousPosition = settings.position;
				}
				else if ( ( previousSize != settings.size ) || ( previousPosition != settings.position ) )
				{
					previousSize = settings.size;
					previousPosition = settings.position;

					showBorderTimer = 3.0f;
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

			if ( ( settings.frameCount > 1 ) && ( settings.frameSize.x > 0 ) && ( settings.frameSize.y > 0 ) && ( texture != null ) )
			{
				currentFrame += Time.deltaTime * settings.animationSpeed;

				if ( currentFrame > settings.frameCount )
				{
					currentFrame %= settings.frameCount;
				}

				var framesPerRow = (float) Math.Floor( texture.width / settings.frameSize.x );
				var framesPerColumn = (float) Math.Floor( texture.height / settings.frameSize.y );

				var currentRow = (float) Math.Floor( currentFrame / framesPerRow );
				var currentColumn = (float) Math.Floor( currentFrame - ( currentRow * framesPerRow ) );

				currentRow = framesPerColumn - currentRow - 1;

				var textureScale = new Vector2( settings.frameSize.x / texture.width, settings.frameSize.y / texture.height );
				var textureOffset = new Vector2( currentColumn * settings.frameSize.x / texture.width, currentRow * settings.frameSize.y / texture.height );

				image.material.SetTextureScale( "_MainTex", textureScale );
				image.material.SetTextureOffset( "_MainTex", textureOffset );
			}

			if ( settings.useClassColors )
			{
				image.color = Color.Lerp( settings.tintColor, classColor, settings.classColorStrength );
			}
			else
			{
				image.color = settings.tintColor;
			}
		}
	}

	public void SetPosition( Vector2 position )
	{
		if ( settings != null )
		{
			position += settings.position + positionOffset;

			rectTransform.localPosition = new Vector3( position.x, position.y, rectTransform.localPosition.z );
		}
	}

	public void SetSize( Vector2 size )
	{
		if ( settings != null )
		{
			size += settings.size;

			rectTransform.sizeDelta = size;

			if ( ( settings.tilingEnabled ) && ( texture != null ) )
			{
				var textureScale = new Vector2( size.x / texture.width, size.y / texture.height );

				image.material.SetTextureScale( "_MainTex", textureScale );

				image.mainTexture.wrapMode = TextureWrapMode.Repeat;
			}
			else
			{
				image.material.SetTextureScale( "_MainTex", Vector2.one );

				image.mainTexture.wrapMode = TextureWrapMode.Clamp;
			}
		}
	}

	public void SetClassColor( Color classColor )
	{
		this.classColor = classColor;
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
			if ( ( texture != newTexture ) || ( previousBorder == null ) || ( previousBorder != settings.border ) )
			{
				texture = newTexture;
				previousBorder = settings.border;

				var sprite = Sprite.Create( texture, new Rect( 0.0f, 0.0f, texture.width, texture.height ), Vector2.zero, 1, 0, SpriteMeshType.FullRect, settings.border, false );

				image.enabled = true;
				image.sprite = sprite;

				texture.wrapMode = TextureWrapMode.Clamp;
				texture.filterMode = FilterMode.Trilinear;
				texture.anisoLevel = 16;
			}

			positionOffset = Vector3.zero;

			if ( settings.size == Vector2.zero )
			{
				var sourceWidth = ( settings.frameCount > 1 ) ? settings.frameSize.x : texture.width;
				var sourceHeight = ( settings.frameCount > 1 ) ? settings.frameSize.y : texture.height;

				rectTransform.localPosition = new Vector3( settings.position.x, -settings.position.y, rectTransform.localPosition.z );
				rectTransform.sizeDelta = new Vector2( sourceWidth, sourceHeight );

				border_RectTransform.localPosition = rectTransform.localPosition;
				border_RectTransform.sizeDelta = rectTransform.sizeDelta;
			}
			else
			{
				if ( ( settings.border == Vector4.zero ) && !settings.tilingEnabled )
				{
					var sourceWidth = ( settings.frameCount > 1 ) ? settings.frameSize.x : texture.width;
					var sourceHeight = ( settings.frameCount > 1 ) ? settings.frameSize.y : texture.height;

					var widthHeightRatio = sourceHeight / sourceWidth;

					float scale;

					if ( settings.size.x * widthHeightRatio <= settings.size.y )
					{
						scale = settings.size.x;
					}
					else
					{
						scale = settings.size.y / widthHeightRatio;
					}

					var width = scale;
					var height = scale * widthHeightRatio;

					if ( ( rectTransform.pivot.x == 0 ) && ( rectTransform.pivot.y == 1 ) )
					{
						var offsetX = ( settings.size.x - width ) / 2;
						var offsetY = ( settings.size.y - height ) / -2;

						positionOffset = new Vector3( offsetX, offsetY, 0 );
					}

					rectTransform.localPosition = new Vector3( settings.position.x + positionOffset.x, -settings.position.y + positionOffset.y, rectTransform.localPosition.z );
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
