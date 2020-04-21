CREATE PROCEDURE [dbo].[GenerateIdNumber]
(
	@TableName AS VARCHAR(50),
	@IdColumnName AS VARCHAR(50),
	@WhereClause AS VARCHAR(50) = '1 = 1',
	@NewId AS BIGINT OUT
)
AS
BEGIN
	CREATE TABLE #NewIdNumbers ([NewId] BIGINT)
	CREATE TABLE #NewIdNumber ([NewId] BIGINT)
		
	DECLARE @sql VARCHAR(2000)
	
	SET @sql = 'IF EXISTS (SELECT 1 FROM ' + @TableName + ' WHERE ' + @IdColumnName + ' = 1 AND (' + @WhereClause + '))	BEGIN
		INSERT INTO
			#NewIdNumbers
		SELECT
			CONVERT(BIGINT, Id + 1)
		FROM
			' + @TableName + '
		WHERE (' + @WhereClause + ')
	END
	
	ELSE
	BEGIN
		INSERT INTO #NewIdNumbers
		(
			[NewId]
		)
		VALUES
		(
			CONVERT(BIGINT, 1)
		)
	END
	
	INSERT INTO
		#NewIdNumber
	SELECT TOP 1
		[NewId]
	FROM
		#NewIdNumbers
	WHERE NOT [NewId] IN
	(
		SELECT
			' + @IdColumnName + '
		FROM
			' + @TableName + '
		WHERE (' + @WhereClause + ')
	)
	ORDER BY
		[NewId]'
		
	EXEC (@sql)

	DROP TABLE
		#NewIdNumbers
	
	SELECT
		@NewId = [NewId]
	FROM
		#NewIdNumber
	
	DROP TABLE #NewIdNumber
END