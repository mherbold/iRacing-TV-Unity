
using System;

using UnityEngine;

[Serializable]
public class SettingsImage
{
	public enum ImageType
	{
		None = 0,
		ImageFile = 1,
		SeriesLogo = 2,
		CarNumber = 3,
		Car = 4,
		Helmet = 5,
		Driver = 6,
		MemberClubRegion = 8,
		MemberID_A = 9,
		MemberID_B = 10,
		MemberID_C = 11
	}

	public ImageType imageType = ImageType.None;
	public ImageType fallbackType = ImageType.None;

	public string filePath = string.Empty;

	public Vector2 position = Vector2.zero;
	public Vector2 size = Vector2.zero;
	public Color tintColor = Color.white;
	public Vector4 border = Vector4.zero;
	public Vector2 frameSize = Vector2.zero;
	public int frameCount = 1;
	public float animationSpeed = 10;
	public bool tilingEnabled = false;
	public bool useClassColors = false;
	public float classColorStrength = 0.5f;
}
