using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using UriLauncher.App.Resources;
using PhoneKit.Framework.Support;
using System.Windows.Media;
using UriLauncher.App.ViewModels;
using Windows.System;
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;

namespace UriLauncher.App.Pages
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();

            // register startup actions
            StartupActionManager.Instance.Register(5, ActionExecutionRule.Equals, () =>
            {
                FeedbackManager.Instance.StartFirst();
            });
            StartupActionManager.Instance.Register(10, ActionExecutionRule.Equals, () =>
            {
                FeedbackManager.Instance.StartSecond();
            });

            DataContext = new MainViewModel();

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// When the page is navigated to.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (await LaunchForward(e))
                return;

            // fire startup events
            StartupActionManager.Instance.Fire();

            // refresh data.
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.RefreshData();
            }
        }

        private async Task<bool> LaunchForward(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString != null && 
                NavigationContext.QueryString.ContainsKey(LaunchItemViewModel.PARAM_LAUNCH_URI))
            {
                // check for BACK from previously launched URI
                if (e.NavigationMode == NavigationMode.Back &&
                    !e.IsNavigationInitiator)
                {
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                    else
                        App.Current.Terminate();
                    return false;
                }

                var uri = NavigationContext.QueryString[LaunchItemViewModel.PARAM_LAUNCH_URI];

                // verify it doesn't try to launch itself, which caused a crash.
                if (uri.StartsWith("launcher:"))
                    return false;

                var result = await LaunchManager.LaunchUriAsync(new Uri(uri, UriKind.Absolute));
                return result;
            }

            return false;
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
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            appBarButton.Text = AppResources.AddTitle;
            appBarButton.Click += (s, e) =>
                {
                    NavigationService.Navigate(new Uri("/Pages/ItemPage.xaml", UriKind.Relative));
                };
            ApplicationBar.Buttons.Add(appBarButton);

            // about
            ApplicationBarMenuItem appBarReferencesMenuItem = new ApplicationBarMenuItem(AppResources.ReferencesTitle);
            appBarReferencesMenuItem.Click += (s, e) =>
            {
                var browser = new WebBrowserTask();
                browser.Uri = new Uri("http://developer.nokia.com/community/wiki/URI_Association_Schemes_List", UriKind.Absolute);
                browser.Show();
            };
            ApplicationBar.MenuItems.Add(appBarReferencesMenuItem);

            // about
            ApplicationBarMenuItem appBarAboutMenuItem = new ApplicationBarMenuItem(AppResources.AboutTitle);
            appBarAboutMenuItem.Click += (s, e) =>
                {
                    NavigationService.Navigate(new Uri("/Pages/AboutPage.xaml", UriKind.Relative));
                };
            ApplicationBar.MenuItems.Add(appBarAboutMenuItem);
        }

        /// <summary>
        /// Is called, when the edit button of a LauchItem in the list was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditItemClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var id = button.Tag;
                var uriString = string.Format("/Pages/ItemPage.xaml?{0}={1}", ItemPage.PARAM_ID, id);
                NavigationService.Navigate(new Uri(uriString, UriKind.Relative));
            }
        }
    }
}