using System;
using System.Collections.Generic;

namespace KeyboardCompanion
{
	public static class MacKeyCodes
	{
		public static Dictionary<VirtualKeyCodeWIN, int> OsxKeysDictionary = new Dictionary<VirtualKeyCodeWIN, int> () {
			{  VirtualKeyCodeWIN.TAB, 48},
			{  VirtualKeyCodeWIN.BACK, 117},
			{  VirtualKeyCodeWIN.RETURN, 36},
			{  VirtualKeyCodeWIN.CAPITAL, 0x39},
			{  VirtualKeyCodeWIN.ESCAPE, 0x35},
			{  VirtualKeyCodeWIN.SPACE, 0x31},
			{  VirtualKeyCodeWIN.PRIOR, 0x74},
			{  VirtualKeyCodeWIN.NEXT, 0x79},
			{  VirtualKeyCodeWIN.END, 0x77},
			{  VirtualKeyCodeWIN.HOME, 0x73},
			{  VirtualKeyCodeWIN.LEFT, 0x7B},
			{  VirtualKeyCodeWIN.UP, 0x7E},
			{  VirtualKeyCodeWIN.RIGHT, 0x7C},
			{  VirtualKeyCodeWIN.DOWN, 0x7D},
			{  VirtualKeyCodeWIN.INSERT, 0x72},
			{  VirtualKeyCodeWIN.DELETE, 0x75},
			{  VirtualKeyCodeWIN.KEY_0, 0x1D},
			{  VirtualKeyCodeWIN.KEY_1, 0x12},
			{  VirtualKeyCodeWIN.KEY_2, 0x13},
			{  VirtualKeyCodeWIN.KEY_3, 0x14},
			{  VirtualKeyCodeWIN.KEY_4, 0x15},
			{  VirtualKeyCodeWIN.KEY_5, 0x17},
			{  VirtualKeyCodeWIN.KEY_6, 0x16},
			{  VirtualKeyCodeWIN.KEY_7, 0x1A},
			{  VirtualKeyCodeWIN.KEY_8, 0x1C},
			{  VirtualKeyCodeWIN.KEY_9, 0x19},
			{  VirtualKeyCodeWIN.KEY_A, 0},
			{  VirtualKeyCodeWIN.KEY_B, 0xB},
			{  VirtualKeyCodeWIN.KEY_C, 0x08},
			{  VirtualKeyCodeWIN.KEY_D, 2},
			{  VirtualKeyCodeWIN.KEY_E, 0xE},
			{  VirtualKeyCodeWIN.KEY_F, 3},
			{  VirtualKeyCodeWIN.KEY_G, 5},
			{  VirtualKeyCodeWIN.KEY_H, 4},
			{  VirtualKeyCodeWIN.KEY_I, 0x22},
			{  VirtualKeyCodeWIN.KEY_J, 0x26},
			{  VirtualKeyCodeWIN.KEY_K, 0x28},
			{  VirtualKeyCodeWIN.KEY_L, 0x25},
			{  VirtualKeyCodeWIN.KEY_M, 0x2E},
			{  VirtualKeyCodeWIN.KEY_N, 0x2D},
			{  VirtualKeyCodeWIN.KEY_O, 0x1F},	
			{  VirtualKeyCodeWIN.KEY_P, 0x23},
			{  VirtualKeyCodeWIN.KEY_Q, 0xC},
			{  VirtualKeyCodeWIN.KEY_R, 0xF},
			{  VirtualKeyCodeWIN.KEY_S, 1},
			{  VirtualKeyCodeWIN.KEY_T, 0x11},
			{  VirtualKeyCodeWIN.KEY_U, 0x20},
			{  VirtualKeyCodeWIN.KEY_V, 0x9},
			{  VirtualKeyCodeWIN.KEY_W, 0xD},
			{  VirtualKeyCodeWIN.KEY_X, 7},
			{  VirtualKeyCodeWIN.KEY_Y, 0x10},
			{  VirtualKeyCodeWIN.KEY_Z, 6},
			{  VirtualKeyCodeWIN.LWIN, 0x37},
			{  VirtualKeyCodeWIN.RWIN, 0x37},
			{  VirtualKeyCodeWIN.NUMPAD0, 0x52},
			{  VirtualKeyCodeWIN.NUMPAD1, 0x53},
			{  VirtualKeyCodeWIN.NUMPAD2, 0x54},
			{  VirtualKeyCodeWIN.NUMPAD3, 0x55},
			{  VirtualKeyCodeWIN.NUMPAD4, 0x56},
			{  VirtualKeyCodeWIN.NUMPAD5, 0x57},
			{  VirtualKeyCodeWIN.NUMPAD6, 0x58},
			{  VirtualKeyCodeWIN.NUMPAD7, 0x5},
			{  VirtualKeyCodeWIN.NUMPAD8, 0x5B},
			{  VirtualKeyCodeWIN.NUMPAD9, 0x5C},
			{  VirtualKeyCodeWIN.MULTIPLY, 0x43},
			{  VirtualKeyCodeWIN.ADD, 0x45},
			{  VirtualKeyCodeWIN.SEPARATOR, 0x4B},
			{  VirtualKeyCodeWIN.SUBTRACT, 0x4E},
			{  VirtualKeyCodeWIN.DECIMAL, 0x41},
			{  VirtualKeyCodeWIN.DIVIDE, 0x4B},
			{  VirtualKeyCodeWIN.F1, 0x7A},
			{  VirtualKeyCodeWIN.F2, 0x78},
			{  VirtualKeyCodeWIN.F3, 0x63},
			{  VirtualKeyCodeWIN.F4, 0x76},
			{  VirtualKeyCodeWIN.F5, 0x60},
			{  VirtualKeyCodeWIN.F6, 0x61},
			{  VirtualKeyCodeWIN.F7, 0x62},
			{  VirtualKeyCodeWIN.F8, 0x64},
			{  VirtualKeyCodeWIN.F9, 0x65},
			{  VirtualKeyCodeWIN.F10, 0x6D},
			{  VirtualKeyCodeWIN.F11, 0x67},
			{  VirtualKeyCodeWIN.F12, 0x6F},
			{  VirtualKeyCodeWIN.F13, 0x69},
			{  VirtualKeyCodeWIN.F14, 0x6B},
			{  VirtualKeyCodeWIN.F15, 0x71},


			{  VirtualKeyCodeWIN.NUMLOCK, 0x47},
			{  VirtualKeyCodeWIN.LSHIFT, 0x38},
			{  VirtualKeyCodeWIN.RSHIFT, 0x38},
			{  VirtualKeyCodeWIN.LCONTROL, 0x3B},
			{  VirtualKeyCodeWIN.RCONTROL, 0x3B},
			{  VirtualKeyCodeWIN.LALT, 0x3A},
			{  VirtualKeyCodeWIN.RALT, 0x3A},
			{  VirtualKeyCodeWIN.OEM_COLON, 0x29},

			{  VirtualKeyCodeWIN.OEM_PLUS, 0x18},
			{  VirtualKeyCodeWIN.OEM_COMMA, 0x2B},
			{  VirtualKeyCodeWIN.OEM_MINUS, 0x1B},
			{  VirtualKeyCodeWIN.OEM_PERIOD, 0x2F},
			{  VirtualKeyCodeWIN.OEM_SLASH, 0x2C},
			{  VirtualKeyCodeWIN.OEM_TILDE, 0x32},
			{  VirtualKeyCodeWIN.OEM_OP_BRACKET, 0x21},
			{  VirtualKeyCodeWIN.OEM_BKSLASH, 0x2A},
			{  VirtualKeyCodeWIN.OEM_CL_BRACKET, 0x1E},
			{  VirtualKeyCodeWIN.OEM_QUOTE, 0x27},

		
		};

	}

