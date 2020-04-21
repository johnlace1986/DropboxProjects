CREATE PROCEDURE [DeleteTable]
(
	@TableId AS INT
)
AS
BEGIN
	DELETE FROM
		[Table]
	WHERE
		[Id] = @TableId
END