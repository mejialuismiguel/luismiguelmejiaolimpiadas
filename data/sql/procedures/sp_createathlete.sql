IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'sp_createathlete')
BEGIN
    DROP PROCEDURE sp_createathlete;
END
GO

CREATE PROCEDURE sp_createathlete
    @p_dni VARCHAR(20),
    @p_first_name VARCHAR(50),
    @p_last_name VARCHAR(50),
    @p_birth_date DATE,
    @p_gender CHAR(1),  -- 'M' or 'F'
    @p_country_id INT,
    @p_weight_category_id INT
AS
BEGIN

    DECLARE @v_category_gender CHAR(1);
    
    SELECT @v_category_gender = gender
    FROM weight_category
    WHERE id = @p_weight_category_id;


    IF @p_gender NOT IN ('M', 'F')
    BEGIN
        RAISERROR('El género del atleta debe ser M o F', 16, 1)
        RETURN;
    END

    IF (SELECT count(1) from athlete where dni = @p_dni) > 0
    BEGIN
        RAISERROR('El DNI ya existe', 16, 1);
        RETURN;
    END

    IF (SELECT count(1) from country where id = @p_country_id) = 0
    BEGIN
        RAISERROR('Pais inexistente', 16, 1);
        RETURN;
    END

    IF (SELECT count(1) from weight_category where id = @p_weight_category_id) = 0
    BEGIN
        RAISERROR('Categoria inexistente', 16, 1);
        RETURN;
    END

    IF @p_gender != @v_category_gender
    BEGIN
        RAISERROR('El género del atleta no coincide con el género de la categoría de peso', 16, 1)
        RETURN;
    END

    IF @p_birth_date > GETDATE()
    BEGIN
        RAISERROR('La fecha de nacimiento no puede ser posterior a la fecha actual', 16, 1)
        RETURN;
    END

    INSERT INTO athlete (dni, first_name, last_name, birth_date, gender, country_id, weight_category_id)
    VALUES (@p_dni,@p_first_name, @p_last_name, @p_birth_date, @p_gender, @p_country_id, @p_weight_category_id);
END
GO
GRANT ALL ON sp_createathlete TO public;