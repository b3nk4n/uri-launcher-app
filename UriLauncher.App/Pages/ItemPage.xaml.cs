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
        private IApplicationBarIconButton _deleteApplicationBarIconButton;

        /// <summary>
        /// Creates an ItemPage instance.
        /// </summary>
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
            ApplicationBar.ForegroundColor = Colors.White;

            // add
            ApplicationBarIconButton appBarAddButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/save.png", UriKind.Relative));
            appBarAddButton.Text = AppResources.AppBarSave;
            appBarAddButton.Click += (s, e) =>
            {
                Save();
            };
            ApplicationBar.Buttons.Add(appBarAddButton);

            // delete
            _deleteApplicationBarIconButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative));
            _deleteApplicationBarIconButton.IsEnabled = false;
            _deleteApplicationBarIconButton.Text = AppResources.Delete;
            _deleteApplicationBarIconButton.Click += (s, e) =>
            {
                var vm = DataContext as LaunchItemViewModel;
                if (vm != null)
                {
                    App.DataRepository.Delete(vm.Item);
                }

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            };
            ApplicationBar.Buttons.Add(_deleteApplicationBarIconButton);
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

                vm.Update(title, uriString);

                App.DataRepository.InsertOrUpdate(vm.Item);

                VibrationHelper.Vibrate(0.15f);

                // after the item is saved, we are in edit mode.
                SetMode(true);
            }
        }

        /// <summary>
        /// Loads the data context an app state when the page is navigated to.
        /// </summary>
        /// <param name="e">The navigation event args.</param>
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
                _deleteApplicationBarIconButton.IsEnabled = true;
            }
            else // add mode
            {
                ImageHeader.Source = new BitmapImage(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
                TextBlockHeader.Text = AppResources.AddTitle;
                _deleteApplicationBarIconButton.IsEnabled = false;
            }
        }

        #region State/Tombstoning

        private const string TITLE_STATE_KEY = "titleState";
        private const string URI_STATE_KEY = "uriState";

        /// <summary>
        /// Saves the app state when the page is navigated from.
        /// </summary>
        /// <param name="e">The navigation event args.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (!e.IsNavigationInitiator)
            {
                SaveState();
            }
            else
            {
                ClearState();
            }
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

        /// <summary>
        /// Clears the app state.
        /// </summary>
        public void ClearState()
        {
            PhoneStateHelper.SaveValue(TITLE_STATE_KEY, null);
            PhoneStateHelper.SaveValue(URI_STATE_KEY, null);
        }

        #endregion
    }
}