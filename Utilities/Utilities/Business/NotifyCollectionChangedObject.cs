using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace Utilities.Business
{
    [Serializable]
    public abstract class NotifyCollectionChangedObject<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        #region Fields

        /// <summary>
        /// Gets or sets the items in the collection
        /// </summary>
        private List<T> items = new List<T>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of items contained in the collection
        /// </summary>
        public Int32 Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets or sets the item at the specified index
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <returns>Value of the item at the specified index</returns>
        public T this[Int32 index]
        {
            get { return items[index];}
            set
            {
                var oldItem = items[index];
                items[index] = value;

                OnItemReplaced(value, oldItem, index);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the NotifyPropertyChangedObject class
        /// </summary>
        protected NotifyCollectionChangedObject()
        {
        }

        /// <summary>
        /// Initialises a new instance of the NotifyPropertyChangedObject class
        /// </summary>
        /// <param name="items">Items to add to the collection</param>
        protected NotifyCollectionChangedObject(IEnumerable<T> items)
            : this()
        {
            this.items = new List<T>(items);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the CollectionChanged event
        /// </summary>
        /// <param name="action">Action that caused the collection to change</param>
        /// <param name="changedItems">The items that changed in the collection</param>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, IEnumerable<T> changedItems)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItems));
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
        /// <param name="removedItems">Items being removed from the collection</param>
        protected void OnRemovedFromCollection(IEnumerable<T> removedItems)
        {
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItems);
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

        /// <summary>
        /// Adds the item to the collection
        /// </summary>
        /// <param name="item">Item being added to the collection</param>
        public void Add(T item)
        {
            AddRange(new T[] { item });
        }

        /// <summary>
        /// Adds the items to the collection
        /// </summary>
        /// <param name="items">Items being added to the collection</param>
        public void AddRange(IEnumerable<T> items)
        {
            this.items.AddRange(items);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, items);
        }

        /// <summary>
        /// Sorts the items in the collection
        /// </summary>
        protected void Sort()
        {   
            items.Sort();
            OnCollectionSorted();
        }

        /// <summary>
        /// Removes the item at the specified index from the collection
        /// </summary>
        /// <param name="index">Index in the collection of the item being removed</param>
        public void RemoveAt(Int32 index)
        {
            Remove(items[index]);
        }

        /// <summary>
        /// Removes the specified item from the collection
        /// </summary>
        /// <param name="item">Item being removed</param>
        public void Remove(T item)
        {
            RemoveRange(new T[] { item });
        }

        /// <summary>
        /// Removes the items that match the specified predicate from the collection
        /// </summary>
        /// <param name="match">Delegate that defines the conditions of the items to remove</param>
        public void RemoveAll(Predicate<T> match)
        {
            RemoveRange(items.FindAll(match));
        }

        /// <summary>
        /// Removes the specified items from the collection
        /// </summary>
        /// <param name="items">Items being removed</param>
        public void RemoveRange(IEnumerable<T> items)
        {
            this.items.RemoveAll(p => items.Contains(p));
            OnRemovedFromCollection(items);
        }

        #endregion

        #region INotifyCollectionChanged Members

        [field:NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion
    }
}
