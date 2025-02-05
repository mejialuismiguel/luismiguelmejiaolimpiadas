IF OBJECT_ID('sp_getathletes', 'P') IS NOT NULL
    DROP PROCEDURE sp_getathletes;
GO
CREATE PROCEDURE sp_getathletes
    @PageNumber INT,
    @PageSize INT,
    @Name NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        id,
        dni,
        first_name,
        last_name,
        birth_date,
        gender,
        country_id,
        weight_category_id
    FROM 
        athlete
    WHERE 
        @Name IS NULL OR first_name LIKE '%' + @Name + '%' OR last_name LIKE '%' + @Name + '%'
    ORDER BY 
        id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO
GRANT EXECUTE ON sp_getathletes TO public;