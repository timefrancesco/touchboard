using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Runtime.InteropServices;
using MonoMac;
using System.Threading.Tasks;
using System.Threading;

namespace ninjaKeyserverosx
{
	public unsafe static class KeyHandler
	{
		[DllImport(Constants.CoreGraphicsLibrary)]
		public static extern IntPtr CGEventCreateKeyboardEvent(IntPtr source, ushort virtualKey, bool keyDown);

		[DllImport(Constants.CoreGraphicsLibrary)]
		public static extern IntPtr CGEventSetFlags(IntPtr sourceEvent, int flags);

		[DllImport(Constants.CoreGraphicsLibrary)]
		public static extern void CGEventKeyboardSetUnicodeString(IntPtr @event, ulong stringLength, char* unicodeString);

		[DllImport(Constants.CoreGraphicsLibrary)]
		public static extern IntPtr CGEventSourceCreate(CGEventSourceStateID stateID);

		[DllImport(Constants.CoreGraphicsLibrary)]
		public static extern void CGEventPost(CGEventTapLocation tap, IntPtr @event);

		[DllImport(Constants.CoreFoundationLibrary)]
		public static extern void CFRelease (IntPtr cf);

		public enum CGEventTapLocation:uint
		{
			kCGHIDEventTap = 0,

			kCGSessionEventTap,

			kCGAnnotatedSessionEventTap
		}

		public enum CGEventSourceStateID:int
		{
			kCGEventSourceStatePrivate = -1,

			kCGEventSourceStateCombinedSessionState = 0,

			kCGEventSourceStateHIDSystemState = 1
		}

		public enum CGEventFlags : int
		{
			kCGEventFlagMaskShift = 0x00020000,
			kCGEventFlagMaskControl = 0x00040000,
			kCGEventFlagMaskAlternate = 0x00080000, //OPTION
			kCGEventFlagMaskCommand = 0x00100000
		}

		public unsafe static void SendKey(List<int> commands)
		{
			IntPtr eventSource = CGEventSourceCreate(CGEventSourceStateID.kCGEventSourceStateHIDSystemState);
			int keyMask = 0;
			List<VirtualKeyCodeOSX> nonModifierKeys = new List<VirtualKeyCodeOSX>();

			foreach (var cmd in commands)
			{
				VirtualKeyCodeOSX newCmd = (VirtualKeyCodeOSX)cmd;

				if (newCmd == VirtualKeyCodeOSX.LCONTROL || newCmd == VirtualKeyCodeOSX.RCONTROL)
					keyMask |= (int)CGEventFlags.kCGEventFlagMaskControl;

				else if (newCmd == VirtualKeyCodeOSX.RCMD || newCmd == VirtualKeyCodeOSX.LCMD)
					keyMask |= (int)CGEventFlags.kCGEventFlagMaskCommand;

				else if (newCmd == VirtualKeyCodeOSX.RSHIFT || newCmd == VirtualKeyCodeOSX.LSHIFT)
					keyMask |= (int)CGEventFlags.kCGEventFlagMaskShift;

				else if (newCmd == VirtualKeyCodeOSX.RALT || newCmd == VirtualKeyCodeOSX.LALT)
					keyMask |= (int)CGEventFlags.kCGEventFlagMaskAlternate;
				else
					nonModifierKeys.Add(newCmd);
			}

			if (keyMask != 0 && nonModifierKeys.Count > 0)
			{
				var keyEventDown = CGEventCreateKeyboardEvent(eventSource, (ushort)nonModifierKeys[0], true);
				CGEventSetFlags(keyEventDown, keyMask);
				CGEventPost(CGEventTapLocation.kCGHIDEventTap, keyEventDown);
				CFRelease(keyEventDown);
			}
			else if (keyMask == 0 && nonModifierKeys.Count > 0)
			{
				foreach (var cmd in nonModifierKeys) {
					var keyEventDown = CGEventCreateKeyboardEvent(eventSource, (ushort)cmd, true);
					var keyEventUp = CGEventCreateKeyboardEvent(eventSource, (ushort)cmd, false);

					CGEventPost(CGEventTapLocation.kCGHIDEventTap, keyEventDown);
					CGEventPost(CGEventTapLocation.kCGHIDEventTap, keyEventUp);

					CFRelease(keyEventDown);
					CFRelease(keyEventUp);
				}
			}
		}
	}
}

