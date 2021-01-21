using System;

namespace EMBC.Inputs
{
    public class KeyEventArgs :
        EventArgs,
        IKeyEventArgs
    {
        #region // storage

        public Key Key { get; }

        public Modifiers Modifiers { get; }

        #endregion

        #region // ctor

        public KeyEventArgs(Key key, bool modifierControl, bool modifierAlt, bool modifierShift, bool modifierWindows)
        {
            Key = key;
            Modifiers |= modifierControl ? Modifiers.Control : Modifiers.None;
            Modifiers |= modifierAlt ? Modifiers.Alt : Modifiers.None;
            Modifiers |= modifierShift ? Modifiers.Shift : Modifiers.None;
            Modifiers |= modifierWindows ? Modifiers.Windows : Modifiers.None;
        }

        public KeyEventArgs(System.Windows.Forms.KeyEventArgs args) :
            this
            (
                (Key)System.Windows.Input.KeyInterop.KeyFromVirtualKey((int)args.KeyCode),
                ((args.Modifiers & System.Windows.Forms.Keys.Control) | (args.Modifiers & System.Windows.Forms.Keys.LControlKey) | (args.Modifiers & System.Windows.Forms.Keys.RControlKey)) != 0,
                (args.Modifiers & System.Windows.Forms.Keys.Alt) != 0,
                ((args.Modifiers & System.Windows.Forms.Keys.Shift) | (args.Modifiers & System.Windows.Forms.Keys.ShiftKey) | (args.Modifiers & System.Windows.Forms.Keys.RShiftKey) | (args.Modifiers & System.Windows.Forms.Keys.LShiftKey)) != 0,
                ((args.Modifiers & System.Windows.Forms.Keys.LWin) | (args.Modifiers & System.Windows.Forms.Keys.RWin)) != 0
            )
        {
        }

        public KeyEventArgs(System.Windows.Input.KeyEventArgs args) :
            this
            (
                (Key)args.Key,
                args.KeyboardDevice.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Control),
                args.KeyboardDevice.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Alt),
                args.KeyboardDevice.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Shift),
                args.KeyboardDevice.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Windows)
            )
        {
        }

        #endregion
    }
}
