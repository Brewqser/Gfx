namespace EMBC.Inputs
{
    public class InputForms :
        Input
    {
        #region // storage

        private System.Windows.Forms.Control Control { get; set; }

        public override System.Drawing.Size Size => Control.Size;

        public override event SizeEventHandler SizeChanged;

        public override event MouseEventHandler MouseMove;

        public override event MouseEventHandler MouseDown;

        public override event MouseEventHandler MouseUp;

        public override event MouseEventHandler MouseWheel;

        public override event KeyEventHandler KeyDown;

        public override event KeyEventHandler KeyUp;

        #endregion

        #region // ctor

        public InputForms(System.Windows.Forms.Control control)
        {
            // store
            Control = control;

            // hook
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

        private void ControlOnSizeChanged(object sender, System.EventArgs args) => SizeChanged?.Invoke(sender, new SizeEventArgs(Control.Size));

        private void ControlOnMouseMove(object sender, System.Windows.Forms.MouseEventArgs args) => MouseMove?.Invoke(sender, new MouseEventArgs(args));

        private void ControlOnMouseDown(object sender, System.Windows.Forms.MouseEventArgs args) => MouseDown?.Invoke(sender, new MouseEventArgs(args));

        private void ControlOnMouseUp(object sender, System.Windows.Forms.MouseEventArgs args) => MouseUp?.Invoke(sender, new MouseEventArgs(args));

        private void ControlOnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs args) => MouseWheel?.Invoke(sender, new MouseEventArgs(args));

        private void ControlOnKeyDown(object sender, System.Windows.Forms.KeyEventArgs args) => KeyDown?.Invoke(sender, new KeyEventArgs(args));

        private void ControlOnKeyUp(object sender, System.Windows.Forms.KeyEventArgs args) => KeyUp?.Invoke(sender, new KeyEventArgs(args));

        #endregion
    }
}
