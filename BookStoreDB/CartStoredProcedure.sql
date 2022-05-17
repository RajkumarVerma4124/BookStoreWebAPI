-----------------------------Welcome To The Book STore DataBase---------------------------------

------------------------------------------Create DB----------------------------------------------
use BookStoreDB;

------------------------------------------------------------------------------------------------------------------------
--******************************************** Creating Cart Table ***************************************************--
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Cart
(
	CartId INT IDENTITY(1,1) NOT NULL  PRIMARY KEY,
	BookQuantity INT DEFAULT 1,
	UserId INT NOT NULL FOREIGN KEY (UserId) REFERENCES Users(UserId),
	BookId INT NOT NULL FOREIGN KEY (BookId) REFERENCES Books(BookId),	
);

------------------------------------------------------------------------------------------------------------------------
--******************************************** Add Cart Stored Procedure *********************************************--
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE spAddBookToCart
	-- Add the parameters for the stored procedure here
	@BookQuantity INT,
	@UserId INT,
	@BookId INT
AS
BEGIN TRY
	INSERT INTO CART VALUES (@BookQuantity,@UserId,@BookId)
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
CREATE PROCEDURE spDeleteCart
	@CartId INT
AS
BEGIN TRY
		DELETE FROM Cart WHERE CartId = @CartId
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
CREATE PROCEDURE spUpdateCart
	@CartId INT,
	@BookQuantity INT
AS
BEGIN TRY
		UPDATE Cart SET BookQuantity = @BookQuantity WHERE CartId = @CartId
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

CREATE PROCEDURE sp_GetCartDetails
AS
BEGIN TRY
	SELECT
		Cart.CartId, Cart.UserId, Cart.BookId, Cart.BookQuantity,	
		Books.BookName, Books.AuthorName, Books.DiscountPrice, Books.ActualPrice, Books.BookImage
	FROM Cart INNER JOIN Books ON Cart.BookId = Books.BookId
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



