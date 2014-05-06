using Microsoft.Phone.Shell;
using PhoneKit.Framework.Core.Graphics;
using PhoneKit.Framework.Core.MVVM;
using PhoneKit.Framework.Core.Storage;
using PhoneKit.Framework.Core.Tile;
using PhoneKit.Framework.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UriLauncher.App.Controls;
using UriLauncher.App.Model;
using UriLauncher.App.Resources;
using Windows.System;

namespace UriLauncher.App.ViewModels
{
    /// <summary>
    /// A launch item which allows launching of an URI.
    /// </summary>
    public class LaunchItemViewModel : ViewModelBase
    {
        /// <summary>
        /// The launch URI navigation parameter.
        /// </summary>
        public const string PARAM_LAUNCH_URI = "launchUri";

        /// <summary>
        /// The wrapped model.
        /// </summary>
        private LaunchItem _item;

        /// <summary>
        /// The launch command.
        /// </summary>
        private ICommand _launchCommand;

        /// <summary>
        /// The pin to start command.
        /// </summary>
        private ICommand _pinToStart;

        /// <summary>
        /// Creates a LauchItem.
        /// </summary>
        /// <remarks>Required for serialisation.</remarks>
        public LaunchItemViewModel()
            : this(new LaunchItem())
        {
        }

        public LaunchItemViewModel(LaunchItem item)
        {
            Item = item;

            InitializeCommands();
        }

        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitializeCommands()
        {
            _launchCommand = new DelegateCommand(async () =>
            {
                if (IsValid)
                {
                    var res = await LaunchManager.LaunchUriAsync(Uri);
                }
            }, () =>
            {
                return IsValid;
            });

            _pinToStart = new DelegateCommand(() =>
            {
                if (CanPinToStart)
                {
                    PinToStartOrUpdate();
                }
            }, () =>
            {
                return CanPinToStart;
            });
        }

        /// <summary>
        /// Pins a tile to the start screen.
        /// </summary>
        private void PinToStartOrUpdate()
        {
            var gfx = GraphicsHelper.Create(new LaunchNormalTileControl(_item.Title, _item.Uri.OriginalString));
            var uri = StorageHelper.SavePng(string.Format("{0}launch_{1}_{2}.png",
                LiveTileHelper.SHARED_SHELL_CONTENT_PATH,
                _item.Id,
                DateTime.Now.Ticks.ToString()), gfx);

            LiveTilePinningHelper.PinOrUpdateTile(CurrentTileUri, new StandardTileData
            {
                Title = AppResources.ApplicationTitle,
                BackgroundImage = uri,
                BackBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                BackTitle = _item.Title
            });
        }

        /// <summary>
        /// Updates the tile and asks for live tile update.
        /// </summary>
        /// <param name="title">The new title</param>
        /// <param name="uriString">The new uri string.</param>
        public void Update(string title, string uriString)
        {
            var oldUriString = (Uri == null) ? null : string.Copy(Uri.OriginalString);
            bool wasPinned = IsPinned;

            if (!LaunchItem.IsUriValid(uriString))
                return;

            // verify the data has changed
            if (Title == title && uriString == oldUriString)
                return;

            Title = title;
            Uri = new Uri(uriString, UriKind.Absolute);

            // ask for pinned tile update
            if (wasPinned)
            {
                if (oldUriString != null && oldUriString != uriString)
                {
                    // remove the olde tile, because the URI has changed.
                    if (MessageBox.Show(AppResources.MessageBoxAskForTileUpdate, AppResources.MessageBoxInfo, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        LiveTileHelper.RemoveTile(GetTileUri(oldUriString));
                    }
                }
                // repin or update the tile
                PinToStartOrUpdate();
            }
        }

        public LaunchItem Item
        {
            get { return _item; }
            set
            {
                if (_item == null || !_item.Equals(value))
                {
                    _item = value;
                    NotifyPropertyChanged("Title");
                    NotifyPropertyChanged("Uri");
                }
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Id
        {
            get { return _item.Id; }
        }
            

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return _item.Title; }
            set
            {
                if (_item.Title != value)
                {
                    _item.Title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Uri.
        /// </summary>
        public Uri Uri
        {
            get { return _item.Uri; }
            set
            {
                if (_item.Uri != value)
                {
                    _item.Uri = value;
                    NotifyPropertyChanged("Uri");
                }
            }
        }

        /// <summary>
        /// Gets whether the URI is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return LaunchItem.IsUriValid(_item.Uri.OriginalString);
            }
        }

        /// <summary>
        /// Gets the tile Uri.
        /// </summary>
        /// <returns></returns>
        private Uri GetTileUri(string uriString)
        {
            var fullUriString = string.Format("/Pages/MainPage.xaml?{0}={1}",
                    PARAM_LAUNCH_URI,
                    HttpUtility.UrlEncode(uriString));
            return new Uri(fullUriString, UriKind.Relative);
        }

        /// <summary>
        /// Gets the current tile Uri or NULL, when no URI is set.
        /// </summary>
        private Uri CurrentTileUri
        {
            get
            {
                if (Uri == null)
                    return null;
                return GetTileUri(Uri.OriginalString);
            }
        }

        /// <summary>
        /// Indicates whether the tile is pinned to start or not.
        /// </summary>
        public bool CanPinToStart
        {
            get
            {
                return !IsPinned;
            }
        }

        /// <summary>
        /// Gets whether the launch item is pinned or not.
        /// </summary>
        public bool IsPinned
        {
            get
            {
                if (CurrentTileUri == null)
                    return false;
                return LiveTileHelper.TileExists(CurrentTileUri);
            }
        }

        /// <summary>
        /// Gets the launch command.
        /// </summary>
        public ICommand LaunchCommand
        {
            get
            {
                return _launchCommand;
            }
        }

        /// <summary>
        /// Gets the pin to start command.
        /// </summary>
        public ICommand PinToStartCommand
        {
            get
            {
                return _pinToStart;
            }
        }
    }
}
