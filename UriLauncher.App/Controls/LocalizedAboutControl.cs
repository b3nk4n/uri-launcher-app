using PhoneKit.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UriLauncher.App.Resources;

namespace UriLauncher.App.Controls
{
    /// <summary>
    /// The localized about page.
    /// </summary>
    public class LocalizedAboutControl : AboutControlBase
    {
        /// <summary>
        /// Localizes the user controls contents and texts.
        /// </summary>
        protected override void LocalizeContent()
        {
            // app
            ApplicationIconSource = new Uri("/Assets/ApplicationIcon.png", UriKind.Relative);
            ApplicationTitle = AppResources.ApplicationTitle;
            ApplicationVersion = AppResources.ApplicationVersion;
            ApplicationAuthor = AppResources.ApplicationAuthor;
            ApplicationDescription = AppResources.ApplicationDescription;

            // buttons
            SupportAndFeedbackText = AppResources.SupportAndFeedback;
            SupportAndFeedbackEmail = "apps@bsautermeister.de";
            PrivacyInfoText = AppResources.PrivacyInfo;
            PrivacyInfoLink = "http://bsautermeister.de/privacy.php";
            RateAndReviewText = AppResources.RateAndReview;
            MoreAppsText = AppResources.MoreApps;
            MoreAppsSearchTerms = "Benjamin Sautermeister";

            // contributors
            ContributorsListVisibility = System.Windows.Visibility.Visible;
            IList<ContributorModel> contributors = new List<ContributorModel>();
            contributors.Add(new ContributorModel("/Assets/Images/icon.png", "John Caserta from The Noun Project"));
            SetContributorsList(contributors);
        }
    }
}
