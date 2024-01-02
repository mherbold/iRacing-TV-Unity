
using System;

using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
	[NonSerialized] public long indexSettings;

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
	}

	public void Update()
	{
		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			WinApi.SetWindowLong( hWnd, WinApi.GWL_STYLE, WinApi.WS_POPUP | WinApi.WS_VISIBLE );
			WinApi.SetWindowLong( hWnd, WinApi.GWL_EXSTYLE, WinApi.WS_EX_LAYERED | WinApi.WS_EX_TRANSPARENT | WinApi.WS_EX_TOPMOST );
			WinApi.SetWindowPos( hWnd, WinApi.HWND_TOPMOST, Settings.overlay.position.x, Settings.overlay.position.y, Settings.overlay.size.x, Settings.overlay.size.y, 0 );
		}
	}

	#endif
}
