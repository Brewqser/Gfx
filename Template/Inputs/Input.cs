using System;

namespace EMBC.Inputs
{
    public abstract class Input :
        IInput
    {
        #region // storage

        public abstract System.Drawing.Size Size { get; }

        public abstract event SizeEventHandler SizeChanged;

        public abstract event MouseEventHandler MouseMove;

        public abstract event MouseEventHandler MouseDown;

        public abstract event MouseEventHandler MouseUp;

        public abstract event MouseEventHandler MouseWheel;

        public abstract event KeyEventHandler KeyDown;

        public abstract event KeyEventHandler KeyUp;

        #endregion

        #region // ctor

        protected Input()
        {
            // TODO: debug
            Test.Subscribe(this);
        }

        public virtual void Dispose()
        {
            // TODO: debug
            Test.Unsubscribe(this);
        }

        #endregion

        #region // test

        private static class Test
        {
            public static void Subscribe(IInput input)
            {
                input.SizeChanged += InputOnSizeChanged;
                input.MouseMove += InputOnMouseMove;
                input.MouseDown += InputOnMouseDown;
                input.MouseUp += InputOnMouseUp;
                input.MouseWheel += InputOnMouseWheel;
                input.KeyDown += InputOnKeyDown;
                input.KeyUp += InputOnKeyUp;
            }

            public static void Unsubscribe(IInput input)
            {
                input.SizeChanged -= InputOnSizeChanged;
                input.MouseMove -= InputOnMouseMove;
                input.MouseDown -= InputOnMouseDown;
                input.MouseUp -= InputOnMouseUp;
                input.MouseWheel -= InputOnMouseWheel;
                input.KeyDown -= InputOnKeyDown;
                input.KeyUp -= InputOnKeyUp;
            }

            private static void InputOnSizeChanged(object sender, ISizeEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.SizeChanged)} {args.NewSize}");
            }

            private static void InputOnMouseMove(object sender, IMouseEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.MouseMove)} {args.Position}");
            }

            private static void InputOnMouseDown(object sender, IMouseEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.MouseDown)} {args.Position} {args.Buttons}");
            }

            private static void InputOnMouseUp(object sender, IMouseEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.MouseUp)} {args.Position} {args.Buttons}");
            }

            private static void InputOnMouseWheel(object sender, IMouseEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.MouseWheel)} {args.Position} {args.WheelDelta}");
            }

            private static void InputOnKeyDown(object sender, IKeyEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.KeyDown)} {args.Modifiers} {args.Key}");
            }

            private static void InputOnKeyUp(object sender, IKeyEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.KeyUp)} {args.Modifiers} {args.Key}");
            }
        }

        #endregion
    }
}
