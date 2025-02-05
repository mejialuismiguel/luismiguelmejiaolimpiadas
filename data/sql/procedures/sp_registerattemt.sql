IF EXISTS (SELECT *
FROM sys.objects
WHERE type = 'P' AND name = 'sp_registerattemt')
BEGIN
    DROP PROCEDURE sp_registerattemt;
END
GO

CREATE PROCEDURE sp_registerattemt
    @p_participation_id INT,
    @p_attempt_number INT,
    -- 1, 2 or 3
    @p_type VARCHAR(50),
    -- 'snatch' or 'clean_jerk'
    @p_weight_lifted FLOAT,
    @p_success BIT
AS
BEGIN
    -- Insertar intento
    INSERT INTO attempt
        (participation_id, attempt_number, type, weight_lifted, success)
    VALUES
        (@p_participation_id, @p_attempt_number, @p_type, @p_weight_lifted, @p_success);
END
GO
GRANT ALL ON sp_registerattemt TO public;