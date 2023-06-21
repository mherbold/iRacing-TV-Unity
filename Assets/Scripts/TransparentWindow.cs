
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

		SettingsUpdated();
	}

	public void SettingsUpdated()
	{
		WinApi.SetWindowPos( hWnd, WinApi.HWND_TOPMOST, Settings.overlay.overlayPosition.x, Settings.overlay.overlayPosition.y, Settings.overlay.overlaySize.x, Settings.overlay.overlaySize.y, 0 );
	}

#endif
}
