
using UnityEngine;

public class CanvasOffset : MonoBehaviour
{
	public Vector2 originalLocalPosition;

	public void Awake()
	{
		var rectTransform = GetComponent<RectTransform>();

		originalLocalPosition = rectTransform.localPosition;
	}

	public void Start()
	{
		var rectTransform = GetComponent<RectTransform>();

		rectTransform.localPosition = Settings.data.overlayPosition + originalLocalPosition;
	}
}
