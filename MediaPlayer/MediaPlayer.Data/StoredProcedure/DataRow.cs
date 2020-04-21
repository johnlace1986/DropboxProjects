using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPlayer.Data.StoredProcedure
{
    public class DataRow
    {
        #region Fields

        /// <summary>
        /// Gets or sets the dictionary that will hold the data in the row
        /// </summary>
        internal Dictionary<string, object> data = new Dictionary<string, object>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value of the column in the row with the specified name
        /// </summary>
        /// <param name="columnName">Name of the column to get the value of</param>
        /// <returns>Value of the column in the row with the specified name</returns>
        public object this[String columnName]
        {
            get
            {
                if (!data.ContainsKey(columnName))
                    throw new KeyNotFoundException();

                return data[columnName];
            }
        }

        #endregion
    }
}
