CREATE PROCEDURE [dbo].[DeleteExpense]
(
	@ExpenseId AS INT
)
AS
BEGIN
	DELETE FROM
		[Expense]
	WHERE
		[Id] = @ExpenseId
END