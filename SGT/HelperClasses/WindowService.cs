using System;

namespace SGT.HelperClasses
{
    public class WindowService
    {
        public IPageViewModel PageViewModel;
        public bool ShowDialog = false;
        public bool IsCloseButtonEnabled = true;
        public bool ShowDialogsOverTitleBar = false;
        public bool ShowInTaskbar = true;
        public bool IsOwnedByMainWindow = true;
        public double WindowWidth = 680;
        public double WindowHeight = 280;
        public double? WindowMinWidth = null;
        public double? WindowMinHeight = null;
        public double? WindowMaxWidth = null;
        public double? WindowMaxHeight = null;

        public Action CloseWindow { get; set; }

        public WindowService(IPageViewModel pageViewModel)
        {
            PageViewModel = pageViewModel;
        }
    }
}