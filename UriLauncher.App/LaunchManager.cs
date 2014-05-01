using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UriLauncher.App.Resources;
using Windows.System;

namespace UriLauncher.App
{
    /// <summary>
    /// The launch manager.
    /// </summary>
    public static class LaunchManager
    {
        /// <summary>
        /// Launches an Uri and complains, if the launch was not successful.
        /// </summary>
        /// <param name="uri">The URI to launch.</param>
        /// <returns></returns>
        public static async Task<bool> LaunchUriAsync(Uri uri)
        {
            var result = await Launcher.LaunchUriAsync(uri);

            if (!result)
                MessageBox.Show(AppResources.MessageBoxLaunchFailed, AppResources.MessageBoxInfo, MessageBoxButton.OK);

            return result;
        }
    }
}
