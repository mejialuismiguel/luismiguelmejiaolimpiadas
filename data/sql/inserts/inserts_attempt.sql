-- Generar intentos para cada participation_id
DECLARE @ParticipationId INT;
DECLARE @AttemptNumber INT;
DECLARE @Type NVARCHAR(10);
DECLARE @WeightLifted FLOAT;
DECLARE @Success BIT;

-- Cursor para iterar sobre los participation_id
DECLARE ParticipationCursor CURSOR FOR
SELECT id FROM tournament_participation;

OPEN ParticipationCursor;
FETCH NEXT FROM ParticipationCursor INTO @ParticipationId;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Generar intentos para 'Arranque'
    SET @Type = 'Arranque';

    SET @AttemptNumber = 1;
    WHILE @AttemptNumber <= 3
    BEGIN
        SET @WeightLifted = FLOOR(RAND() * (180 - 100 + 1)) + 100;
        SET @Success = CAST(FLOOR(RAND() * 2) AS BIT); -- Generar 0 o 1 aleatoriamente
        INSERT INTO attempt (participation_id, attempt_number, type, weight_lifted, success)
        VALUES (@ParticipationId, @AttemptNumber, @Type, @WeightLifted, @Success);
        SET @AttemptNumber = @AttemptNumber + 1;
    END

    -- Generar intentos para 'Envion'
    SET @Type = 'Envion';

    SET @AttemptNumber = 1;
    WHILE @AttemptNumber <= 3
    BEGIN
        SET @WeightLifted = FLOOR(RAND() * (180 - 100 + 1)) + 100;
        SET @Success = CAST(FLOOR(RAND() * 2) AS BIT); -- Generar 0 o 1 aleatoriamente
        INSERT INTO attempt (participation_id, attempt_number, type, weight_lifted, success)
        VALUES (@ParticipationId, @AttemptNumber, @Type, @WeightLifted, @Success);
        SET @AttemptNumber = @AttemptNumber + 1;
    END

    -- Si 'Envion' tiene éxito, actualizar 'Arranque' a éxito
    IF EXISTS (SELECT 1 FROM attempt WHERE participation_id = @ParticipationId AND type = 'Envion' AND success = 1)
    BEGIN
        UPDATE attempt
        SET success = 1
        WHERE participation_id = @ParticipationId AND type = 'Arranque';
    END

    FETCH NEXT FROM ParticipationCursor INTO @ParticipationId;
END

CLOSE ParticipationCursor;
DEALLOCATE ParticipationCursor;