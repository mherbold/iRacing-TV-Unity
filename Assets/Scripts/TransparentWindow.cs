
using System;

using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
	public void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}

#if !UNITY_EDITOR

	IntPtr hWnd;

	public void Start()
	{
		hWnd = WinApi.GetActiveWindow();

		WinApi.MARGINS margins = new() { Left = -1, Right = -1, Top = -1, Bottom = -1 };

		WinApi.DwmExtendFrameIntoClientArea( hWnd, ref margins );

		WinApi.SetWindowLong( hWnd, WinApi.GWL_STYLE, WinApi.WS_POPUP | WinApi.WS_VISIBLE );

		WinApi.SetWindowLong( hWnd, WinApi.GWL_EXSTYLE, WinApi.WS_EX_LAYERED | WinApi.WS_EX_TRANSPARENT | WinApi.WS_EX_TOPMOST );

		OverlayUpdated();
	}

	public void OverlayUpdated()
	{
		WinApi.SetWindowPos( hWnd, WinApi.HWND_TOPMOST, Settings.data.overlayPosition.x, Settings.data.overlayPosition.y, Settings.data.overlaySize.x, Settings.data.overlaySize.y, 0 );
	}

#endif
}
