IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_getparticipants')
BEGIN
    DROP PROCEDURE sp_getparticipants;
END
GO

CREATE PROCEDURE sp_getparticipants
    @TournamentId INT = NULL,
    @AthleteName NVARCHAR(100) = NULL,
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        tp.id,
        athlete_id,
        tournament_id,
        tp.weight_category_id
    FROM 
        tournament_participation tp
    INNER JOIN 
        athlete a ON tp.athlete_id = a.id
    INNER JOIN 
        tournament t ON tp.tournament_id = t.id
    INNER JOIN 
        country c ON a.country_id = c.id
    WHERE 
        (@TournamentId IS NULL OR tp.tournament_id = @TournamentId) AND
        (@AthleteName IS NULL OR (a.first_name + ' ' + a.last_name) LIKE '%' + @AthleteName + '%')
    ORDER BY 
        tp.id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GRANT ALL ON sp_getparticipants TO public;