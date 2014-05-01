using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UriLauncher.App.Model
{
    /// <summary>
    /// I simple repository interface.
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>
        /// Inserts or updates the data to the repository.
        /// </summary>
        /// <param name="data">The data to add or update.</param>
        void InsertOrUpdate(T data);

        /// <summary>
        /// Deletes the data item from the repository.
        /// </summary>
        /// <param name="data">The data to delete.</param>
        /// <returns>Return TRUE for success, else FALSE.</returns>
        bool Delete(T data);

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <returns>Returns TRUE for success, else FALSE.</returns>
        bool Save();

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns>Returns the loaded collection or an empty list.</returns>
        ICollection<T> GetAll();
        
        /// <summary>
        /// Gets an item by the given ID.
        /// </summary>
        /// <param name="id">The id of the item.</param>
        /// <returns>Returns the item or NULL.</returns>
        T GetById(string id);
    }
}
