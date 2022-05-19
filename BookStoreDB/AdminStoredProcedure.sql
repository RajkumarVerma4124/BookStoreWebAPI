-----------------------------------------Welcome To The Book STore DataBase---------------------------------------

----------------------------------------------------Create DB----------------------------------------------
use BookStoreDB;

------------------------------------------------------------------------------------------------------------------------
--************************************************ Creating Admin Table ***********************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE TABLE BookAdmin (
	AdminId INT IDENTITY(1,1) PRIMARY KEY,
	FullName VARCHAR(100) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Password VARCHAR(255) NOT NULL,
	MobileNumber VARCHAR(50) NOT NULL,
	Address VARCHAR(255) NOT NULL
);

INSERT INTO BookAdmin VALUES ('Admin','admin@bookstore.com', 'Admin@12345', '+91 8163475881', '42, 14th Main, 15th Cross, Sector 4 ,opp to BDA complex, near Kumarakom restaurant, HSR Layout, Bangalore 560034');
UPDATE BookAdmin SET FullName = 'Admin Raj' Where AdminId = 1;
SELECT * FROM BookAdmin;

------------------------------------------------------------------------------------------------------------------------
--************************************** Creating Admin Login Stored Procedure ****************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spLoginAdmin
	@Email varchar(255),
	@Password varchar(255)
AS
BEGIN TRY
	IF(EXISTS(SELECT * from BookAdmin WHERE Email = @Email and Password = @Password))
	BEGIN
		SELECT * from BookAdmin;
	END
	ELSE
	BEGIN
		SELECT 1;
	END 
END TRY
BEGIN CATCH
SELECT
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_SEVERITY() AS ErrorSeverity,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH
