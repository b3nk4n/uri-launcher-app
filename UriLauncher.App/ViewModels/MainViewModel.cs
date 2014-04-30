using PhoneKit.Framework.Core.MVVM;
using PhoneKit.Framework.Core.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UriLauncher.App.ViewModels
{
    /// <summary>
    /// The main page view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The file name of the launch items list.
        /// </summary>
        public const string COLLECTION_STORAGE_FILE = "launchList.data";

        /// <summary>
        /// The list of lauch items.
        /// </summary>
        public ObservableCollection<LaunchItem> LaunchItems { get; set; }

        /// <summary>
        /// Creates a MainViewModel instance.
        /// </summary>
        public MainViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                LaunchItems = new ObservableCollection<LaunchItem>();
                LaunchItems.Add(new LaunchItem("First Item", new Uri("test:go?param=12345", UriKind.Absolute)));
                LaunchItems.Add(new LaunchItem("Second Item", new Uri("app:do?param=12345", UriKind.Absolute)));
                LaunchItems.Add(new LaunchItem("Third Item", new Uri("whatever:check", UriKind.Absolute)));
            }
            else
            {
                Load();
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void Load()
        {
            var loadedItems = StorageHelper.LoadSerializedFile<Collection<LaunchItem>>(COLLECTION_STORAGE_FILE);
            LaunchItems = new ObservableCollection<LaunchItem>(loadedItems);
        }

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <returns>Returns TRUE when the save process was successful, else FALSE in case of error.</returns>
        public bool Save()
        {
            return StorageHelper.SaveAsSerializedFile<Collection<LaunchItem>>(COLLECTION_STORAGE_FILE, LaunchItems);
        }
    }
}
