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

            // Beispielcode zur Lokalisierung der ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// When the page is navigated to.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // fire startup events
            StartupActionManager.Instance.Fire();
        }

        // Beispielcode zur Erstellung einer lokalisierten ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // ApplicationBar der Seite einer neuen Instanz von ApplicationBar zuweisen
        //    ApplicationBar = new ApplicationBar();

        //    // Eine neue Schaltfläche erstellen und als Text die lokalisierte Zeichenfolge aus AppResources zuweisen.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Ein neues Menüelement mit der lokalisierten Zeichenfolge aus AppResources erstellen
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}