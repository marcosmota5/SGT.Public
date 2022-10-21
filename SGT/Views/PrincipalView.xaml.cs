using ChromeTabs;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
using SGT.HelperClasses;
using SGT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.PresentationSource;

namespace SGT.Views
{
    /// <summary>
    /// Interaction logic for PrincipalView.xaml
    /// </summary>
    public partial class PrincipalView : UserControl
    {
        public PrincipalView()
        {
            InitializeComponent();
        }

        private void mniOrdensServicoPe2squisar_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Evento para mudar o menuitem e mostrá-lo na lateral direita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mniFerramentasRequisicoes.ApplyTemplate();
            Popup pop = mniFerramentasRequisicoes.Template.FindName("PART_Popup", mniFerramentasRequisicoes) as Popup;
            if (pop != null)
            {
                pop.Placement = PlacementMode.Right;
            }
        }

        private void PackIconMaterial_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AlteraExpander(expPropostas);
            AlteraExpander(expOrdensServico);
            AlteraExpander(expUsuarios);
            AlteraExpander(expFerramentas);
        }

        private void AlteraExpander(Expander expander)
        {
            try
            {
                if (TextoMenu.Visibility != Visibility.Visible)
                {
                    ExpanderHelper.SetHeaderDownStyle(expander, ExpanderHelper.GetHeaderDownStyle(expBase));
                    expander.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    Style style = this.FindResource("ExpanderDownHeaderStyle") as Style;

                    ExpanderHelper.SetHeaderDownStyle(expander, style);
                    expander.FlowDirection = FlowDirection.LeftToRight;
                }
            }
            catch (Exception)
            {
            }
        }

        // Teste da janela

        //We use this collection to keep track of what windows we have open
        protected List<DockingWindow> OpenWindows = new List<DockingWindow>();

        protected bool TryDockWindow(Point absoluteScreenPosition, IPageViewModel dockedWindowVM)
        {
            Point relativePoint = PointFromScreen(absoluteScreenPosition);//The screen position relative to the tab control
            //Hit test against the tab control
            if (ctChromeTabWindow.InputHitTest(relativePoint) is FrameworkElement element)
            {
                ////test if the mouse is over the tab panel or a tab item.
                if (CanInsertTabItem(element))
                {
                    //TabBase dockedWindowVM = (TabBase)win.DataContext;
                    PrincipalViewModel vm = (PrincipalViewModel)DataContext;
                    //System.Diagnostics.Trace.WriteLine(dockedWindowVM.Name);
                    try
                    {
                        ((dynamic)dockedWindowVM).EhMovimentacao = true;
                    }
                    catch (Exception)
                    {
                    }
                    vm.Titles.Add(dockedWindowVM);
                    vm.SelectedTitle = dockedWindowVM;
                    //We run this method on the tab control for it to grab the tab and position it at the mouse, ready to move again.
                    ctChromeTabWindow.GrabTab(dockedWindowVM);
                    return true;
                }
            }
            return false;
        }

        protected bool TryDragTabToWindow(Point position, IPageViewModel draggedTab)
        {
            //if (draggedTab is TabClass3)
            //    return false;//As an example, we don't want TabClass3 to form new windows, so we stop it here.
            //if (draggedTab.IsPinned)
            //    return false;//We don't want pinned tabs to be draggable either.

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
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToShortTimeString() + " got window");
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
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToShortTimeString() + " dragging window");

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
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToShortTimeString() + " closed window");
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

            if (windowUnder != null && windowUnder.Equals(Window.GetWindow(this)))
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

        private void TabControl_TabDraggedOutsideBonds(object sender, TabDragEventArgs e)
        {
            IPageViewModel draggedTab = e.Tab as IPageViewModel;
            if (TryDragTabToWindow(e.CursorPosition, draggedTab))
            {
                try
                {
                    ((dynamic)draggedTab).EhMovimentacao = true;
                }
                catch (Exception)
                {
                }
                //Set Handled to true to tell the tab control that we have dragged the tab to a window, and the tab should be closed.
                e.Handled = true;
            }
        }
    }
}