	//
	// Summary:
	//     The list of VirtualKeyCodes (see: http://msdn.microsoft.com/en-us/library/ms645540(VS.85).aspx)
	public enum VirtualKeyCodeWIN
	{
		//
		// Summary:
		//     Left mouse button
		LBUTTON = 1,
		//
		// Summary:
		//     Right mouse button
		RBUTTON = 2,
		//
		// Summary:
		//     Control-break processing
		CANCEL = 3,
		//
		// Summary:
		//     Middle mouse button (three-button mouse) - NOT contiguous with LBUTTON and RBUTTON
		MBUTTON = 4,
		//
		// Summary:
		//     Windows 2000/XP: X1 mouse button - NOT contiguous with LBUTTON and RBUTTON
		XBUTTON1 = 5,
		//
		// Summary:
		//     Windows 2000/XP: X2 mouse button - NOT contiguous with LBUTTON and RBUTTON
		XBUTTON2 = 6,
		//
		// Summary:
		//     BACKSPACE key
		BACK = 8,
		//
		// Summary:
		//     TAB key
		TAB = 9,
		//
		// Summary:
		//     CLEAR key
		CLEAR = 12,
		//
		// Summary:
		//     ENTER key
		RETURN = 13,
		//
		// Summary:
		//     SHIFT key
		SHIFT = 16,
		//
		// Summary:
		//     CTRL key
		CONTROL = 17,
		//
		// Summary:
		//     ALT key
		MENU = 18,
		//
		// Summary:
		//     PAUSE key
		PAUSE = 19,
		//
		// Summary:
		//     CAPS LOCK key
		CAPITAL = 20,
		//
		// Summary:
		//     Input Method Editor (IME) Kana mode
		KANA = 21,
		//
		// Summary:
		//     IME Hanguel mode (maintained for compatibility; use HANGUL)
		HANGEUL = 21,
		//
		// Summary:
		//     IME Hangul mode
		HANGUL = 21,
		//
		// Summary:
		//     IME Junja mode
		JUNJA = 23,
		//
		// Summary:
		//     IME final mode
		FINAL = 24,
		//
		// Summary:
		//     IME Hanja mode
		HANJA = 25,
		//
		// Summary:
		//     IME Kanji mode
		KANJI = 25,
		//
		// Summary:
		//     ESC key
		ESCAPE = 27,
		//
		// Summary:
		//     IME convert
		CONVERT = 28,
		//
		// Summary:
		//     IME nonconvert
		NONCONVERT = 29,
		//
		// Summary:
		//     IME accept
		ACCEPT = 30,
		//
		// Summary:
		//     IME mode change request
		MODECHANGE = 31,
		//
		// Summary:
		//     SPACEBAR
		SPACE = 32,
		//
		// Summary:
		//     PAGE UP key
		PRIOR = 33,
		//
		// Summary:
		//     PAGE DOWN key
		NEXT = 34,
		//
		// Summary:
		//     END key
		END = 35,
		//
		// Summary:
		//     HOME key
		HOME = 36,
		//
		// Summary:
		//     LEFT ARROW key
		LEFT = 37,
		//
		// Summary:
		//     UP ARROW key
		UP = 38,
		//
		// Summary:
		//     RIGHT ARROW key
		RIGHT = 39,
		//
		// Summary:
		//     DOWN ARROW key
		DOWN = 40,
		//
		// Summary:
		//     SELECT key
		SELECT = 41,
		//
		// Summary:
		//     PRINT key
		PRINT = 42,
		//
		// Summary:
		//     EXECUTE key
		EXECUTE = 43,
		//
		// Summary:
		//     PRINT SCREEN key
		SNAPSHOT = 44,
		//
		// Summary:
		//     INS key
		INSERT = 45,
		//
		// Summary:
		//     DEL key
		DELETE = 46,
		//
		// Summary:
		//     HELP key
		HELP = 47,
		//
		// Summary:
		//     0 key
		KEY_0 = 48,
		//
		// Summary:
		//     1 key
		KEY_1 = 49,
		//
		// Summary:
		//     2 key
		KEY_2 = 50,
		//
		// Summary:
		//     3 key
		KEY_3 = 51,
		//
		// Summary:
		//     4 key
		KEY_4 = 52,
		//
		// Summary:
		//     5 key
		KEY_5 = 53,
		//
		// Summary:
		//     6 key
		KEY_6 = 54,
		//
		// Summary:
		//     7 key
		KEY_7 = 55,
		//
		// Summary:
		//     8 key
		KEY_8 = 56,
		//
		// Summary:
		//     9 key
		KEY_9 = 57,
		//
		// Summary:
		//     A key
		KEY_A = 65,
		//
		// Summary:
		//     B key
		KEY_B = 66,
		//
		// Summary:
		//     C key
		KEY_C = 67,
		//
		// Summary:
		//     D key
		KEY_D = 68,
		//
		// Summary:
		//     E key
		KEY_E = 69,
		//
		// Summary:
		//     F key
		KEY_F = 70,
		//
		// Summary:
		//     G key
		KEY_G = 71,
		//
		// Summary:
		//     H key
		KEY_H = 72,
		//
		// Summary:
		//     I key
		KEY_I = 73,
		//
		// Summary:
		//     J key
		KEY_J = 74,
		//
		// Summary:
		//     K key
		KEY_K = 75,
		//
		// Summary:
		//     L key
		KEY_L = 76,
		//
		// Summary:
		//     M key
		KEY_M = 77,
		//
		// Summary:
		//     N key
		KEY_N = 78,
		//
		// Summary:
		//     O key
		KEY_O = 79,
		//
		// Summary:
		//     P key
		KEY_P = 80,
		//
		// Summary:
		//     Q key
		KEY_Q = 81,
		//
		// Summary:
		//     R key
		KEY_R = 82,
		//
		// Summary:
		//     S key
		KEY_S = 83,
		//
		// Summary:
		//     T key
		KEY_T = 84,
		//
		// Summary:
		//     U key
		KEY_U = 85,
		//
		// Summary:
		//     V key
		KEY_V = 86,
		//
		// Summary:
		//     W key
		KEY_W = 87,
		//
		// Summary:
		//     X key
		KEY_X = 88,
		//
		// Summary:
		//     Y key
		KEY_Y = 89,
		//
		// Summary:
		//     Z key
		KEY_Z = 90,
		//
		// Summary:
		//     Left Windows key (Microsoft Natural keyboard)
		LWIN = 91,
		//
		// Summary:
		//     Right Windows key (Natural keyboard)
		RWIN = 92,
		//
		// Summary:
		//     Applications key (Natural keyboard)
		APPS = 93,
		//
		// Summary:
		//     Computer Sleep key
		SLEEP = 95,
		//
		// Summary:
		//     Numeric keypad 0 key
		NUMPAD0 = 96,
		//
		// Summary:
		//     Numeric keypad 1 key
		NUMPAD1 = 97,
		//
		// Summary:
		//     Numeric keypad 2 key
		NUMPAD2 = 98,
		//
		// Summary:
		//     Numeric keypad 3 key
		NUMPAD3 = 99,
		//
		// Summary:
		//     Numeric keypad 4 key
		NUMPAD4 = 100,
		//
		// Summary:
		//     Numeric keypad 5 key
		NUMPAD5 = 101,
		//
		// Summary:
		//     Numeric keypad 6 key
		NUMPAD6 = 102,
		//
		// Summary:
		//     Numeric keypad 7 key
		NUMPAD7 = 103,
		//
		// Summary:
		//     Numeric keypad 8 key
		NUMPAD8 = 104,
		//
		// Summary:
		//     Numeric keypad 9 key
		NUMPAD9 = 105,
		//
		// Summary:
		//     Multiply key
		MULTIPLY = 106,
		//
		// Summary:
		//     Add key
		ADD = 107,
		//
		// Summary:
		//     Separator key
		SEPARATOR = 108,
		//
		// Summary:
		//     Subtract key
		SUBTRACT = 109,
		//
		// Summary:
		//     Decimal key
		DECIMAL = 110,
		//
		// Summary:
		//     Divide key
		DIVIDE = 111,
		//
		// Summary:
		//     F1 key
		F1 = 112,
		//
		// Summary:
		//     F2 key
		F2 = 113,
		//
		// Summary:
		//     F3 key
		F3 = 114,
		//
		// Summary:
		//     F4 key
		F4 = 115,
		//
		// Summary:
		//     F5 key
		F5 = 116,
		//
		// Summary:
		//     F6 key
		F6 = 117,
		//
		// Summary:
		//     F7 key
		F7 = 118,
		//
		// Summary:
		//     F8 key
		F8 = 119,
		//
		// Summary:
		//     F9 key
		F9 = 120,
		//
		// Summary:
		//     F10 key
		F10 = 121,
		//
		// Summary:
		//     F11 key
		F11 = 122,
		//
		// Summary:
		//     F12 key
		F12 = 123,
		//
		// Summary:
		//     F13 key
		F13 = 124,
		//
		// Summary:
		//     F14 key
		F14 = 125,
		//
		// Summary:
		//     F15 key
		F15 = 126,
		//
		// Summary:
		//     F16 key
		F16 = 127,
		//
		// Summary:
		//     F17 key
		F17 = 128,
		//
		// Summary:
		//     F18 key
		F18 = 129,
		//
		// Summary:
		//     F19 key
		F19 = 130,
		//
		// Summary:
		//     F20 key
		F20 = 131,
		//
		// Summary:
		//     F21 key
		F21 = 132,
		//
		// Summary:
		//     F22 key
		F22 = 133,
		//
		// Summary:
		//     F23 key
		F23 = 134,
		//
		// Summary:
		//     F24 key
		F24 = 135,
		//
		// Summary:
		//     NUM LOCK key
		NUMLOCK = 144,
		//
		// Summary:
		//     SCROLL LOCK key
		SCROLL = 145,
		//
		// Summary:
		//     Left SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
		LSHIFT = 160,
		//
		// Summary:
		//     Right SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
		RSHIFT = 161,
		//
		// Summary:
		//     Left CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
		LCONTROL = 162,
		//
		// Summary:
		//     Right CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
		RCONTROL = 163,
		//
		// Summary:
		//     Left MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
		LALT = 164,
		//
		// Summary:
		//     Right MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
		RALT = 165,
		//
		// Summary:
		//     Windows 2000/XP: Browser Back key
		BROWSER_BACK = 166,
		//
		// Summary:
		//     Windows 2000/XP: Browser Forward key
		BROWSER_FORWARD = 167,
		//
		// Summary:
		//     Windows 2000/XP: Browser Refresh key
		BROWSER_REFRESH = 168,
		//
		// Summary:
		//     Windows 2000/XP: Browser Stop key
		BROWSER_STOP = 169,
		//
		// Summary:
		//     Windows 2000/XP: Browser Search key
		BROWSER_SEARCH = 170,
		//
		// Summary:
		//     Windows 2000/XP: Browser Favorites key
		BROWSER_FAVORITES = 171,
		//
		// Summary:
		//     Windows 2000/XP: Browser Start and Home key
		BROWSER_HOME = 172,
		//
		// Summary:
		//     Windows 2000/XP: Volume Mute key
		VOLUME_MUTE = 173,
		//
		// Summary:
		//     Windows 2000/XP: Volume Down key
		VOLUME_DOWN = 174,
		//
		// Summary:
		//     Windows 2000/XP: Volume Up key
		VOLUME_UP = 175,
		//
		// Summary:
		//     Windows 2000/XP: Next Track key
		MEDIA_NEXT_TRACK = 176,
		//
		// Summary:
		//     Windows 2000/XP: Previous Track key
		MEDIA_PREV_TRACK = 177,
		//
		// Summary:
		//     Windows 2000/XP: Stop Media key
		MEDIA_STOP = 178,
		//
		// Summary:
		//     Windows 2000/XP: Play/Pause Media key
		MEDIA_PLAY_PAUSE = 179,
		//
		// Summary:
		//     Windows 2000/XP: Start Mail key
		LAUNCH_MAIL = 180,
		//
		// Summary:
		//     Windows 2000/XP: Select Media key
		LAUNCH_MEDIA_SELECT = 181,
		//
		// Summary:
		//     Windows 2000/XP: Start Application 1 key
		LAUNCH_APP1 = 182,
		//
		// Summary:
		//     Windows 2000/XP: Start Application 2 key
		LAUNCH_APP2 = 183,
		//
		// Summary:
		//     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
		//     For the US standard keyboard, the ';:' key
		OEM_COLON = 186,
		//
		// Summary:
		//     Windows 2000/XP: For any country/region, the '+' key
		OEM_PLUS = 187,
		//
		// Summary:
		//     Windows 2000/XP: For any country/region, the ',' key
		OEM_COMMA = 188,
		//
		// Summary:
		//     Windows 2000/XP: For any country/region, the '-' key
		OEM_MINUS = 189,
		//
		// Summary:
		//     Windows 2000/XP: For any country/region, the '.' key
		OEM_PERIOD = 190,
		//
		// Summary:
		//     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
		//     For the US standard keyboard, the '/?' key
		OEM_SLASH = 191,
		//
		// Summary:
		//     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
		//     For the US standard keyboard, the '`~' key
		OEM_TILDE = 192,
		//
		// Summary:
		//     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
		//     For the US standard keyboard, the '[{' key
		OEM_OP_BRACKET = 219,
		//
		// Summary:
		//     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
		//     For the US standard keyboard, the '\|' key
		OEM_BKSLASH = 220,
		//
		// Summary:
		//     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
		//     For the US standard keyboard, the ']}' key
		OEM_CL_BRACKET = 221,
		//
		// Summary:
		//     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
		//     For the US standard keyboard, the 'single-quote/double-quote' key
		OEM_QUOTE = 222,
		//
		// Summary:
		//     Used for miscellaneous characters; it can vary by keyboard.
		OEM_8 = 223,
		//
		// Summary:
		//     Windows 2000/XP: Either the angle bracket key or the backslash key on the RT
		//     102-key keyboard
		OEM_102 = 226,
		//
		// Summary:
		//     Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
		PROCESSKEY = 229,
		//
		// Summary:
		//     Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
		//     The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard
		//     input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN,
		//     and WM_KEYUP
		PACKET = 231,
		//
		// Summary:
		//     Attn key
		ATTN = 246,
		//
		// Summary:
		//     CrSel key
		CRSEL = 247,
		//
		// Summary:
		//     ExSel key
		EXSEL = 248,
		//
		// Summary:
		//     Erase EOF key
		EREOF = 249,
		//
		// Summary:
		//     Play key
		PLAY = 250,
		//
		// Summary:
		//     Zoom key
		ZOOM = 251,
		//
		// Summary:
		//     Reserved
		NONAME = 252,
		//
		// Summary:
		//     PA1 key
		PA1 = 253,
		//
		// Summary:
		//     Clear key
		OEM_CLEAR = 254
	}
}

