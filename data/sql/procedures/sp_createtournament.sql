IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_createtournament')
BEGIN
    DROP PROCEDURE sp_createtournament;
END
GO

CREATE PROCEDURE sp_createtournament
    @Name NVARCHAR(100),
    @Location NVARCHAR(100),
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tournament (name, location, start_date, end_date)
    VALUES (@Name, @Location, @StartDate, @EndDate);
END;
GRANT ALL ON sp_createtournament TO public;