IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_getweightcategories')
BEGIN
    DROP PROCEDURE sp_getweightcategories;
END
GO

CREATE PROCEDURE sp_getweightcategories
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        id,
        name,
        min_weight,
        max_weight,
        gender
    FROM 
        weight_category
    ORDER BY 
        id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GRANT ALL ON sp_getweightcategories TO public;