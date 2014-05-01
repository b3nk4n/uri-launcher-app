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

            // URI association launch for contoso.
            /*if (tempUri.Contains("launcher:ShowProducts?CategoryID="))
            {
                // Get the category ID (after "CategoryID=").
                int categoryIdIndex = tempUri.IndexOf("CategoryID=") + 11;
                string categoryId = tempUri.Substring(categoryIdIndex);

                // Map the show products request to ShowProducts.xaml
                return new Uri("/ShowProducts.xaml?CategoryID=" + categoryId, UriKind.Relative);
            }*/

            // Otherwise perform normal launch.
            return uri;
        }
    }
}
