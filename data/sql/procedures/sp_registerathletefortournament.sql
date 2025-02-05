IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_registerathletefortournament')
BEGIN
    DROP PROCEDURE sp_registerathletefortournament;
END
GO

CREATE PROCEDURE sp_registerathletefortournament
    @p_athlete_id INT,
    @p_tournament_id INT,
    @p_weight_category_id INT
AS
BEGIN
    -- Verificar que el atleta pertenece a la categoría de peso de acuerdo al género
    DECLARE @v_category_gender CHAR(1);
    DECLARE @v_athlete_gender CHAR(1);
    
    -- Obtener género de la categoría de peso
    SELECT @v_category_gender = gender
    FROM weight_category
    WHERE id = @p_weight_category_id;

    -- Obtener género del atleta
    SELECT @v_athlete_gender = gender
    FROM athlete
    WHERE id = @p_athlete_id;

    -- Validar que el género coincida
    IF @v_category_gender != @v_athlete_gender
    BEGIN
        RAISERROR('El género del atleta no coincide con el género de la categoría de peso', 16, 1);
    END

    -- Insertar participación
    INSERT INTO tournament_participation (athlete_id, tournament_id, weight_category_id)
    VALUES (@p_athlete_id, @p_tournament_id, @p_weight_category_id);
END
GO
GRANT ALL ON sp_registerathletefortournament TO public;