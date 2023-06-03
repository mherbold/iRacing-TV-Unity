
using System;
using System.Runtime.InteropServices;

using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
#if !UNITY_EDITOR

	public struct MARGINS
	{
		public int Left;
		public int Right;
		public int Top;
		public int Bottom;
	}

	[DllImport( "user32.dll" )]
	public static extern IntPtr GetActiveWindow();

	[DllImport( "user32.dll" )]
	public static extern int SetWindowLong( IntPtr hWnd, int nIndex, uint dwNewLong );

	[DllImport( "user32.dll" )]
	public static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );

	[DllImport( "Dwmapi.dll" )]
	public static extern uint DwmExtendFrameIntoClientArea( IntPtr hWnd, ref MARGINS margins );

	public const int GWL_EXSTYLE = -20;

	public const uint WS_EX_TOPMOST = 0x00000008;
	public const uint WS_EX_TRANSPARENT = 0x00000020;
	public const uint WS_EX_LAYERED = 0x00080000;

	public static readonly IntPtr HWND_TOPMOST = new IntPtr( -1 );

	public const uint SWP_NOSIZE = 0x0001;
	public const uint SWP_NOMOVE = 0x0002;

	public IntPtr hWnd;

	public void Start()
	{
		hWnd = GetActiveWindow();

		MARGINS margins = new() { Left = -1, Right = -1, Top = -1, Bottom = -1 };

		DwmExtendFrameIntoClientArea( hWnd, ref margins );

		SetWindowLong( hWnd, GWL_EXSTYLE, WS_EX_TOPMOST | WS_EX_TRANSPARENT | WS_EX_LAYERED );

		SetWindowPos( hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE );
	}

#endif
}
