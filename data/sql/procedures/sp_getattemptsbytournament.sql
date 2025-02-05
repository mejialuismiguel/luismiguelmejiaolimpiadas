IF OBJECT_ID('sp_getattemptsbytournament', 'P') IS NOT NULL
    DROP PROCEDURE sp_getattemptsbytournament;
GO
CREATE PROCEDURE sp_getattemptsbytournament
    @TournamentId INT = NULL,
    @TournamentName NVARCHAR(100) = NULL,
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        a.id,
        a.participation_id,
        a.attempt_number,
        a.type,
        a.weight_lifted,
        a.success,
        t.name AS tournament_name,
        t.id AS tournament_id
    FROM 
        attempt a
    INNER JOIN 
        tournament_participation tp ON a.participation_id = tp.id
    INNER JOIN 
        tournament t ON tp.tournament_id = t.id
    WHERE 
        (@TournamentId IS NULL OR t.id = @TournamentId) AND
        (@TournamentName IS NULL OR t.name LIKE '%' + @TournamentName + '%')
    ORDER BY 
        t.id, tp.id, a.id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO
GRANT all ON sp_getattemptsbytournament TO public;