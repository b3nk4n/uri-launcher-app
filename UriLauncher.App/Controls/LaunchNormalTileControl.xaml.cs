using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace UriLauncher.App.Controls
{
    public partial class LaunchNormalTileControl : UserControl
    {
        public LaunchNormalTileControl(string title, string content)
        {
            InitializeComponent();

            TextBlockTitle.Text = title;
            TextBlockContent.Text = content;
        }
    }
}
