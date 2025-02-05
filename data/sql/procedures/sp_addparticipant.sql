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
            BEGIN TRY
                THROW 50000, 'El atleta ya está registrado en este torneo.', 1;
            END TRY
            BEGIN CATCH
                RETURN;
            END CATCH
        END
    END
    ELSE
    BEGIN
        BEGIN TRY
            THROW 50000, 'Datos inválidos para atleta, torneo o categoría de peso.', 1;
        END TRY
        BEGIN CATCH
            RETURN;
        END CATCH
    END
END;

GRANT ALL ON sp_addparticipant TO public;