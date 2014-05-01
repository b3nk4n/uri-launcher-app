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
using UriLauncher.App.Model;
using PhoneKit.Framework.Storage;
using PhoneKit.Framework.OS;

namespace UriLauncher.App.Pages
{
    public partial class ItemPage : PhoneApplicationPage
    {
        /// <summary>
        /// The ID parameter.
        /// </summary>
        public const string PARAM_ID = "id";

        /// <summary>
        /// The delete appbar menu item.
        /// </summary>
        private IApplicationBarMenuItem _deleteApplicationBarMenuItem;

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
            ApplicationBar.Opacity = 0.99f;
            ApplicationBar.BackgroundColor = (Color)App.Current.Resources["PhoneAccentColor"];

            // add
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/save.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarSave;
            appBarButton.Click += (s, e) =>
            {
                Save();
            };
            ApplicationBar.Buttons.Add(appBarButton);

            // delete
            _deleteApplicationBarMenuItem = new ApplicationBarMenuItem(AppResources.Delete);
            _deleteApplicationBarMenuItem.IsEnabled = false;
            _deleteApplicationBarMenuItem.Click += (s, e) =>
            {
                var vm = DataContext as LaunchItemViewModel;
                if (vm != null)
                {
                    App.DataRepository.Delete(vm.Item);
                }

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            };
            ApplicationBar.MenuItems.Add(_deleteApplicationBarMenuItem);
        }

        /// <summary>
        /// Saves the URI item.
        /// </summary>
        private void Save()
        {
            var vm = DataContext as LaunchItemViewModel;
            if (vm != null)
            {
                var title = TextBoxTitle.Text;
                var uriString = TextBoxUri.Text;

                if (!LaunchItem.IsUriValid(uriString))
                {
                    MessageBox.Show(AppResources.MessageBoxInvalidUri, AppResources.MessageBoxWarning, MessageBoxButton.OK);
                    return;
                }

                vm.Title = title;
                vm.Uri = new Uri(uriString, UriKind.Absolute);
                App.DataRepository.InsertOrUpdate(vm.Item);

                VibrationHelper.Vibrate(0.1f);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LaunchItemViewModel item; 
            if (NavigationContext.QueryString != null &&
                NavigationContext.QueryString.ContainsKey(PARAM_ID))
            {
                SetMode(true);
                var id = NavigationContext.QueryString[PARAM_ID];
                item = new LaunchItemViewModel(App.DataRepository.GetById(id));
            }
            else
            {
                SetMode(false);
                item = new LaunchItemViewModel();
            }

            LoadState();

            DataContext = item;
        }

        #region State/Tombstoning

        private const string TITLE_STATE_KEY = "titleState";
        private const string URI_STATE_KEY = "uriState";

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SaveState();
        }

        /// <summary>
        /// Loads the app state.
        /// </summary>
        private void LoadState()
        {
            var title = PhoneStateHelper.LoadValue<string>(TITLE_STATE_KEY, null);
            if (title != null)
                TextBoxTitle.Text = title;

            var uri = PhoneStateHelper.LoadValue<string>(URI_STATE_KEY, null);
            if (uri != null)
                TextBoxUri.Text = uri;
        }

        /// <summary>
        /// Saves the app state.
        /// </summary>
        private void SaveState()
        {
            PhoneStateHelper.SaveValue(TITLE_STATE_KEY, TextBoxTitle.Text);
            PhoneStateHelper.SaveValue(URI_STATE_KEY, TextBoxUri.Text);
        }

        #endregion

        /// <summary>
        /// Sets the page mode (edit or add).
        /// </summary>
        /// <param name="isEdit">TRUE for edit mode, FALSE for add mode.</param>
        private void SetMode(bool isEditMode)
        {
            if (isEditMode)
            {
                ImageHeader.Source = new BitmapImage(new Uri("/Assets/AppBar/edit.png", UriKind.Relative));
                TextBlockHeader.Text = AppResources.EditTitle;
                _deleteApplicationBarMenuItem.IsEnabled = true;
            }
            else // add mode
            {
                ImageHeader.Source = new BitmapImage(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
                TextBlockHeader.Text = AppResources.AddTitle;
            }
        }
    }
}