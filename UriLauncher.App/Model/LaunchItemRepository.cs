using PhoneKit.Framework.Core.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UriLauncher.App.Model
{
    /// <summary>
    /// The launch item repository
    /// </summary>
    public class LaunchItemRepository : IRepository<LaunchItem>
    {
        /// <summary>
        /// The file name of the launch items list.
        /// </summary>
        public const string COLLECTION_STORAGE_FILE = "launchList.data";

        /// <summary>
        /// The data container.
        /// </summary>
        Collection<LaunchItem> _dataContainer;

        /// <summary>
        /// Indicates whether the data has been loaded.
        /// </summary>
        private bool _isDataLoaded = false;

        /// <summary>
        /// Creates a LauchItemRepository and loads the data.
        /// </summary>
        public LaunchItemRepository()
        {
            Load();
        }

        public void InsertOrUpdate(LaunchItem data)
        {
            var item = GetById(data.Id);

            if (item == null)
            {
                _dataContainer.Insert(0, data);
            }
            else
            {
                item.Title = data.Title;
                item.Uri = data.Uri;
            }
        }

        public bool Delete(LaunchItem data)
        {
            return _dataContainer.Remove(data);
        }

        private void Load()
        {
            if (!_isDataLoaded)
            {
                _dataContainer = StorageHelper.LoadSerializedFile<Collection<LaunchItem>>(COLLECTION_STORAGE_FILE);

                if (_dataContainer == null)
                    _dataContainer = new Collection<LaunchItem>();
            }
        }

        public bool Save()
        {
            return StorageHelper.SaveAsSerializedFile<Collection<LaunchItem>>(COLLECTION_STORAGE_FILE, _dataContainer);
        }

        public ICollection<LaunchItem> GetAll()
        {
            return _dataContainer;
        }

        public LaunchItem GetById(string id)
        {
            return _dataContainer.FirstOrDefault(e => e.Id == id);
        }
    }
}
