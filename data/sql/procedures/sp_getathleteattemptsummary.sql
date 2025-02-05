IF OBJECT_ID('sp_getathleteattemptsummary', 'P') IS NOT NULL
    DROP PROCEDURE sp_getathleteattemptsummary;
GO
CREATE PROCEDURE sp_getathleteattemptsummary
    @TournamentId INT = NULL,
    @AthleteId INT = NULL,
    @AthleteDni NVARCHAR(20) = NULL,
    @AthleteName NVARCHAR(100) = NULL,
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        a.first_name + ' ' + a.last_name AS AthleteName,
        a.dni AS AthleteDni,
        t.name AS TournamentName,
        t.id AS TournamentId,
        COUNT(DISTINCT attempt.participation_id + '-' + attempt.attempt_number) AS TotalAttempts
    FROM 
        attempt
    INNER JOIN 
        tournament_participation tp ON attempt.participation_id = tp.id
    INNER JOIN 
        tournament t ON tp.tournament_id = t.id
    INNER JOIN 
        athlete a ON tp.athlete_id = a.id
    WHERE 
        (@TournamentId IS NULL OR t.id = @TournamentId) AND
        (@AthleteId IS NULL OR a.id = @AthleteId) AND
        (@AthleteDni IS NULL OR a.dni = @AthleteDni) AND
        (@AthleteName IS NULL OR (a.first_name + ' ' + a.last_name) LIKE '%' + @AthleteName + '%')
    GROUP BY 
        a.first_name, a.last_name, a.dni, t.name, t.id
    ORDER BY 
        a.first_name, a.last_name, a.dni, t.name
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO
GRANT ALL ON sp_getathleteattemptsummary TO public;