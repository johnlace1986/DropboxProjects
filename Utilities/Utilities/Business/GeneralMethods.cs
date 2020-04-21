using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.Reflection;
using System.Drawing;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Xml;
using System.Configuration;
using System.Globalization;
using System.Data.Common;
using System.Security.Cryptography;

namespace Utilities.Business
{
    public static class GeneralMethods
    {
        #region Static Methods

        /// <summary>
        /// Ensures a specified path ends with a back-slash
        /// </summary>
        /// <param name="path">Path to append to</param>
        /// <returns>The specified path with a trailing back-slash</returns>
        public static String AppendTrailingSlash(String path)
        {
            if (path != null)
            {
                if (!path.EndsWith("\\"))
                    path += "\\";
            }

            return path;
        }

        /// <summary>
        /// Serialises the passed-in object to an array of bytes.
        /// </summary>
        /// <param name="serialisableObject">The serialisable object to be serialised.</param>
        /// <returns>An array of bytes that represent the serialised object.</returns>
        public static byte[] SerialiseObject(object serialisableObject)
        {
            if (serialisableObject != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, serialisableObject);

                    ms.Seek(0, 0);

                    return ms.ToArray();
                }
            }
            else
                return new byte[0];
        }

        /// <summary>
        /// Deserialises the passed-in array of bytes to an instance of a serialisable object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialise the byte array into.</typeparam>
        /// <param name="value">An array of bytes that represent the serialised object.</param>
        /// <returns>An instance of an object of type T represented by the passed-in byte array.</returns>
        public static T DeSerialiseObject<T>(byte[] value) where T : class, new()
        {
            try
            {
                if (value.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        return (T)bf.Deserialize(ms);
                    }
                }
                else
                    return new T();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Clones the specified object
        /// </summary>
        /// <typeparam name="T">Type of object being cloned</typeparam>
        /// <param name="obj">Object being cloned</param>
        /// <returns>Clone of the object</returns>
        public static T Clone<T>(object obj) where T : class, new()
        {
            //convert to bytes
            byte[] data = GeneralMethods.SerialiseObject(obj);

            //deserialise
            T clone = GeneralMethods.DeSerialiseObject<T>(data);

            return clone;
        }

        /// <summary>
        /// Returns true if the specified nullable boolean value has a value which is true
        /// </summary>
        /// <param name="value">Nullable boolean being determined</param>
        /// <returns>True if the specified nullable boolean value has a value which is true, false if it does not have a value or the value is false</returns>
        public static Boolean GetNullableBoolValue(Boolean? value)
        {
            return ((value.HasValue) && (value.Value));
        }

        /// <summary>
        /// Displays the details of an application error in a message box
        /// </summary>
        /// <param name="message">Details of the application error</param>
        public static void MessageBoxApplicationError(String message)
        {
            MessageBox(message, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays the details of an exception in a message box
        /// </summary>
        /// <param name="e">Exception being displayed</param>
        /// <param name="prefix">Textual prefix to display in the message box</param>
        public static void MessageBoxException(System.Exception e, String prefix)
        {
            String message = String.Empty;

            if (!String.IsNullOrEmpty(prefix))
                message += prefix + Environment.NewLine;

            while (e != null)
            {
                message += e.Message;
                e = e.InnerException;
            }

            MessageBox(message, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays a message box
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <returns>Button that was clicked by the user</returns>
        public static MessageBoxResult MessageBox(String message)
        {
            return MessageBox(message, MessageBoxButton.OK, MessageBoxImage.None);
        }

        /// <summary>
        /// Displays a message box
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="buttons">Buttons to display in the message box</param>
        /// <returns>Button that was clicked by the user</returns>
        public static MessageBoxResult MessageBox(String message, MessageBoxButton buttons)
        {
            return MessageBox(message, buttons, MessageBoxImage.None);
        }

        /// <summary>
        /// Displays a message box
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="buttons">Buttons to display in the message box</param>
        /// <param name="image">Image to display in the message box</param>
        /// <returns>Button that was clicked by the user</returns>
        public static MessageBoxResult MessageBox(String message, MessageBoxButton buttons, MessageBoxImage image)
        {
            return System.Windows.MessageBox.Show(message, "", buttons, image);
        }

        /// <summary>
        /// Executes a lambda expression on the GUI thread ensuring controls can be accessed
        /// </summary>
        /// <param name="dispatcher">Dispatcher for the GUI thread</param>
        /// <param name="ts">Lambda expression to execute</param>
        public static void ExecuteDelegateOnGuiThread(Dispatcher dispatcher, ThreadStart ts)
        {
            dispatcher.Invoke(DispatcherPriority.Normal, ts);
        }

        /// <summary>
        /// Converts a boolean value to a visibility value
        /// </summary>
        /// <param name="value">Boolean value being converted</param>
        /// <returns>Visibile if the specifed value is true, Collapsed if not</returns>
        public static Visibility ConvertBooleanToVisibility(Boolean value)
        {
            return (value ? Visibility.Visible : Visibility.Collapsed);
        }

        /// <summary>
        /// Converts a visibility value to a boolean value
        /// </summary>
        /// <param name="value">Visibility value being converted</param>
        /// <returns>True if the specified value is Visible, false if not</returns>
        public static Boolean ConvertVisibilityToBoolean(Visibility value)
        {
            return value == Visibility.Visible;
        }

        /// <summary>
        /// Allows the value of a dependency property to be set across threads
        /// </summary>
        /// <param name="owner">Dependency object that owns the property that is having it's value changed</param>
        /// <param name="dp">Dependency property having it's value set</param>
        /// <param name="value">New value of the dependency property</param>
        public static void SetDependencyPropertyValue(DependencyObject owner, DependencyProperty dp, object value)
        {
            GeneralMethods.ExecuteDelegateOnGuiThread(owner.Dispatcher, () => owner.SetValue(dp, value));
        }

        /// <summary>
        /// Loads the modules of the specified type
        /// </summary>
        /// <typeparam name="T">Type of module to load</typeparam>
        /// <param name="assembly">Assembly containing the modules to be loaded</param>
        /// <returns>Collection of modules loaded from the specified directory</returns>
        public static T[] LoadModules<T>(Assembly assembly)
        {
            List<T> modules = new List<T>();
            String moduleName = typeof(T).Name;

            try
            {
                foreach (Type type in assembly.GetTypes().Where(p => p.GetInterface(moduleName) != null))
                {
                    modules.Add((T)Activator.CreateInstance(type));
                }
            }
            catch (ReflectionTypeLoadException rtle)
            {
                if (rtle.LoaderExceptions.Length > 0)
                    throw rtle.LoaderExceptions[0];
                else
                    throw rtle;
            }

            return modules.ToArray();
        }

        /// <summary>
        /// Loads the modules of the specified type
        /// </summary>
        /// <typeparam name="T">Type of module to load</typeparam>
        /// <param name="moduleDirectory">Directory of the module files</param>
        /// <returns>Collection of modules loaded from the specified directory</returns>
        public static T[] LoadModules<T>(String moduleDirectory)
        {
            DirectoryInfo di = new DirectoryInfo(moduleDirectory);
            List<T> modules = new List<T>();

            if (di.Exists)
            {
                foreach (FileInfo fi in di.GetFiles("*.dll"))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(fi.FullName);
                        modules.AddRange(LoadModules<T>(assembly));
                    }
                    catch (BadImageFormatException)
                    {
                        //not a .NET dll, do nothing
                    }
                    catch (FileLoadException)
                    {
                        //wrong version of .NET, do nothing
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                }
            }

            modules.Sort();
            return modules.ToArray();
        }

        /// <summary>
        /// Resizes the specified size to it uniformly fits within the maximum bounds
        /// </summary>
        /// <param name="size">Size being resized</param>
        /// <param name="maximum">Maximum size the resized size must fit into</param>
        /// <returns>Resized size that fits uniformly within the maximum bounds</returns>
        public static SizeF ResizeBounds(SizeF size, SizeF maximum)
        {
            return ResizeBounds(size.Width, size.Height, maximum.Width, maximum.Height);
        }

        /// <summary>
        /// Resizes the specified size to it uniformly fits within the maximum bounds
        /// </summary>
        /// <param name="width">Width of the size being sized</param>
        /// <param name="height">Height of the size being resized</param>
        /// <param name="maxWidth">Maximum width the resized size must fit into</param>
        /// <param name="maxHeight">Maximum height the resized size must fit into</param>
        /// <returns>Resized size that fits uniformly within the maximum bounds</returns>
        public static SizeF ResizeBounds(float width, float height, float maxWidth, float maxHeight)
        {
            if (width > maxWidth)
            {
                float factor = maxWidth / width;
                float newHeight = height * factor;

                return ResizeBounds(maxWidth, newHeight, maxWidth, maxHeight);
            }

            if (height > maxHeight)
            {
                float factor = maxHeight / height;
                float newWidth = width * factor;

                return ResizeBounds(newWidth, maxHeight, maxWidth, maxHeight);
            }

            return new SizeF(width, height);
        }

        /// <summary>
        /// Sets the selected item of an items control to the item with the specified data context
        /// </summary>
        /// <param name="container">Item's control containing the item</param>
        /// <param name="item">Data context of the item being selected</param>
        /// <returns>True if the item selected, false if not</returns>
        public static Boolean SetItemsControlSelectedItem(ItemsControl itemsControl, object dataContext)
        {
            if (itemsControl == null || dataContext == null)
                return false;

            TreeViewItem childNode = itemsControl.ItemContainerGenerator.ContainerFromItem(dataContext) as TreeViewItem;

            if (childNode != null)
            {
                return childNode.IsSelected = true;
            }

            if (itemsControl.Items.Count > 0)
            {
                foreach (object childItem in itemsControl.Items)
                {
                    ItemsControl childControl = itemsControl.ItemContainerGenerator.ContainerFromItem(childItem) as ItemsControl;

                    if (SetItemsControlSelectedItem(childControl, dataContext))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Expands a tree view to the node bound to the specified data context
        /// </summary>
        /// <param name="itemsControl">itemControl containing the nodes to expand from</param>
        /// <param name="dataContext">Item being expanded to</param>
        /// <param name="parent">Tree view item containing the item being expanded to</param>
        /// <returns>True if the items control contains an item bound to the specified data context, false if not</returns>
        public static Boolean ExpandTreeViewToItem(ItemsControl itemsControl, object dataContext, out TreeViewItem parent)
        {
            foreach (object item in itemsControl.Items)
            {
                TreeViewItem tvi;

                if (item == dataContext)
                {
                    parent = itemsControl as TreeViewItem;
                    return true;
                }

                tvi = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                if (tvi != null)
                {
                    if (ExpandTreeViewToItem(tvi, dataContext, out parent))
                    {
                        tvi.IsExpanded = true;
                        itemsControl.UpdateLayout();
                        return true;
                    }
                }
            }

            parent = null;
            return false;
        }

        /// <summary>
        /// Removes the milliseconds from the specified date
        /// </summary>
        /// <param name="date">Date being trimmed</param>
        /// <returns>Specified date with milliseconds trimmed</returns>
        public static DateTime RemoveMilliseconds(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        /// <summary>
        /// Gets the container for the specified item
        /// </summary>
        /// <param name="parent">Parent containing the item</param>
        /// <param name="item">Item to search for</param>
        /// <returns>Container for the specified item</returns>
        public static DependencyObject ContainerFromItem(ItemsControl parent, object item)
        {
            foreach (object child in parent.Items)
            {
                ItemsControl container = parent.ItemContainerGenerator.ContainerFromItem(child) as ItemsControl;

                if (child == item)
                    return container;

                DependencyObject childContainer = ContainerFromItem(container, item);

                if (childContainer != null)
                    return childContainer;
            }

            return null;
        }

        /// <summary>
        /// Traverses up the specified tree view to the first parent of specified item that is bound to an object of the specified parent type
        /// </summary>
        /// <param name="treeView">Tree view containing the desired tree view item</param>
        /// <param name="item">Item bound to the child of the desired tree view item</param>
        /// <param name="parentType">Type of data context of the parent tree view item</param>
        /// <returns>First parent of specified item that is bound to an object of the specified parent type</returns>
        public static TreeViewItem GetTreeViewItemParent(TreeView treeView, object item, Type parentType)
        {
            if (item != null)
            {
                TreeViewItem child = ContainerFromItem(treeView, item) as TreeViewItem;

                if (child != null)
                {
                    do
                    {
                        child = VisualTreeHelpers.FindAncestor<TreeViewItem>(child);

                        if (child == null)
                            return null;
                    }
                    while (!(child.DataContext.GetType() == parentType));

                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the app setting with the sepcified key
        /// </summary>
        /// <param name="configFilename">Path to the config file</param>
        /// <param name="key">Key of the desired app settings</param>
        /// <returns>Value of the app setting with the sepcified key</returns>
        public static String GetAppSetting(String configFilename, String key)
        {
            NameValueCollection values = (NameValueCollection)GetConfig("appSettings", configFilename);
            return values[key];
        }

        /// <summary>
        /// Gets the config section from a file
        /// </summary>
        /// <param name="sectionName">Name of the desired config section</param>
        /// <param name="configFileName">Path to the file containing the config section</param>
        /// <returns>Config section from a file</returns>
        private static object GetConfig(string sectionName, string configFileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFileName);

            IConfigurationSectionHandler handler = GetHandler(sectionName, xmlDoc);
            object config = null;

            config = GetAppSettingsFileHandler(sectionName, handler, xmlDoc);

            return config;
        }

        /// <summary>
        /// Gets the handler for a config section
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="xmlDoc">XML document containing the config</param>
        /// <returns>Handler for a config section</returns>
        private static IConfigurationSectionHandler GetHandler(string sectionName, XmlDocument xmlDoc)
        {
            IConfigurationSectionHandler handler = null;

            handler = new NameValueSectionHandler();
            return handler;
        }

        /// <summary>
        /// Gets the handler for a config section
        /// </summary>
        /// <param name="sectionName">Name of the handler</param>
        /// <param name="parentHandler">Handler of the parent section</param>
        /// <param name="xmlDoc">XML document containing the config</param>
        /// <returns>Handler for a config section</returns>
        private static object GetAppSettingsFileHandler(string sectionName, IConfigurationSectionHandler parentHandler, XmlDocument xmlDoc)
        {
            object handler = null;
            XmlNode node = xmlDoc.SelectSingleNode("//" + sectionName);
            XmlAttribute att = (XmlAttribute)node.Attributes.RemoveNamedItem("file");

            if (att == null || att.Value == null || att.Value.Length == 0)
            {
                return parentHandler.Create(null, null, node);
            }
            else
            {
                string fileName = att.Value;
                string dir = Path.GetDirectoryName(fileName);
                string fullName = Path.Combine(dir, fileName);
                XmlDocument xmlDoc2 = new XmlDocument();
                xmlDoc2.Load(fullName);

                object parent = parentHandler.Create(null, null, node);
                IConfigurationSectionHandler h = new NameValueSectionHandler();
                handler = h.Create(parent, null, xmlDoc2.DocumentElement);
            }

            return handler;
        }

        /// <summary>
        /// Gets the last day of the specified month
        /// </summary>
        /// <param name="month">Specified month</param>
        /// <param name="year">Specified year</param>
        /// <returns>Last day of the specified month</returns>
        public static Int32 GetLastDayOfMonth(Int32 month, Int32 year)
        {
            return GetLastDateOfMonth(month, year).Day;
        }

        /// <summary>
        /// Gets the last date of the specified month
        /// </summary>
        /// <param name="month">Specified month</param>
        /// <param name="year">Specified year</param>
        /// <returns>Last date of the specified month</returns>
        public static DateTime GetLastDateOfMonth(Int32 month, Int32 year)
        {
            Int32 lastDayOfMonth = 32;
            String lastDayOfMonthString;
            DateTime lastDateOfMonth;

            do
            {
                lastDayOfMonth--;
                lastDayOfMonthString = lastDayOfMonth.ToString("00") + "/" + month.ToString("00") + "/" + year.ToString("0000");
            }
            while (!DateTime.TryParseExact(lastDayOfMonthString, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out lastDateOfMonth));

            return lastDateOfMonth;
        }

        /// <summary>
        /// Copies the data from the source stream to the destination stream
        /// </summary>
        /// <param name="source">Stream to the data being copied</param>
        /// <param name="destination">Stream to the location the data will be copied to</param>
        public static void CopyFromStream(Stream source, Stream destination)
        {
            CopyFromStream(source, destination, 1024 * 1024); //1MB
        }

        /// <summary>
        /// Copies the data from the source stream to the destination stream
        /// </summary>
        /// <param name="source">Stream to the data being copied</param>
        /// <param name="destination">Stream to the location the data will be copied to</param>
        /// <param name="maxBufferSize">Maximum size of the buffer used to copy the data</param>
        public static void CopyFromStream(Stream source, Stream destination, Int32 maxBufferSize)
        {
            Int64 bytesWritten = 0;

            //copy data in blocks to allow a stream longer than Int32.MaxValue to be copied
            while (bytesWritten < source.Length)
            {
                Int64 bufferSize = source.Length - bytesWritten;

                if (bufferSize > maxBufferSize)
                    bufferSize = maxBufferSize;

                //can safely convert bufferSize to Int32 because it will never be bigger than maxBufferSize
                Int32 shrunkBufferSize = Convert.ToInt32(bufferSize);

                byte[] buffer = new byte[shrunkBufferSize];
                source.Read(buffer, 0, shrunkBufferSize);
                destination.Write(buffer, 0, shrunkBufferSize);

                bytesWritten += bufferSize;
            }
        }

        /// <summary>
        /// Copies the data to the destination stream
        /// </summary>
        /// <param name="data">Data being copied</param>
        /// <param name="destination">Stream to the location the data will be copied to</param>
        public static void CopyToStream(byte[] data, Stream destination)
        {
            CopyToStream(data, destination, 1024 * 1024); //1MB
        }

        /// <summary>
        /// Copies the data to the destination stream
        /// </summary>
        /// <param name="data">Data being copied</param>
        /// <param name="destination">Stream to the location the data will be copied to</param>
        /// <param name="maxBufferSize">Maximum size of the buffer used to copy the data</param>
        public static void CopyToStream(byte[] data, Stream destination, Int32 maxBufferSize)
        {
            Int64 bytesWritten = 0;

            while (bytesWritten < data.LongLength)
            {
                Int64 bufferSize = data.LongLength - bytesWritten;

                if (bufferSize > maxBufferSize)
                    bufferSize = maxBufferSize;

                //can safely convert bufferSize to Int32 because it will never be bigger than maxBufferSize
                Int32 shrunkBufferSize = Convert.ToInt32(bufferSize);
                
                byte[] buffer = new byte[shrunkBufferSize];

                for (Int32 i = 0; i < shrunkBufferSize; i++)
                {
                    buffer[i] = data[bytesWritten + i];
                }

                destination.Write(buffer, 0, shrunkBufferSize);

                bytesWritten += bufferSize;
            }
        }

        /// <summary>
        /// Syncs the items in a collection
        /// </summary>
        /// <typeparam name="T">The type of items being synched</typeparam>
        /// <param name="oldCollection">The old collection of item that needs to be updated</param>
        /// <param name="newCollection">The lastest version of the collection</param>
        /// <param name="comparer">Comparer used to match 2 items from the old and new collections</param>
        /// <param name="addAction">Action to perform when an item needs to be added</param>
        /// <param name="updateAction">Action to perform when an item needs to be updated (the first parameter is the old, out of date item and the second is the new item being updated)</param>
        /// <param name="deleteAction">Action to perform when an item needs to be deleted</param>
        public static void SyncCollections<T>(IEnumerable<T> oldCollection, IEnumerable<T> newCollection, Func<T, T, Boolean> comparer, Action<T> addAction, Action<T, T> updateAction, Action<T> deleteAction)
        {
            List<T> lstOldCollection = new List<T>(oldCollection);
            List<T> lstNewCollection = new List<T>(newCollection);

            Type type = typeof(T);

            while (lstNewCollection.Count > 0)
            {
                T newItem = lstNewCollection[0];
                T oldItem = lstOldCollection.SingleOrDefault(p => comparer(p, newItem));

                Boolean exists;

                if (type.IsValueType)
                {
                    if (type.IsAbstract)
                        throw new ArgumentException("Cannot get default value of abstract class");

                    //create a default object of type T so we can compare if type T is not nullable
                    object nullItem = (T)Activator.CreateInstance(type);

                    //compare oldItem with the nullItem previously created
                    //if they are the same then that means oldItem is null
                    exists = !((object)oldItem).Equals(nullItem);
                }
                else
                {
                    //compare with null
                    exists = (oldItem != null);
                }

                if (exists)
                {
                    updateAction(oldItem, newItem);
                    lstOldCollection.Remove(oldItem);
                }
                else
                    addAction(newItem);

                lstNewCollection.Remove(newItem);
            }

            while (lstOldCollection.Count > 0)
            {
                deleteAction(lstOldCollection[0]);
                lstOldCollection.RemoveAt(0);
            }
        }

        /// <summary>
        /// Hashes the specified value
        /// </summary>
        /// <param name="value">Value being hashed</param>
        /// <returns>Hashed value of the specified string</returns>
        public static String HashString(String value)
        {
            SHA512 sha = new SHA512CryptoServiceProvider();
            sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value));
            byte[] result = sha.Hash;

            StringBuilder builder = new StringBuilder();

            for (Int32 i = 0; i < result.Length; i++)
                builder.Append(result[i].ToString("x2"));

            return builder.ToString();
        }

        /// <summary>
        /// Groups the items into a string
        /// </summary>
        /// <typeparam name="T">Type of items being grouped</typeparam>
        /// <param name="items">Items being grouped</param>
        /// <param name="getLabel">Function to get the label for an item</param>
        /// <param name="separator">Separator used to delimit the items in the group</param>
        /// <returns>String representing the grouped items</returns>
        public static String GroupItems<T>(IEnumerable<T> items, Func<T, String> getLabel, String separator)
        {
            return GroupItems<T>(items, getLabel, separator, separator);
        }

        /// <summary>
        /// Groups the items into a string
        /// </summary>
        /// <typeparam name="T">Type of items being grouped</typeparam>
        /// <param name="items">Items being grouped</param>
        /// <param name="getLabel">Function to get the label for an item</param>
        /// <param name="separator">Separator used to delimit the items in the group</param>
        /// <param name="finalSeparator">Separator used to delimit the final item in the group</param>
        /// <returns>String representing the grouped items</returns>
        public static String GroupItems<T>(IEnumerable<T> items, Func<T, String> getLabel, String separator, String finalSeparator)
        {
            String group = String.Empty;
            Int32 count = items.Count();

            if (count > 0)
            {
                group = getLabel(items.ElementAt(0));

                if (count > 1)
                {
                    Int32 index;

                    for (index = 1; index < count - 1; index++)
                        group += separator + getLabel(items.ElementAt(index));

                    group += finalSeparator + getLabel(items.ElementAt(index));
                }
            }

            return group;
        }

        #endregion
    }
}
