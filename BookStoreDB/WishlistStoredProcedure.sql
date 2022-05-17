-----------------------------Welcome To The Book STore DataBase---------------------------------

------------------------------------------Create DB----------------------------------------------
use BookStoreDB;

------------------------------------------------------------------------------------------------------------------------
--******************************************** Creating Wishlist Table ***********************************************--
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Wishlist (
	WishlistId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL FOREIGN KEY (UserId) REFERENCES Users(UserId),
	BookId INT NOT NULL FOREIGN KEY (BookId) REFERENCES Books(BookId)	
);

Select * From Wishlist
------------------------------------------------------------------------------------------------------------------------
--******************************************** Add To Wishlist Stored Procedure **************************************--
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE spAddWishlist
	-- Add the parameters for the stored procedure here
	@UserId INT,
	@BookId INT
AS
BEGIN TRY
	IF EXISTS(SELECT * FROM Wishlist WHERE BookId = @BookId AND UserId = @UserId)
		SELECT 1;
	ELSE
	BEGIN
		IF EXISTS(SELECT * FROM Books WHERE BookId = @BookId)
		BEGIN
			INSERT INTO Wishlist VALUES (@UserId,@BookId)
		END
		ELSE
			SELECT 2;
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
--***************************************** Delete Wishlist Stored Procedure *****************************************--
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE spDeleteWishlist
	@WishlistId INT
AS
BEGIN TRY
		DELETE FROM Wishlist WHERE WishlistId = @WishlistId
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
--******************************************** Get All Wishlist Stored Procedure ************************************--
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_GetAllWishlist
	@UserId INT
AS
BEGIN TRY
	SELECT
		w.WishlistId, w.UserId, w.BookId,	
		b.BookName, b.AuthorName, b.DiscountPrice, b.ActualPrice, b.BookImage
	FROM Wishlist w INNER JOIN Books b ON w.BookId = b.BookId WHERE UserId=@UserId;
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