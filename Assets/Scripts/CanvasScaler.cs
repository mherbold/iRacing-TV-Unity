
using UnityEngine;

public class CanvasScaler : MonoBehaviour
{
	public void Start()
	{
		var rectTransform = GetComponent<RectTransform>();

		var scaleFactor = Settings.data.overlaySize.y / 1080;

		rectTransform.localScale = Vector3.one * scaleFactor;
	}
}
