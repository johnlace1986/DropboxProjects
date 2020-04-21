using System;
using System.Data;
using System.Data.SqlClient;
using Utilities.Data;
using Utilities.Data.SQL;
using sp = Utilities.Data.StoredProcedure;

namespace Wedding.eVite.Business
{
    [DbObjectMetaData("ExpenseId", DbType.Int32)]
    public class Expense : SqlDbObject<Int32>
    {
        #region Fields
        
        /// <summary>
        /// Gets or sets the name of the expense
        /// </summary>
        private String name;

        /// <summary>
        /// Gets or sets the cost of the expense
        /// </summary>
        private Decimal cost;

        /// <summary>
        /// Gets or sets the amount of the expense that has currently been paid
        /// </summary>
        private Decimal paid;

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets the name of the expense
        /// </summary>
        [sp.DataColumn("Name", DbType.String)]
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the cost of the expense
        /// </summary>
        [sp.DataColumn("Cost", DbType.Decimal)]
        public Decimal Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }

        /// <summary>
        /// Gets or sets the amount of the expense that has currently been paid
        /// </summary>
        [sp.DataColumn("Paid", DbType.Decimal)]
        public Decimal Paid
        {
            get { return paid; }
            set
            {
                paid = value;
                OnPropertyChanged("Paid");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Expense class
        /// </summary>
        public Expense()
            : base()
        {
            Id = -1;
            Name = String.Empty;
            Cost = 0;
            Paid = 0;
        }

        /// <summary>
        /// Initialises a new instance of the Expense class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="expenseId">Unique identifier of the expense</param>
        public Expense(SqlConnection conn, Int32 expenseId)
            : base(conn, expenseId)
        {
        }

        #endregion
        
        #region Static Methods

        /// <summary>
        /// Gets the expenses currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Expenses currently in the system</returns>
        public static Expense[] GetExpenses(SqlConnection conn)
        {
            return Data.Expense.GetExpenses(conn);
        }
        
        #endregion

        #region SqlDbObject Members

        protected override void Load(SqlConnection conn, Int32 id)
        {
            Expense expense = Data.Expense.GetExpenseById(conn, id);

            Id = expense.Id;
            Name = expense.Name;
            Cost = expense.Cost;
            Paid = expense.Paid;
        }
        
        protected override void AddToDatabase(SqlConnection conn)
        {
            AddToDatabase(conn, "AddExpense");
        }

        protected override void UpdateInDatabase(SqlConnection conn)
        {
            UpdateInDatabase(conn, "UpdateExpense");
        }

        protected override void DeleteFromDatabase(SqlConnection conn)
        {
            DeleteFromDatabase(conn, "DeleteExpense");
        }

        protected override void ResetProperties()
        {
            Id = -1;
        }

        #endregion
    }
}
