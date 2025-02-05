IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_gettournaments')
BEGIN
    DROP PROCEDURE sp_gettournaments;
END
GO

CREATE PROCEDURE sp_gettournaments
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
        location,
        start_date,
        end_date
    FROM 
        tournament
    WHERE 
        @Name IS NULL OR name LIKE '%' + @Name + '%'
    ORDER BY 
        id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;

GRANT ALL ON sp_gettournaments TO public;