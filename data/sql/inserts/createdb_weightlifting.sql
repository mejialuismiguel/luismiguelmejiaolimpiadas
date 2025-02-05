IF EXISTS (SELECT name FROM sys.databases WHERE name = 'weightlifting')
BEGIN
    DROP DATABASE [weightlifting];
END
GO

CREATE DATABASE [weightlifting];
GO

USE [weightlifting];
GO

CREATE TABLE [country] (
  [id] int PRIMARY KEY,
  [name] varchar(100),
  [code] char(3) UNIQUE
)
GO

CREATE TABLE [weight_category] (
  [id] int PRIMARY KEY,
  [name] varchar(50),
  [min_weight] float NOT NULL,
  [max_weight] float NOT NULL,
  [gender] char(1) NOT NULL
)
GO

CREATE TABLE [athlete] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [dni] varchar(20),
  [first_name] varchar(50),
  [last_name] varchar(50),
  [birth_date] date,
  [gender] char(1) NOT NULL,
  [country_id] int,
  [weight_category_id] int
)
GO

CREATE TABLE [tournament] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [name] varchar(50),
  [location] varchar(100),
  [start_date] date NOT NULL,
  [end_date] date
)
GO

CREATE TABLE [tournament_participation] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [athlete_id] int,
  [tournament_id] int,
  [weight_category_id] int
)
GO

CREATE TABLE [attempt] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [participation_id] int,
  [attempt_number] int,
  [type] varchar(10),
  [weight_lifted] float,
  [success] bit NOT NULL
)
GO

CREATE UNIQUE INDEX [athlete_index_0] ON [athlete] ("dni")
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'CHECK (min_weight > 0)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'weight_category',
@level2type = N'Column', @level2name = 'min_weight';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'CHECK (min_weight > 0)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'weight_category',
@level2type = N'Column', @level2name = 'max_weight';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'CHECK (gender IN (''M'', ''F''))',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'weight_category',
@level2type = N'Column', @level2name = 'gender';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'CHECK (gender IN (''M'', ''F''))',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'athlete',
@level2type = N'Column', @level2name = 'gender';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'CHECK (start_date < end_date)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'tournament',
@level2type = N'Column', @level2name = 'start_date';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'CHECK (success IN (0, 1))',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'attempt',
@level2type = N'Column', @level2name = 'success';
GO

ALTER TABLE [athlete] ADD FOREIGN KEY ([country_id]) REFERENCES [country] ([id])
GO

ALTER TABLE [athlete] ADD FOREIGN KEY ([weight_category_id]) REFERENCES [weight_category] ([id])
GO

ALTER TABLE [tournament_participation] ADD FOREIGN KEY ([athlete_id]) REFERENCES [athlete] ([id])
GO

ALTER TABLE [tournament_participation] ADD FOREIGN KEY ([tournament_id]) REFERENCES [tournament] ([id])
GO

ALTER TABLE [tournament_participation] ADD FOREIGN KEY ([weight_category_id]) REFERENCES [weight_category] ([id])
GO

ALTER TABLE [attempt] ADD FOREIGN KEY ([participation_id]) REFERENCES [tournament_participation] ([id])
GO
