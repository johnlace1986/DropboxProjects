using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Business;

namespace Utilities.Data.StoredProcedure
{
    public class DataCell : NotifyPropertyChangedObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets the row index of the cell
        /// </summary>
        private Int32 rowIndex;

        /// <summary>
        /// Gets or sets the column index of the cell
        /// </summary>
        private Int32 columnIndex;

        /// <summary>
        /// Gets or sets the column name of the cell
        /// </summary>
        private String columnName;

        /// <summary>
        /// Gets or sets the value of the cell
        /// </summary>
        private object value;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the row index of the cell
        /// </summary>
        public Int32 RowIndex
        {
            get { return rowIndex; }
            private set
            {
                rowIndex = value;
                OnPropertyChanged("RowIndex");
            }
        }

        /// <summary>
        /// Gets or sets the column index of the cell
        /// </summary>
        public Int32 ColumnIndex
        {
            get { return columnIndex; }
            private set
            {
                columnIndex = value;
                OnPropertyChanged("ColumnIndex");
            }
        }

        /// <summary>
        /// Gets or sets the column name of the cell
        /// </summary>
        public String ColumnName
        {
            get { return columnName; }
            private set
            {
                columnName = value;
                OnPropertyChanged("ColumnName");
            }
        }

        /// <summary>
        /// Gets or sets the value of the cell
        /// </summary>
        public object Value
        {
            get { return value; }
            private set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DataCell class
        /// </summary>
        /// <param name="rowIndex">Row index of the cell</param>
        /// <param name="columnIndex">Column index of the cell</param>
        /// <param name="columnName">Column name of the cell</param>
        /// <param name="value">Value of the cell</param>
        internal DataCell(Int32 rowIndex, Int32 columnIndex, String columnName, object value)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            ColumnName = columnName;
            Value = value;
        }

        #endregion
    }
}
