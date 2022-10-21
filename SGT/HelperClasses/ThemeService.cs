using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGT.HelperClasses
{
    public class ThemeService
    {
        public ThemeService(string color, string systemName, string displayName)
        {
            Color = color;
            SystemName = systemName;
            DisplayName = displayName;
        }

        public string Color { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
    }
}