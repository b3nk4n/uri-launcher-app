using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using UriLauncher.App.ViewModels;
using System.Windows.Media;
using UriLauncher.App.Resources;
using System.Windows.Media.Imaging;

namespace UriLauncher.App.Pages
{
    public partial class ItemPage : PhoneApplicationPage
    {
        /// <summary>
        /// The ID parameter.
        /// </summary>
        public const string PARAM_ID = "id";

        public ItemPage()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Builds the localized app bar.
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 0.9f;
            ApplicationBar.BackgroundColor = (Color)App.Current.Resources["PhoneAccentColor"];

            // add
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/save.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarSave;
            appBarButton.Click += (s, e) =>
            {
                Save();
            };
            ApplicationBar.Buttons.Add(appBarButton);
        }

        /// <summary>
        /// Saves the URI item.
        /// </summary>
        private void Save()
        {
            var vm = DataContext as LaunchItemViewModel;
            if (vm != null)
            {
                vm.Title = TextBoxTitle.Text;
                vm.Uri = new Uri(TextBoxUri.Text, UriKind.Absolute);
                App.DataRepository.InsertOrUpdate(vm.Item);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LaunchItemViewModel item; 
            if (NavigationContext.QueryString != null &&
                NavigationContext.QueryString.ContainsKey(PARAM_ID))
            {
                SetHeader("/Assets/AppBar/edit.png", AppResources.EditTitle);
                var id = NavigationContext.QueryString[PARAM_ID];
                item = new LaunchItemViewModel(App.DataRepository.GetById(id));
            }
            else
            {
                SetHeader("/Assets/AppBar/add.png", AppResources.AddTitle);
                item = new LaunchItemViewModel();
            }

            DataContext = item;
        }

        /// <summary>
        /// Sets and updates the header.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <param name="text">The header text.</param>
        private void SetHeader(string imagePath, string text)
        {
            ImageHeader.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            TextBlockHeader.Text = text;
        }
    }
}