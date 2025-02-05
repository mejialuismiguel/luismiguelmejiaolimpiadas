IF OBJECT_ID('sp_getcompetitionresults', 'P') IS NOT NULL
    DROP PROCEDURE sp_getcompetitionresults;
GO
CREATE PROCEDURE sp_getcompetitionresults
    @TournamentId INT = NULL,
    @TournamentName NVARCHAR(100) = NULL,
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        c.code AS CountryCode,
        a.first_name + ' ' + a.last_name AS AthleteName,
        MAX(CASE WHEN at.type = 'Arranque' THEN at.weight_lifted ELSE 0 END) AS MaxSnatch,
        MAX(CASE WHEN at.type = 'Envion' THEN at.weight_lifted ELSE 0 END) AS MaxCleanAndJerk,
        MAX(CASE WHEN at.type = 'Arranque' THEN at.weight_lifted ELSE 0 END) + MAX(CASE WHEN at.type = 'Envion' THEN at.weight_lifted ELSE 0 END) AS TotalWeight
    FROM 
        attempt at
    INNER JOIN 
        tournament_participation tp ON at.participation_id = tp.id
    INNER JOIN 
        athlete a ON tp.athlete_id = a.id
    INNER JOIN 
        country c ON a.country_id = c.id
    INNER JOIN 
        tournament t ON tp.tournament_id = t.id
    WHERE 
        (@TournamentId IS NULL OR t.id = @TournamentId) AND
        (@TournamentName IS NULL OR t.name LIKE '%' + @TournamentName + '%')
    GROUP BY 
        c.code, a.first_name, a.last_name
    ORDER BY 
        TotalWeight DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO
GRANT ALL ON sp_getcompetitionresults TO public;