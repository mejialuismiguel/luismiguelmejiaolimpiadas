-- delete from athlete;
-- select * from athlete;

WITH numbers AS (
    SELECT TOP (150) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS number
    FROM sys.all_objects
),
random_data AS (
    SELECT
        -- Generate a unique DNI (simulating with a number for now)
        number + 100000000 AS dni,
        
        -- Random first names from a predefined list
        CASE 
            WHEN RAND(CHECKSUM(NEWID())) < 0.1 THEN 'John'
            WHEN RAND(CHECKSUM(NEWID())) < 0.2 THEN 'Mike'
            WHEN RAND(CHECKSUM(NEWID())) < 0.3 THEN 'David'
            WHEN RAND(CHECKSUM(NEWID())) < 0.4 THEN 'Chris'
            WHEN RAND(CHECKSUM(NEWID())) < 0.5 THEN 'James'
            WHEN RAND(CHECKSUM(NEWID())) < 0.6 THEN 'Robert'
            WHEN RAND(CHECKSUM(NEWID())) < 0.7 THEN 'Daniel'
            WHEN RAND(CHECKSUM(NEWID())) < 0.8 THEN 'Paul'
            WHEN RAND(CHECKSUM(NEWID())) < 0.9 THEN 'Mark'
            ELSE 'Andrew'
        END AS first_name,
        
        -- Random last names from a predefined list
        CASE 
            WHEN RAND(CHECKSUM(NEWID())) < 0.1 THEN 'Smith'
            WHEN RAND(CHECKSUM(NEWID())) < 0.2 THEN 'Johnson'
            WHEN RAND(CHECKSUM(NEWID())) < 0.3 THEN 'Williams'
            WHEN RAND(CHECKSUM(NEWID())) < 0.4 THEN 'Brown'
            WHEN RAND(CHECKSUM(NEWID())) < 0.5 THEN 'Jones'
            WHEN RAND(CHECKSUM(NEWID())) < 0.6 THEN 'Garcia'
            WHEN RAND(CHECKSUM(NEWID())) < 0.7 THEN 'Miller'
            WHEN RAND(CHECKSUM(NEWID())) < 0.8 THEN 'Davis'
            WHEN RAND(CHECKSUM(NEWID())) < 0.9 THEN 'Rodriguez'
            ELSE 'Martinez'
        END AS last_name,
        
        -- Random birth dates between 1980-01-01 and 2000-12-31
        DATEADD(DAY, (RAND(CHECKSUM(NEWID())) * (DATEDIFF(DAY, '1980-01-01', '2000-12-31'))), '1980-01-01') AS birth_date,
        
        -- Random gender ('M' or 'F')
        CASE WHEN RAND(CHECKSUM(NEWID())) < 0.5 THEN 'M' ELSE 'F' END AS gender,
        
        -- Random country_id (1, 2, or 3)
        FLOOR(RAND(CHECKSUM(NEWID())) * 3) + 1 AS country_id,
        
        -- Random Weight category ID (1, 2, or 3)
        FLOOR(RAND(CHECKSUM(NEWID())) * 3) + 1  AS weight_category_id
    FROM 
        numbers
)
INSERT INTO athlete (dni, first_name, last_name, birth_date, gender, country_id, weight_category_id)
SELECT dni, first_name, last_name, birth_date, gender, country_id, weight_category_id
FROM random_data;