IF OBJECT_ID('sp_addattempt', 'P') IS NOT NULL
    DROP PROCEDURE sp_addattempt;
GO
CREATE PROCEDURE sp_addattempt
    @ParticipationId INT,
    @AttemptNumber INT,
    @Type NVARCHAR(10),
    @WeightLifted FLOAT,
    @Success BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @AttemptNumber BETWEEN 1 AND 3 AND
       @Type IN ('Arranque', 'Envion') AND
       (SELECT COUNT(1) FROM attempt WHERE participation_id = @ParticipationId AND type = @Type) < 3
    BEGIN
        INSERT INTO attempt (participation_id, attempt_number, type, weight_lifted, success)
        VALUES (@ParticipationId, @AttemptNumber, @Type, @WeightLifted, @Success);
    END
    ELSE
    BEGIN
        RAISERROR('El Atleta ha excedido el numero máximo de intentos.', 16, 1);
    END
END;
GRANT all ON sp_addattempt TO public;