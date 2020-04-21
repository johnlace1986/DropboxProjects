using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Utilities.Data.StoredProcedure
{
    public class DataTable : IEnumerable<DataRow>
    {
        #region Fields

        /// <summary>
        /// Gets or sets the collection of rows in the table
        /// </summary>
        internal List<DataRow> rows = new List<DataRow>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the row in the table at the specified index
        /// </summary>
        /// <param name="index">Index of the desired row</param>
        /// <returns>Row in the table at the specified index</returns>
        public DataRow this[int index]
        {
            get
            {
                if (index >= rows.Count)
                    throw new ArgumentOutOfRangeException("index");

                return rows[index];
            }
        }

        /// <summary>
        /// Gets the number of rows contained in the DataTable
        /// </summary>
        public int RowCount
        {
            get { return rows.Count; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Reads the data from the data reader into a DataTable
        /// </summary>
        /// <param name="dr">Data reader containing the data</param>
        /// <returns>Data table containing the data read from the data reader</returns>
        public static DataTable FromDataReader(DbDataReader dr)
        {
            DataTable dt = new DataTable();

            Int32 rowIndex = 0;
            while (dr.Read())
            {
                dt.rows.Add(DataRow.FromDataReader(dr, rowIndex));
                rowIndex++;
            }

            return dt;
        }

        #endregion

        #region IEnumberable<DataRow> Members

        public IEnumerator<DataRow> GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        #endregion
    }
}
