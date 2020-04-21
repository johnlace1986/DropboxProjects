using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace MediaPlayer.Data
{
    [Serializable]
    public abstract class NotifyCollectionChangedObject<T> : INotifyCollectionChanged
    {
        #region Instance Methods

        /// <summary>
        /// Fires the CollectionChanged event
        /// </summary>
        /// <param name="action">Action that caused the collection to change</param>
        /// <param name="changedItem">The item that changed in the collection</param>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, T changedItem)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }

        /// <summary>
        /// Fires the CollectionChanged event when the collection is sorted
        /// </summary>
        protected void OnCollectionSorted()
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Fires the collection changed event when an item is removed from the collection
        /// </summary>
        /// <param name="removedItem">Item being removed from the collection</param>
        /// <param name="index">Index in the collection of the item being removed</param>
        protected void OnRemovedFromCollection(T removedItem, int index)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
        }

        /// <summary>
        /// Fires the collection changed event when an item is replaced with another item in the collection
        /// </summary>
        /// <param name="newItem">The new item that the old item was replaced with</param>
        /// <param name="oldItem">The item that was replaced</param>
        /// <param name="index">The index in the collection of the item that was replaced</param>
        protected void OnItemReplaced(T newItem, T oldItem, int index)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }

        #endregion

        #region INotifyCollectionChanged Members

        [field:NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }
}
