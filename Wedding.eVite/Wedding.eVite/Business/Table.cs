using System;
using System.Data;
using System.Data.SqlClient;
using Utilities.Data;
using Utilities.Data.SQL;
using sp = Utilities.Data.StoredProcedure;

namespace Wedding.eVite.Business
{
    [DbObjectMetaData("TableId", DbType.Int32)]
    public class Table : SqlDbObject<Int32>, IComparable<Table>
    {
        #region Fields
        
        /// <summary>
        /// Gets or sets the guests that have been assigned to the table
        /// </summary>
        private Guest[] guests;

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the guests that have been assigned to the table
        /// </summary>
        public Guest[] Guests
        {
            get { return guests; }
            private set
            {
                guests = value;
                OnPropertyChanged("Guests");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Table class
        /// </summary>
        public Table()
            : base()
        {
            Id = -1;
            Guests = new Guest[0];
        }

        /// <summary>
        /// Initialises a new instance of the Table class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="tableId">Unique identifier of the table</param>
        public Table(SqlConnection conn, Int32 tableId)
            : base(conn, tableId)
        {
        }

        #endregion

        #region Static Methods
        
        /// <summary>
        /// Gets all tables currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All tables currently in the system</returns>
        public static Table[] GetTables(SqlConnection conn)
        {
            return Data.Table.GetTables(conn);
        }

        #endregion

        #region SqlDbObject Members

        protected override void Load(SqlConnection conn, Int32 id)
        {
            Id = id;
            Guests = Guest.GetGuestsByTableId(conn, id);
        }
        
        protected override void Load(SqlConnection conn, sp.DataRow row)
        {
            base.Load(conn, row);
            Guests = Guest.GetGuestsByTableId(conn, Id);
        }

        protected override void AddToDatabase(SqlConnection conn)
        {
            AddToDatabase(conn, "AddTable");
        }

        protected override void UpdateInDatabase(SqlConnection conn)
        {
        }

        protected override void DeleteFromDatabase(SqlConnection conn)
        {
            DeleteFromDatabase(conn, "DeleteTable");
        }

        protected override void ResetProperties()
        {
            Id = -1;
        }

        #endregion

        #region IComparable<Table> Members

        public int CompareTo(Table other)
        {
            return Id.CompareTo(other.Id);
        }

        #endregion
    }
}
