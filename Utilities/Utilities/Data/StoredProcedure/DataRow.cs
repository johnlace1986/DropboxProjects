using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Utilities.Data.StoredProcedure
{
    public class DataRow : IEnumerable<DataCell>
    {
        #region Fields

        /// <summary>
        /// Gets or sets the collection of cells in the row
        /// </summary>
        private List<DataCell> cells = new List<DataCell>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the index in the data table of the row
        /// </summary>
        public Int32 RowIndex { get; private set; }

        /// <summary>
        /// Gets the number of columns in the row
        /// </summary>
        public Int32 ColumnCount
        {
            get { return cells.Count; }
        }

        /// <summary>
        /// Gets the value of the column in the row at the specified index
        /// </summary>
        /// <param name="index">Index of the desired value</param>
        /// <returns>Value of the column in the row at the specified index</returns>
        public object this[Int32 index]
        {
            get { return this[cells[index].ColumnName]; }
        }

        /// <summary>
        /// Gets the value of the column in the row with the specified name
        /// </summary>
        /// <param name="columnName">Name of the column to get the value of</param>
        /// <returns>Value of the column in the row with the specified name</returns>
        public object this[String columnName]
        {
            get 
            {
                DataCell cell = cells.SingleOrDefault(c => c.ColumnName == columnName);

                if (cell == null)
                    throw new KeyNotFoundException(String.Format("Column \"{0}\" was not present in the data row.", columnName));

                return cell.Value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DataRow class
        /// </summary>
        /// <param name="rowIndex">Index in the data table of the row</param>
        private DataRow(Int32 rowIndex)
        {
            RowIndex = rowIndex;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Reads the data from the data reader into a DataRow
        /// </summary>
        /// <param name="dr">Data reader containing the data</param>
        /// <param name="rowIndex">Index in the data table of the row</param>
        /// <returns>Data row containing the data read from the data reader</returns>
        internal static DataRow FromDataReader(DbDataReader dr, Int32 rowIndex)
        {
            DataRow row = new DataRow(rowIndex);
            
            for (int i = 0; i < dr.FieldCount; i++)
            {
                String columnName = dr.GetName(i);
                object value = FormatDatabaseValue(dr[columnName]);
                
                row.cells.Add(new DataCell(rowIndex, i, columnName, value));
            }

            return row;
        }

        /// <summary>
        /// Format a value read from the database
        /// </summary>
        /// <param name="value">Value read from the database</param>
        /// <returns>Value read from the database converts into a readable format</returns>
        private static object FormatDatabaseValue(object value)
        {
            if (value == DBNull.Value)
                return null;

            if (value is DateTime)
                return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);

            return value;
        }

        #endregion

        #region IEnumerable<DataCell> Members

        public IEnumerator<DataCell> GetEnumerator()
        {
            return cells.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return cells.GetEnumerator();
        }

        #endregion
    }
}
