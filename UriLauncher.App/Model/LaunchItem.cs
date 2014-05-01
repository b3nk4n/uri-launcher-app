using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UriLauncher.App.Model
{
    public class LaunchItem
    {
        /// <summary>
        /// The default tile title.
        /// </summary>
        public const string DEFAULT_TITLE = "";

        /// <summary>
        /// The unique ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The launch URI.
        /// </summary>
        public Uri Uri { get; set; }

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
        }

        public override string ToString()
        {
            return string.Format("[id: {0}, title: {1}, uri:{2}]", Id, Title, Uri);
        }

        public override bool Equals(object obj)
        {
            LaunchItem other = obj as LaunchItem;

            if (other == null)
                return false;

            return this.Id == other.Id
                && other.Title == this.Title
                && other.Uri == this.Uri;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Title.GetHashCode() ^ Uri.ToString().GetHashCode();
        }
    }
}
