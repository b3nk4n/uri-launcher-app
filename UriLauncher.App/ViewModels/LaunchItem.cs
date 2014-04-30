using Microsoft.Phone.Shell;
using PhoneKit.Framework.Core.MVVM;
using PhoneKit.Framework.Core.Tile;
using PhoneKit.Framework.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace UriLauncher.App.ViewModels
{
    /// <summary>
    /// A launch item which allows launching of an URI.
    /// </summary>
    public class LaunchItem : ViewModelBase
    {
        /// <summary>
        /// The default tile title.
        /// </summary>
        public const string DEFAULT_TITLE = "";

        /// <summary>
        /// The launch URI navigation parameter.
        /// </summary>
        public const string PARAM_LAUNCH_URI = "launchUri";

        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The title.
        /// </summary>
        public string _title;

        /// <summary>
        /// The launch uri.
        /// </summary>
        public Uri _uri;

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
        public LaunchItem()
            : this(DEFAULT_TITLE, null)
        {
        }

        public LaunchItem(string title, Uri uri)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Uri = uri;

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
                    await Launcher.LaunchUriAsync(Uri);
                }
            }, () =>
            {
                return IsValid;
            });

            _pinToStart = new DelegateCommand(() =>
            {
                if (CanPinToStart)
                {
                    // TODO images with better tile
                    LiveTilePinningHelper.PinOrUpdateTile(TileUri, new StandardTileData
                    {
                        Title = Title,
                    });
                }
            }, () =>
            {
                return CanPinToStart;
            });
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Uri.
        /// </summary>
        public Uri Uri
        {
            get { return _uri; }
            set
            {
                if (_uri != value)
                {
                    _uri = value;
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
                return true; // TODO implement
            }
        }

        private Uri TileUri
        {
            get
            {
                var uriString = string.Format("/Pages/MainPage.xaml?{0}={1}", 
                    PARAM_LAUNCH_URI,
                    HttpUtility.UrlEncode(Uri.OriginalString));
                return new Uri(uriString, UriKind.Relative);
            }
        }

        /// <summary>
        /// Indicates whether the tile is pinned to start or not.
        /// </summary>
        public bool CanPinToStart
        {
            get
            {
                return LiveTileHelper.TileExists(TileUri);
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
