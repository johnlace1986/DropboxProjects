using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using sp = Utilities.Data.StoredProcedure;
using Utilities.EventArgs;

namespace Utilities.Data
{
    public abstract class DbHierarchicalObject<T> : DbObject<T>
    {
        #region Fields

        /// <summary>
        /// Gets or sets the parent object
        /// </summary>
        protected DbHierarchicalObject<T> parent;

        /// <summary>
        /// Gets or sets the child object
        /// </summary>
        protected DbHierarchicalObject<T> child;

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the all generations of children
        /// </summary>
        public DbHierarchicalObject<T>[] ChildrenFlattened
        {
            get
            {
                List<DbHierarchicalObject<T>> children = new List<DbHierarchicalObject<T>>();
                DbHierarchicalObject<T> child = this.child;

                while (child != null)
                {
                    children.Add(child);
                    child = child.child;
                }

                return children.ToArray();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DbHierarchicalObject class
        /// </summary>
        protected DbHierarchicalObject()
            : this(null)
        {
            if (GetType().IsDefined(typeof(DbHierarchicalObjectMetaDataAttribute), false) == false)
                throw new InvalidOperationException("DbHierarchicalObjects must have a DbHierarchicalObjectMetaDataAttribute attribute");

        }

        /// <summary>
        /// Initialises a new instance of the DbHierarchicalObject class
        /// </summary>
        /// <param name="parent">Parent object</param>
        protected DbHierarchicalObject(DbHierarchicalObject<T> parent)
            : base()
        {
            this.parent = parent;
            this.child = null;

            Saved += DbHierarchicalObject_Saved;
            Deleted += DbHierarchicalObject_Deleted;
        }

        /// <summary>
        /// Initialises a new instance of the DbHierarchicalObject class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="id">Unique identifier of the object</param>
        protected DbHierarchicalObject(DbConnection conn, T id)
            :base(conn, id)
        {

        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Gets the child object that belongs to the specified parent
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="parent">Parent of the desired child</param>
        /// <returns>Child object that belongs to the specified parent</returns>
        internal abstract DbHierarchicalObject<T> GetChildByParent(DbConnection conn, DbHierarchicalObject<T> parent);

        #endregion

        #region Event Handlers

        private void DbHierarchicalObject_Saved(object sender, DbObjectSavedEventArgs<T> e)
        {
            if (this.child != null)
                this.child.Save(e.Connection);
        }

        private void DbHierarchicalObject_Deleted(object sender, DbObjectDeletedEventArgs<T> e)
        {
            DbHierarchicalObject<T> currentChild = this.child;

            while (currentChild != null)
            {
                currentChild.IsInDatabase = false;
                currentChild.ResetProperties();
                currentChild = currentChild.child;
            }
        }

        #endregion

        #region DbObject<T> Members

        internal override sp.ParameterCollection GetParametersForStoredProcedure(Boolean includeId)
        {
            sp.ParameterCollection parameters = base.GetParametersForStoredProcedure(includeId);

            //get the ID of the parent
            object parentId = this.parent == null ? null : (object)this.parent.Id;

            //use the meta data attribute to create a parameter for the parent ID
            DbHierarchicalObjectMetaDataAttribute metaData = GetType().GetCustomAttribute<DbHierarchicalObjectMetaDataAttribute>();
            parameters.AddWithValue(metaData.ParentIdParameterName, metaData.IdParameterDataType, parentId);

            return parameters;
        }

        internal override void LoadPropertiesFromDataRow(DbConnection conn, sp.DataRow row)
        {
            base.LoadPropertiesFromDataRow(conn, row);
            this.child = GetChildByParent(conn, this);
        }
        
        internal override void Load(DbConnection conn, T id)
        {

        }

        #endregion
    }
}
