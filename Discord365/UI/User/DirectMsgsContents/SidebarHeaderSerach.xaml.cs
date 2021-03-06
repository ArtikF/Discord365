﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discord365.UI.User.DirectMsgsContents
{
    /// <summary>
    /// Interaction logic for SidebarHeaderSerach.xaml
    /// </summary>
    public partial class SidebarHeaderSerach : UserControl
    {
        public SidebarHeaderSerach()
        {
            InitializeComponent();
        }

        private void TbSearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = new Screens.ScreenSearch();
            App.MainWnd.ContentBasic.Set(new Screens.ScreenSearchHeader(), s);
            s.tbSearchBox.Text = tbSearchText.Text;
            App.MainWnd.Sidebar.Set(null, null);
        }
    }
}
