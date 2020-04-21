using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Business
{
    [Serializable]
    public class SortableNotifyCollectionChangedObject<T> : NotifyCollectionChangedObject<T> where T : IComparable
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SortableNotifyCollectionChangedObject class
        /// </summary>
        protected SortableNotifyCollectionChangedObject()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the SortableNotifyCollectionChangedObject class
        /// </summary>
        /// <param name="items">Items to add to the collection</param>
        protected SortableNotifyCollectionChangedObject(IEnumerable<T> items)
            : base(items)
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Sorts the items in the collection
        /// </summary>
        public new void Sort()
        {
            base.Sort();
        }

        #endregion
    }
}
