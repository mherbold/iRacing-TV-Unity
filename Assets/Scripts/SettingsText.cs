
using System;

using UnityEngine;

using TMPro;

[Serializable]
public class SettingsText
{
	public enum FontIndex
	{
		None = -1,
		FontA,
		FontB,
		FontC,
		FontD
	};

	public FontIndex fontIndex = FontIndex.FontA;

	public int fontSize = 0;

	public TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft;

	public Vector2 position = Vector2.zero;
	public Vector2 size = Vector2.zero;
	public Color tintColor = Color.white;
	public bool allowOverflow = true;
}
