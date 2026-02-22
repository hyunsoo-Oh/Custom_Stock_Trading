using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTradingMonitor.Presentation.Models
{
    public class NavItem
    {
        public string Title { get; }
        public string Icon { get; }
        public NavItem(string title, string icon)
        {
            Title = title;
            Icon = icon;
        }
    }
}
