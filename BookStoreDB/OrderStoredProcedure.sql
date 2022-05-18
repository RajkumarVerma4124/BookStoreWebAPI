-----------------------------Welcome To The Book STore DataBase---------------------------------

------------------------------------------Create DB----------------------------------------------
use BookStoreDB;

------------------------------------------------------------------------------------------------------------------------
--********************************************** Creating Order Table *********************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE TABLE BookOrders (
    OrderId INT NOT NULL IDENTITY (1,1) PRIMARY KEY,
	OrderTotalPrice FLOAT NOT NULL,
	ActualTotalPrice FLOAT NOT NULL,
	BookQuantity INT NOT NULL,
	OrderDate Date NOT NULL,
	UserId INT NOT NULL, FOREIGN KEY (UserId) REFERENCES Users(UserId),
	BookId INT NOT NULL FOREIGN KEY (BookId) REFERENCES Books(BookId),
	AddressId INT NOT NULL FOREIGN KEY (AddressId) REFERENCES Address(AddressId),
);

SELECT * FROM BookOrders

------------------------------------------------------------------------------------------------------------------------
--********************************************** Creating Add Order Procedure *****************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spAddOrders
	@UserId INT,
	@AddressId INT,
	@BookId INT
AS
	DECLARE @DiscountPrice FLOAT
	DECLARE @ActualPrice FLOAT
	DECLARE @BookQuantity INT
	DECLARE @InitialBookQuantity INT
BEGIN TRY
	IF (EXISTS(SELECT * FROM Books WHERE BookId = @BookId))
	BEGIN
		IF EXISTS (SELECT * FROM Address WHERE AddressId = @AddressId)
		BEGIN
			BEGIN TRY
				BEGIN TRANSACTION	
					SELECT @DiscountPrice = DiscountPrice FROM Books WHERE BookId = @BookId;
					SELECT @ActualPrice = ActualPrice FROM Books WHERE BookId = @BookId;
					SELECT @BookQuantity = BookQuantity FROM Cart Where BookId = @BookId AND UserId = @UserId;	
					SELECT @InitialBookQuantity = BookQuantity - @BookQuantity FROM Books WHERE BookId = @BookId;
					IF(@InitialBookQuantity < 0)
					BEGIN
						SELECT 3
					END
					ELSE
						INSERT INTO BookOrders VALUES (@BookQuantity*@DiscountPrice, @BookQuantity*@ActualPrice, @BookQuantity, GETDATE(), @UserId, @BookId, @AddressId)
						UPDATE Books SET BookQuantity = BookQuantity - @BookQuantity
						DELETE FROM Cart WHERE UserId = @UserId
				COMMIT TRANSACTION
			END TRY	
			BEGIN CATCH
				ROLLBACK TRANSACTION
			END CATCH
		END 
		ELSE
		BEGIN
			Select 1
		END	
	END 
	ELSE
	BEGIN
			Select 2
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
--********************************************** Creating Get All Order Procedure *************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spGetAllOrders
	@UserId INT
AS
BEGIN TRY
	SELECT 
		Books.BookName,Books.AuthorName,Books.BookImage,
		BookOrders.OrderId, BookOrders.OrderDate, BookOrders.ActualTotalPrice, BookOrders.OrderTotalPrice, 
		BookOrders.UserId, BookOrders.BookId, BookOrders.AddressId, BookOrders.BookQuantity
		FROM Books INNER JOIN BookOrders ON BookOrders.BookId=Books.BookId 
		WHERE BookOrders.UserId=@UserId
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