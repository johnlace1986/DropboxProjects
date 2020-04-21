using System;
using System.Data;
using System.Data.SqlClient;
using Utilities.Exception;
using sql = Utilities.Data.SQL;

namespace Wedding.eVite.Data
{
    internal static class Expense
    {
        #region Static Methods

        /// <summary>
        /// Gets the expenses currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Expenses currently in the system</returns>
        public static Business.Expense[] GetExpenses(SqlConnection conn)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetExpenses", conn);

                return cmd.ExecuteMappedReader<Business.Expense, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get expenses", e);
            }
        }

        /// <summary>
        /// Gets the expense with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="expenseId">Unique identifier of the expense</param>
        /// <returns>Expense with the specified unique identifier</returns>
        public static Business.Expense GetExpenseById(SqlConnection conn, Int32 expenseId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetExpenseById", conn);
                cmd.Parameters.AddWithValue("ExpenseId", DbType.Int32, expenseId);

                return cmd.ExecuteMappedSingleReader<Business.Expense, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get expense", e);
            }
        }

        #endregion
    }
}
