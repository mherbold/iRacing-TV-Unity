
using System;
using System.Runtime.InteropServices;

public static class WinApi
{
	public const int GWL_WNDPROC = -4;
	public const int GWL_HINSTANCE = -6;
	public const int GWL_HWNDPARENT = -8;
	public const int GWL_ID = -12;
	public const int GWL_STYLE = -16;
	public const int GWL_EXSTYLE = -20;
	public const int GWL_USERDATA = -21;

	public const uint WS_BORDER = 0x800000;
	public const uint WS_CAPTION = 0xc00000;
	public const uint WS_CHILD = 0x40000000;
	public const uint WS_CLIPCHILDREN = 0x2000000;
	public const uint WS_CLIPSIBLINGS = 0x4000000;
	public const uint WS_DISABLED = 0x8000000;
	public const uint WS_DLGFRAME = 0x400000;
	public const uint WS_GROUP = 0x20000;
	public const uint WS_HSCROLL = 0x100000;
	public const uint WS_MAXIMIZE = 0x1000000;
	public const uint WS_MAXIMIZEBOX = 0x10000;
	public const uint WS_MINIMIZE = 0x20000000;
	public const uint WS_MINIMIZEBOX = 0x20000;
	public const uint WS_OVERLAPPED = 0x0;
	public const uint WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
	public const uint WS_POPUP = 0x80000000;
	public const uint WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU;
	public const uint WS_SIZEFRAME = 0x40000;
	public const uint WS_SYSMENU = 0x80000;
	public const uint WS_TABSTOP = 0x10000;
	public const uint WS_VISIBLE = 0x10000000;
	public const uint WS_VSCROLL = 0x200000;

	public const uint WS_EX_ACCEPTFILES = 0x00000010;
	public const uint WS_EX_APPWINDOW = 0x00040000;
	public const uint WS_EX_CLIENTEDGE = 0x00000200;
	public const uint WS_EX_COMPOSITED = 0x02000000;
	public const uint WS_EX_CONTEXTHELP = 0x00000400;
	public const uint WS_EX_CONTROLPARENT = 0x00010000;
	public const uint WS_EX_DLGMODALFRAME = 0x00000001;
	public const uint WS_EX_LAYERED = 0x00080000;
	public const uint WS_EX_LAYOUTRTL = 0x00400000;
	public const uint WS_EX_LEFT = 0x00000000;
	public const uint WS_EX_LEFTSCROLLBAR = 0x00004000;
	public const uint WS_EX_LTRREADING = 0x00000000;
	public const uint WS_EX_MDICHILD = 0x00000040;
	public const uint WS_EX_NOACTIVATE = 0x08000000;
	public const uint WS_EX_NOINHERITLAYOUT = 0x00100000;
	public const uint WS_EX_NOPARENTNOTIFY = 0x00000004;
	public const uint WS_EX_NOREDIRECTIONBITMAP = 0x00200000;
	public const uint WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE;
	public const uint WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST;
	public const uint WS_EX_RIGHT = 0x00001000;
	public const uint WS_EX_RIGHTSCROLLBAR = 0x00000000;
	public const uint WS_EX_RTLREADING = 0x00002000;
	public const uint WS_EX_STATICEDGE = 0x00020000;
	public const uint WS_EX_TOOLWINDOW = 0x00000080;
	public const uint WS_EX_TOPMOST = 0x00000008;
	public const uint WS_EX_TRANSPARENT = 0x00000020;
	public const uint WS_EX_WINDOWEDGE = 0x00000100;

	public static readonly IntPtr HWND_TOP = new( 0 );
	public static readonly IntPtr HWND_BOTTOM = new( 1 );
	public static readonly IntPtr HWND_TOPMOST = new( -1 );
	public static readonly IntPtr HWND_NOTOPMOST = new( -2 );

	public const int SWP_ASYNCWINDOWPOS = 0x4000;
	public const int SWP_DEFERERASE = 0x2000;
	public const int SWP_DRAWFRAME = 0x0020;
	public const int SWP_FRAMECHANGED = 0x0020;
	public const int SWP_HIDEWINDOW = 0x0080;
	public const int SWP_NOACTIVATE = 0x0010;
	public const int SWP_NOCOPYBITS = 0x0100;
	public const int SWP_NOMOVE = 0x0002;
	public const int SWP_NOOWNERZORDER = 0x0200;
	public const int SWP_NOREDRAW = 0x0008;
	public const int SWP_NOREPOSITION = 0x0200;
	public const int SWP_NOSENDCHANGING = 0x0400;
	public const int SWP_NOSIZE = 0x0001;
	public const int SWP_NOZORDER = 0x0004;
	public const int SWP_SHOWWINDOW = 0x0040;

	[StructLayout( LayoutKind.Sequential )]
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
	public static extern uint GetWindowLongPtr( IntPtr hWnd, int nIndex );

	[DllImport( "user32.dll" )]
	public static extern uint SetWindowLong( IntPtr hWnd, int nIndex, uint dwNewLong );

	[DllImport( "user32.dll", SetLastError = true )]
	public static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );

	[DllImport( "dwmapi.dll" )]
	public static extern int DwmExtendFrameIntoClientArea( IntPtr hwnd, ref MARGINS margins );
}
