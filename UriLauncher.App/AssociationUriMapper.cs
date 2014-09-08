using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace UriLauncher.App
{
    class AssociationUriMapper : UriMapperBase
    {
        private string tempUri;

        public override Uri MapUri(Uri uri)
        {
            tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());

            // URI association launch for launcher itself (else it caused an InvalidOperationException: No XAML was found at the location '/Protocol'.)
            if (tempUri.Contains("/Protocol"))
            {
                return new Uri(string.Format("/Pages/MainPage.xaml"), UriKind.Relative);
            }

            // Otherwise perform normal launch.
            return uri;
        }
    }
}
