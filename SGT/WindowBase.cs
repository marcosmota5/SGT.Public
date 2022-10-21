﻿using ChromeTabs;
using SGT.HelperClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using static System.Windows.PresentationSource;

namespace SGT
{
    public abstract partial class WindowBase : Window
    {
        //We use this collection to keep track of what windows we have open
        protected List<DockingWindow> OpenWindows = new List<DockingWindow>();

        protected abstract bool TryDockWindow(Point absoluteScreenPosition, IPageViewModel dockedWindowVM);

        public WindowBase()
        {
        }

        protected bool TryDragTabToWindow(Point position, IPageViewModel draggedTab)
        {
            DockingWindow win = OpenWindows.FirstOrDefault(x => x.DataContext == draggedTab);//check if it's already open

            if (win == null)//If not, create a new one
            {
                win = new DockingWindow
                {
                    Title = draggedTab?.Name,
                    DataContext = draggedTab
                };

                win.Closed += win_Closed;
                win.Loaded += win_Loaded;
                win.LocationChanged += win_LocationChanged;
                win.Tag = position;
                var scale = VisualTreeHelper.GetDpi(this);
                win.Left = position.X / scale.DpiScaleX - win.Width / 2;
                win.Top = position.Y / scale.DpiScaleY - 10;

                win.Show();
            }
            else
            {
                Debug.WriteLine(DateTime.Now.ToShortTimeString() + " got window");
                MoveWindow(win, position);
            }
            OpenWindows.Add(win);
            return true;
        }

        private void win_Loaded(object sender, RoutedEventArgs e)
        {
            Window win = (Window)sender;
            win.Loaded -= win_Loaded;
            Point cursorPosition = (Point)win.Tag;
            MoveWindow(win, cursorPosition);
        }

        private void MoveWindow(Window win, Point pt)
        {
            //Use a BeginInvoke to delay the execution slightly, else we can have problems grabbing the newly opened window.
            Dispatcher.BeginInvoke(new Action(() =>
            {
                win.Topmost = true;
                //We position the window at the mouse position
                var scale = VisualTreeHelper.GetDpi(this);
                win.Left = pt.X / scale.DpiScaleX - win.Width / 2;
                win.Top = pt.Y / scale.DpiScaleY - 10;
                Debug.WriteLine(DateTime.Now.ToShortTimeString() + " dragging window");

                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    win.DragMove();//capture the movement to the mouse, so it can be dragged around
                }

                win.Topmost = false;
            }));
        }

        //remove the window from the open windows collection when it is closed.
        private void win_Closed(object sender, EventArgs e)
        {
            OpenWindows.Remove(sender as DockingWindow);
            Debug.WriteLine(DateTime.Now.ToShortTimeString() + " closed window");
        }

        //We use this to keep track of where the window is on the screen, so we can dock it later
        private void win_LocationChanged(object sender, EventArgs e)
        {
            Window win = (Window)sender;
            if (!win.IsLoaded)
                return;
            W32Point pt = new W32Point();
            if (!Win32.GetCursorPos(ref pt))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            Point absoluteScreenPos = new Point(pt.X, pt.Y);

            var windowUnder = FindWindowUnderThisAt(win, absoluteScreenPos);

            if (windowUnder != null && windowUnder.Equals(this))
            {
                if (TryDockWindow(absoluteScreenPos, win.DataContext as IPageViewModel))
                {
                    win.Close();
                }
            }
        }

        protected bool CanInsertTabItem(FrameworkElement element)
        {
            if (element is ChromeTabItem)
                return true;
            if (element is ChromeTabPanel)
                return true;
            object child = LogicalTreeHelper.GetChildren(element).Cast<object>().FirstOrDefault(x => x is ChromeTabPanel);
            if (child != null)
                return true;
            FrameworkElement localElement = element;
            while (true)
            {
                Object obj = localElement?.TemplatedParent;
                if (obj == null)
                    break;

                if (obj is ChromeTabItem)
                    return true;
                localElement = localElement.TemplatedParent as FrameworkElement;
            }
            return false;
        }

        /// <summary>
        /// Used P/Invoke to find and return the top window under the cursor position
        /// </summary>
        /// <param name="source"></param>
        /// <param name="screenPoint"></param>
        /// <returns></returns>
        private Window FindWindowUnderThisAt(Window source, Point screenPoint)  // WPF units (96dpi), not device units
        {
            //This prevents "UI debugging tools for XAML" from interfering when debugging.
            var allWindows = SortWindowsTopToBottom(Application.Current.Windows.OfType<Window>().Where(x => x.GetType().ToString() != "Microsoft.VisualStudio.DesignTools.WpfTap.WpfVisualTreeService.Adorners.AdornerWindow"
                                                                                                         && x.GetType().ToString() != "Microsoft.VisualStudio.DesignTools.WpfTap.WpfVisualTreeService.Adorners.AdornerLayerWindow"));
            var windowsUnderCurrent = from win in allWindows
                                      where (win.WindowState == WindowState.Maximized || Win32.GetWindowRect(win).Contains(screenPoint))
                                      && !Equals(win, source)
                                      select win;
            return windowsUnderCurrent.FirstOrDefault();
        }

        /// <summary>
        /// We need to do some P/Invoke magic to get the windows on screen
        /// </summary>
        /// <param name="unsorted"></param>
        /// <returns></returns>
        private IEnumerable<Window> SortWindowsTopToBottom(IEnumerable<Window> unsorted)
        {
            var byHandle = unsorted.ToDictionary(win =>
                ((HwndSource)FromVisual(win)).Handle);

            for (IntPtr hWnd = Win32.GetTopWindow(IntPtr.Zero); hWnd != IntPtr.Zero; hWnd = Win32.GetWindow(hWnd, Win32.GW_HWNDNEXT))
            {
                if (byHandle.ContainsKey(hWnd))
                    yield return byHandle[hWnd];
            }
        }
    }
}