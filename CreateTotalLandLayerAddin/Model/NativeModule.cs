using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeModule
{
    public class NativeMethods
    {

        public const Int32 STD_INPUT_HANDLE = -10;

        public const Int32 ENABLE_MOUSE_INPUT = 0x0010;
        public const Int32 ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const Int32 ENABLE_EXTENDED_FLAGS = 0x0080;

        public const Int32 KEY_EVENT = 1;
        public const Int32 MOUSE_EVENT = 2;


        [DebuggerDisplay("EventType: {EventType}")]
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public Int16 EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
        }

        [DebuggerDisplay("{dwMousePosition.X}, {dwMousePosition.Y}")]
        public struct MOUSE_EVENT_RECORD
        {
            public COORD dwMousePosition;
            public Int32 dwButtonState;
            public Int32 dwControlKeyState;
            public Int32 dwEventFlags;
        }

        [DebuggerDisplay("{X}, {Y}")]
        public struct COORD
        {
            public UInt16 X;
            public UInt16 Y;
        }

        [DebuggerDisplay("KeyCode: {wVirtualKeyCode}")]
        [StructLayout(LayoutKind.Explicit)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0)]
            [MarshalAsAttribute(UnmanagedType.Bool)]
            public Boolean bKeyDown;
            [FieldOffset(4)]
            public UInt16 wRepeatCount;
            [FieldOffset(6)]
            public UInt16 wVirtualKeyCode;
            [FieldOffset(8)]
            public UInt16 wVirtualScanCode;
            [FieldOffset(10)]
            public Char UnicodeChar;
            [FieldOffset(10)]
            public Byte AsciiChar;
            [FieldOffset(12)]
            public Int32 dwControlKeyState;
        };


        public class ConsoleHandle : SafeHandleMinusOneIsInvalid
        {
            public ConsoleHandle() : base(false) { }

            protected override bool ReleaseHandle()
            {
                return true; //releasing console handle is not our business
            }
        }

        //

        public static bool DisableConsoleQuickEditMode(out string errmsg)
        {
            errmsg = "";
            //disable QUICK_EDIT_MODE(因滑鼠點到console，變成選取模式，會造成程式暫停)
            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);
            int mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode)))
            {
                errmsg = "無法取得console的模式,後續關閉快速編輯模式(QUICK_EDIT_MODE)\n";
                return false;
                //throw new Win32Exception(); 
            }

            //mode |= NativeMethods.ENABLE_MOUSE_INPUT;//enable mouse input
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;//disable quick edit mode
                                                          //mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;
            if (!(NativeMethods.SetConsoleMode(handle, mode)))
            {
                errmsg = "無法關閉console的快速編輯模式(QUICK_EDIT_MODE)";
                return false;
                //throw new Win32Exception();
            }

            return true;
        }

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean GetConsoleMode(ConsoleHandle hConsoleHandle, ref Int32 lpMode);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean ReadConsoleInput(ConsoleHandle hConsoleInput, ref INPUT_RECORD lpBuffer, UInt32 nLength, ref UInt32 lpNumberOfEventsRead);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean SetConsoleMode(ConsoleHandle hConsoleHandle, Int32 dwMode);

    }
}
