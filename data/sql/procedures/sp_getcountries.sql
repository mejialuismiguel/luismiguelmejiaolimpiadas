IF OBJECT_ID('sp_getcountries', 'P') IS NOT NULL
    DROP PROCEDURE sp_getcountries;
GO
CREATE PROCEDURE sp_getcountries
    @PageNumber INT,
    @PageSize INT,
    @Name NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        id,
        name,
        code
    FROM 
        country
    WHERE 
        @Name IS NULL OR name LIKE '%' + @Name + '%'
    ORDER BY 
        id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GRANT ALL ON sp_getcountries TO public;