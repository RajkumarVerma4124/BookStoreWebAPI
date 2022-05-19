-----------------------------Welcome To The Book STore DataBase---------------------------------

------------------------------------------Create DB----------------------------------------------
use BookStoreDB;

------------------------------------------------------------------------------------------------------------------------
--******************************************** Creating Cart Table ***************************************************--
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Cart (
	CartId INT IDENTITY(1,1) PRIMARY KEY,
	BookQuantity INT DEFAULT 1,
	UserId INT NOT NULL FOREIGN KEY (UserId) REFERENCES Users(UserId),
	BookId INT NOT NULL FOREIGN KEY (BookId) REFERENCES Books(BookId),	
);

Select * From Cart
------------------------------------------------------------------------------------------------------------------------
--******************************************** Add Cart Stored Procedure *********************************************--
------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spAddBookToCart
	-- Add the parameters for the stored procedure here
	@BookQuantity INT,
	@UserId INT,
	@BookId INT
AS
BEGIN TRY
	IF EXISTS(SELECT * FROM CART WHERE UserId = @UserId AND BookId = @BookId)
	BEGIN
		UPDATE Cart SET BookQuantity = BookQuantity + @BookQuantity WHERE UserId = @UserId AND BookId = @BookId 
	END
	ELSE
	BEGIN
		INSERT INTO Cart VALUES (@BookQuantity,@UserId,@BookId)
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
--******************************************** Delete Cart Stored Procedure ******************************************--
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE spDeleteCart
	@CartId INT,
	@UserId INT
AS
BEGIN TRY
		DELETE FROM Cart WHERE CartId = @CartId AND UserId= @UserId
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
--******************************************** Update Cart Stored Procedure ******************************************--
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE spUpdateCart
	@CartId INT,
	@BookQuantity INT,
	@UserId INT
AS
BEGIN TRY
		UPDATE Cart SET BookQuantity = @BookQuantity WHERE CartId = @CartId AND UserId = @UserId 
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
--******************************************** Get Cart Stored Procedure *********************************************--
---------------------------------------------------------------------------------------------------------------------------

ALTER PROCEDURE sp_GetCartDetails
		@UserId INT
AS
BEGIN TRY
	SELECT
		Cart.CartId, Cart.UserId, Cart.BookId, Cart.BookQuantity,	
		Books.BookName, Books.AuthorName, Books.DiscountPrice, Books.ActualPrice, Books.BookImage
	FROM Cart INNER JOIN Books ON Cart.BookId = Books.BookId WHERE UserId=@UserId;
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



