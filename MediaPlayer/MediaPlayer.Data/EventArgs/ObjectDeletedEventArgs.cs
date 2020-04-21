using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPlayer.Data.EventArgs
{
    public delegate void ObjectDeletedEventHanlder(object sender, ObjectDeletedEventArgs e);

    public class ObjectDeletedEventArgs : System.EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the object that was deleted
        /// </summary>
        public SqlDbObject DeletedObject { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ObjectDeletedEventArgs class
        /// </summary>
        /// <param name="deletedObject">The object that was deleted</param>
        public ObjectDeletedEventArgs(SqlDbObject deletedObject)
        {
            DeletedObject = deletedObject;
        }

        #endregion
    }
}
