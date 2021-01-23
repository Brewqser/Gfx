﻿using MathNet.Spatial.Euclidean;
using EMBC.Mathematics.Extensions;

namespace EMBC.Inputs
{
    public class InputWpf :
        Input
    {
        #region // storage

        private System.Windows.FrameworkElement Control { get; set; }

        public override System.Drawing.Size Size => new System.Drawing.Size((int)Control.ActualWidth, (int)Control.ActualHeight);

        public override event SizeEventHandler SizeChanged;

        public override event MouseEventHandler MouseMove;

        public override event MouseEventHandler MouseDown;

        public override event MouseEventHandler MouseUp;

        public override event MouseEventHandler MouseWheel;

        public override event KeyEventHandler KeyDown;

        public override event KeyEventHandler KeyUp;

        #endregion

        #region // ctor

        public InputWpf(System.Windows.FrameworkElement control)
        {
            Control = control;

            Control.SizeChanged += ControlOnSizeChanged;
            Control.MouseMove += ControlOnMouseMove;
            Control.MouseDown += ControlOnMouseDown;
            Control.MouseUp += ControlOnMouseUp;
            Control.MouseWheel += ControlOnMouseWheel;
            Control.KeyDown += ControlOnKeyDown;
            Control.KeyUp += ControlOnKeyUp;
        }

        public override void Dispose()
        {
            Control.SizeChanged -= ControlOnSizeChanged;
            Control.MouseMove -= ControlOnMouseMove;
            Control.MouseDown -= ControlOnMouseDown;
            Control.MouseUp -= ControlOnMouseUp;
            Control.MouseWheel -= ControlOnMouseWheel;
            Control.KeyDown -= ControlOnKeyDown;
            Control.KeyUp -= ControlOnKeyUp;

            Control = default;
        }

        #endregion

        #region // handlers

        private void ControlOnSizeChanged(object sender, System.Windows.SizeChangedEventArgs args) => SizeChanged?.Invoke(sender, new SizeEventArgs(Size));

        private void ControlOnMouseMove(object sender, System.Windows.Input.MouseEventArgs args) => MouseMove?.Invoke(sender, new MouseEventArgs(args, GetPosition(args), 0));

        private void ControlOnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs args) => MouseDown?.Invoke(sender, new MouseEventArgs(args, GetPosition(args), 0));

        private void ControlOnMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs args) => MouseUp?.Invoke(sender, new MouseEventArgs(args, GetPosition(args), 0));

        private void ControlOnMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs args) => MouseWheel?.Invoke(sender, new MouseEventArgs(args, GetPosition(args)));

        private void ControlOnKeyDown(object sender, System.Windows.Input.KeyEventArgs args) => KeyDown?.Invoke(sender, new KeyEventArgs(args));

        private void ControlOnKeyUp(object sender, System.Windows.Input.KeyEventArgs args) => KeyUp?.Invoke(sender, new KeyEventArgs(args));

        #endregion

        #region // routines

        private Point2D GetPosition(System.Windows.Input.MouseEventArgs args)
        {
            return args.GetPosition(Control).ToPoint2D();
        }

        #endregion
    }
}
