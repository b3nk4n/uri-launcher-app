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
using UriLauncher.App.Model;

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
        public ObservableCollection<LaunchItemViewModel> LaunchItems { get; set; }

        /// <summary>
        /// Creates a MainViewModel instance.
        /// </summary>
        public MainViewModel()
        {
            LoadDesignOrRuntimeData();
        }

        /// <summary>
        /// Loads the design or runtime data.
        /// </summary>
        private void LoadDesignOrRuntimeData()
        {
            LaunchItems = new ObservableCollection<LaunchItemViewModel>();

            if (DesignerProperties.IsInDesignTool)
            {
                LaunchItems.Add(new LaunchItemViewModel(new LaunchItem("First Item", new Uri("test:go?param=12345", UriKind.Absolute))));
                LaunchItems.Add(new LaunchItemViewModel(new LaunchItem("Second Item", new Uri("app:do?param=12345", UriKind.Absolute))));
                LaunchItems.Add(new LaunchItemViewModel(new LaunchItem("Third Item", new Uri("whatever:check", UriKind.Absolute))));
            }
        }

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        public void RefreshData()
        {
            LaunchItems.Clear();
            var items = App.DataRepository.GetAll();

            if (items != null)
            {
                foreach (var item in items)
                {
                    LaunchItems.Add(new LaunchItemViewModel(item));
                }
            }
        }
    }
}
