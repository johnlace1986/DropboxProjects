CREATE PROCEDURE [dbo].[GetExpenseById]
(
	@ExpenseId AS INT
)
AS
BEGIN
	SELECT
		[Id] AS [ExpenseId]
		,[Name]
		,[Cost]
		,[Paid]
	FROM
		[Expense]
	WHERE
		[Id] = @ExpenseId
END