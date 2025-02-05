IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_getparticipants')
BEGIN
    DROP PROCEDURE sp_getparticipants;
END
GO

CREATE PROCEDURE sp_getparticipants
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        id,
        athlete_id,
        tournament_id,
        weight_category_id
    FROM 
        tournament_participation
    ORDER BY 
        id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GRANT ALL ON sp_getparticipants TO public;