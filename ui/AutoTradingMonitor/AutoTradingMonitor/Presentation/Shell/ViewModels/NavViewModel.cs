using AutoTradingMonitor.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoTradingMonitor.Presentation.ViewModels.Shell
{
    public class NavViewModel
    {
        // ===== 네비게이션 =====
        public ObservableCollection<NavItem> NavItems { get; }
        public NavItem? SelectedNavItem { get; set; }

        public NavViewModel() 
        {
            NavItems = new ObservableCollection<NavItem>
            {
                new NavItem("Home", "🏠"),
                new NavItem("Strategies", "🤖"),
                new NavItem("Orders", "🔁"),
                new NavItem("Positions", "💼"),
                new NavItem("Backtest", "📈"),
                new NavItem("Reports", "📊"),
                new NavItem("System", "🖥"),
            };

            SelectedNavItem = NavItems[0];
        }
    }
}
