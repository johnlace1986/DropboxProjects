using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using Utilities.Business;

namespace MediaPlayer.Library.Business
{
    [Serializable]
    public class FileExtensionCollection : NotifyCollectionChangedObject<IntelligentString>, IEnumerable<IntelligentString>
    {
        #region Fields

        /// <summary>
        /// Gets or sets the list of extensions in the collection
        /// </summary>
        internal List<IntelligentString> extensions = new List<IntelligentString>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the extension at the specified index
        /// </summary>
        /// <param name="index">Index of the desired extension</param>
        /// <returns>Extension at the specified index</returns>
        public IntelligentString this[int index]
        {
            get { return extensions[index]; }
        }

        /// <summary>
        /// Gets the number of extensions in the collection
        /// </summary>
        public int Count
        {
            get { return extensions.Count; }
        }

        /// <summary>
        /// Gets the fiter text for the collection of extensions
        /// </summary>
        public String FilterText
        {
            get
            {
                String filterText = String.Empty;

                foreach (IntelligentString extension in extensions)
                    filterText += "*" + extension.Value + ";";

                if (filterText.EndsWith(";"))
                    filterText = filterText.Substring(0, filterText.Length - 1);

                return filterText;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FileExtensionCollection class
        /// </summary>
        internal FileExtensionCollection()
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds a new extension to the collection
        /// </summary>
        /// <param name="extension">Extension being added to the collection</param>
        public void Add(IntelligentString extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            if (extension.Substring(1).Contains("."))
                throw new ArgumentException("Extension contains multiple full stops");

            if (extensions.Contains(extension))
                throw new ArgumentException("Collection already contains extension: " + extension);

            extensions.Add(extension);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, extension);
        }

        /// <summary>
        /// Removes the specified extension from the collection
        /// </summary>
        /// <param name="extension">Extension being removed</param>
        public void Remove(IntelligentString extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            if (!extensions.Contains(extension))
                throw new ArgumentException("Colleciton does not contain extension: " + extension);

            for (int i = 0; i < extensions.Count; i++)
            {
                if (extensions[i] == extension)
                {
                    RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes the extension from the collection at the specified index
        /// </summary>
        /// <param name="index">Index of the extension being removed</param>
        public void RemoveAt(int index)
        {
            IntelligentString extension = extensions[index];
            extensions.RemoveAt(index);
            OnRemovedFromCollection(extension, index);
        }

        /// <summary>
        /// Removes all extensions from the collection
        /// </summary>
        public void Clear()
        {
            while (extensions.Count != 0)
                RemoveAt(0);
        }

        /// <summary>
        /// Sorts the extensions in the collection
        /// </summary>
        public void Sort()
        {
            extensions.Sort();
            OnCollectionSorted();
        }

        #endregion

        #region IEnumerable<FileType> Members

        public IEnumerator<IntelligentString> GetEnumerator()
        {
            return extensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return extensions.GetEnumerator();
        }

        #endregion
    }
}
