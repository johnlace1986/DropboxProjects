using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPlayer.Data.EventArgs
{
    public delegate void ObjectSavedEventHandler(object sender, ObjectSavedEventArgs e);

    public class ObjectSavedEventArgs : System.EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the object that was saved
        /// </summary>
        public SqlDbObject SavedObject { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ObjectSavedEventArgs class
        /// </summary>
        /// <param name="savedObject">The object that was saved</param>
        public ObjectSavedEventArgs(SqlDbObject savedObject)
        {
            SavedObject = savedObject;
        }

        #endregion
    }
}
