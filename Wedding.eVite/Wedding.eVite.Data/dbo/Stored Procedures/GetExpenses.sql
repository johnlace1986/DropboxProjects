CREATE PROCEDURE [GetExpenses]
AS
BEGIN
	SELECT
		[Id] AS [ExpenseId]
		,[Name]
		,[Cost]
		,[Paid]
	FROM
		[Expense]
END