IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_addparticipant')
BEGIN
    DROP PROCEDURE sp_addparticipant;
END
GO

CREATE PROCEDURE sp_addparticipant
    @AthleteId INT,
    @TournamentId INT,
    @WeightCategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM athlete WHERE id = @AthleteId) AND
       EXISTS (SELECT 1 FROM tournament WHERE id = @TournamentId) AND
       EXISTS (SELECT 1 FROM weight_category WHERE id = @WeightCategoryId)
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM tournament_participation WHERE athlete_id = @AthleteId AND tournament_id = @TournamentId)
        BEGIN
            INSERT INTO tournament_participation (athlete_id, tournament_id, weight_category_id)
            VALUES (@AthleteId, @TournamentId, @WeightCategoryId);
        END
        ELSE
        BEGIN
            RAISERROR('El atleta ya está registrado en este torneo.', 16, 1)
            RETURN;
        END
    END
    ELSE
    BEGIN
        RAISERROR('Datos inválidos para atleta, torneo o categoría de peso.', 16, 1)
        RETURN;
    END
END;

GRANT ALL ON sp_addparticipant TO public;