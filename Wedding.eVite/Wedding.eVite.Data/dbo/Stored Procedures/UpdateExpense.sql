CREATE PROCEDURE [UpdateExpense]
(
	@ExpenseId AS INT
	,@Name AS VARCHAR(100)
	,@Cost AS DECIMAL(18, 2)
	,@Paid AS DECIMAL(18, 2)
)
AS
BEGIN
	UPDATE
		[Expense]
	SET
		[Name] = @Name
		,[Cost] = @Cost
		,[Paid] = @Paid
	WHERE
		[Id] = @ExpenseId
END