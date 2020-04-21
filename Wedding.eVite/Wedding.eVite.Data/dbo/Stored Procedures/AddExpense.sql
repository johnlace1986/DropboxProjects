CREATE PROCEDURE [AddExpense]
(
	@Name AS VARCHAR(100)
	,@Cost AS DECIMAL(18, 2)
	,@Paid AS DECIMAL(18, 2)
)
AS
BEGIN
	INSERT INTO [Expense]
	(
		[Name]
		,[Cost]
		,[Paid]
	)
	VALUES
	(
		@Name
		,@Cost
		,@Paid
	)

	SELECT CONVERT(INT, SCOPE_IDENTITY()) AS [NewExpenseId]
END