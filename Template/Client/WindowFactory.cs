﻿using EMBC.Engine.Render;
using EMBC.Utils;
using EMBC.Win;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMBC.Client
{
    public static class WindowFactory
    {
        public static IReadOnlyList<IRenderHost> SeedWindows()
        {
            var size = new System.Drawing.Size(720, 480);

            var renderHosts = new[]
            {
                CreateWindowForm(size, "Forms Gdi", h => new Drivers.Gdi.Render.RenderHost(h)),
                CreateWindowWpf(size, "Wpf Gdi", h => new Drivers.Gdi.Render.RenderHost(h))
            };

            SortWindows(renderHosts);

            return renderHosts;
        }

        private static IRenderHost CreateWindowForm(System.Drawing.Size size, string title, Func<IntPtr, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Forms.Form()
            {
                Size = size,
                Text = title
            };

            var hostControl = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.Transparent,
                ForeColor = System.Drawing.Color.Transparent
            };
            window.Controls.Add(hostControl);

            hostControl.MouseEnter += (sender, args) =>
            {
                if (System.Windows.Forms.Form.ActiveForm != window) window.Activate();
                if (!hostControl.Focused) hostControl.Focus();
            };

            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            window.Show();
            return ctorRenderHost(hostControl.Handle);
        }

        private static IRenderHost CreateWindowWpf(System.Drawing.Size size, string title, Func<IntPtr, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Window()
            {
                Width = size.Width,
                Height = size.Height,
                Title = title
            };

            var hostControl = new System.Windows.Controls.Grid
            {
                Background = System.Windows.Media.Brushes.Transparent,
                Focusable = true
            };
            window.Content = hostControl;

            hostControl.MouseEnter += (sender, args) =>
            {
                if (!window.IsActive) window.Activate();
                if (!hostControl.IsFocused) hostControl.Focus();
            };

            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            window.Show();

            return ctorRenderHost(hostControl.Handle());
        }

        private static void SortWindows(IEnumerable<IRenderHost> renderHosts)
        {
            var windowsInfos = renderHosts.Select(renderHost => new WindowInfo(renderHost.HostHandle).GetRoot()).ToArray();

            var maxSize = new System.Drawing.Size(windowsInfos.Max(a => a.RectangleWindow.Width), windowsInfos.Max(a => a.RectangleWindow.Height));

            var maxColums = (int) Math.Ceiling(Math.Sqrt(windowsInfos.Length));
            var maxRows = (int) Math.Ceiling((double)windowsInfos.Length / maxColums);

            var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
            var left = primaryScreen.WorkingArea.Width / 2 - maxColums * maxSize.Width / 2;
            var top = primaryScreen.WorkingArea.Height / 2 - maxRows * maxSize.Height / 2;

            for (var row = 0 ; row < maxRows; row++)
            {
                for (var col = 0; col < maxColums; col++)
                {
                    var i = row * maxColums + col;
                    if (i >= windowsInfos.Length) return;

                    var x = col * maxSize.Width + left;
                    var y = row * maxSize.Height + top;

                    var windowInfo = windowsInfos[i];

                    User32.MoveWindow(windowInfo.Handle, x, y, windowInfo.RectangleWindow.Width, windowInfo.RectangleWindow.Height, false);
                }
            }
        }
    }
}