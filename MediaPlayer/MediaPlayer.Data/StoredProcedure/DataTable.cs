using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

namespace MediaPlayer.Data.StoredProcedure
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
        /// Reads the data from the SQL Data Reader into a DataTable
        /// </summary>
        /// <param name="dr">SQL Data Reader containing the data</param>
        /// <returns>Data table containing the data read from the SQL Data Reader</returns>
        public static DataTable FromSqlDataReader(SqlDataReader dr)
        {
            DataTable dt = new DataTable();

            while (dr.Read())
            {
                DataRow row = new DataRow();

                for (int i = 0; i < dr.FieldCount; i++)
                    row.data.Add(dr.GetName(i), dr[i]);

                dt.rows.Add(row);
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
