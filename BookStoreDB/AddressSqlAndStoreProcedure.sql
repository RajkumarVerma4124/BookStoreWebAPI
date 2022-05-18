-----------------------------Welcome To The Book STore DataBase---------------------------------

------------------------------------------Create DB----------------------------------------------
use BookStoreDB;

------------------------------------------------------------------------------------------------------------------------
--*************************************** Creating Address Type Table ****************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE TABLE AddressType(
	TypeId INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
	AddrType VARCHAR(50)
);

INSERT INTO AddressType VALUES ('Home')
INSERT INTO AddressType VALUES ('Work')
INSERT INTO AddressType VALUES ('Other')

SELECT * FROM AddressType

------------------------------------------------------------------------------------------------------------------------
--********************************************* Creating Address Table ***********************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Address (
	AddressId INT IDENTITY(1,1) PRIMARY KEY,
	Address VARCHAR(MAX) NOT NULL,
	City VARCHAR(100) NOT NULL,
	State VARCHAR(100) NOT NULL,
	TypeId INT NOT NULL FOREIGN KEY (TypeId) REFERENCES AddressType(TypeId),
	UserId INT NOT NULL FOREIGN KEY (UserId) REFERENCES Users(UserId),
);

SELECT * FROM Address

------------------------------------------------------------------------------------------------------------------------
--**************************************** Creating Add Adress Stored Procedure ************************************----
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spAddAddress
	@Address VARCHAR(MAX),
	@City VARCHAR(100),
	@State VARCHAR(100),
	@TypeId INT,
	@UserId INT
AS
BEGIN TRY
	If EXISTS(SELECT * FROM AddressType WHERE TypeId=@TypeId)
		BEGIN
			INSERT INTO Address VALUES(@Address, @City, @State, @TypeId, @UserId);
		END
	Else
		BEGIN
			SELECT 2
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

------------------------------------------------------------------------------------------------------------------------
--*************************************** Creating Update Address Stored Procedure **********************************----
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spUpdateAddress
	@AddressId INT,
	@Address VARCHAR(MAX),
	@City VARCHAR(100),
	@State VARCHAR(100),
	@TypeId INT
AS
BEGIN TRY
	IF EXISTS(SELECT * FROM AddressType WHERE TypeId=@TypeId)
	BEGIN
		IF EXISTS(SELECT * FROM Address WHERE AddressId = @AddressId)
		BEGIN
			UPDATE Address SET 
				Address = @Address, 
				City = @City,
				State = @State, 
				TypeId = @TypeId
			WHERE AddressId = @AddressId
		END
		Else
		BEGIN
			SELECT 1
		END
	END
	Else
	BEGIN
		SELECT 2
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

------------------------------------------------------------------------------------------------------------------------
--*************************************** Creating Delete Address Stored Procedure *********************************----
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE spDeleteAddress
	@AddressId INT
AS
BEGIN TRY
	DELETE FROM Address WHERE AddressId=@AddressId
END TRY
BEGIN CATCH
SELECT
    ERROR_NUMBER() as ErrorNumber,
    ERROR_STATE() as ErrorState,
    ERROR_PROCEDURE() as ErrorProcedure,
    ERROR_LINE() as ErrorLine,
    ERROR_MESSAGE() as ErrorMessage;
End CATCH

------------------------------------------------------------------------------------------------------------------------
--**************************************** Creating Get Address Stored Procedure ************************************----
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spGetAddress
	@UserId INT,
	@TypeId INT
AS
BEGIN TRY
	SELECT * FROM Address WHERE UserId = @UserId And TypeId = @TypeId
END TRY
BEGIN CATCH
SELECT
    ERROR_NUMBER() as ErrorNumber,
    ERROR_STATE() as ErrorState,
    ERROR_PROCEDURE() as ErrorProcedure,
    ERROR_LINE() as ErrorLine,
    ERROR_MESSAGE() as ErrorMessage;
End CATCH

------------------------------------------------------------------------------------------------------------------------
--**************************************** Creating Get ALL Address Stored Procedure ************************************----
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spGetAllAddress
	@UserId INT
AS
BEGIN TRY
	SELECT * FROM Address WHERE UserId = @UserId 
END TRY
BEGIN CATCH
SELECT
    ERROR_NUMBER() as ErrorNumber,
    ERROR_STATE() as ErrorState,
    ERROR_PROCEDURE() as ErrorProcedure,
    ERROR_LINE() as ErrorLine,
    ERROR_MESSAGE() as ErrorMessage;
End CATCH
