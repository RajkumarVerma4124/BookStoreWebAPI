-----------------------------Welcome To The Book STore DataBase---------------------------------

------------------------------------------Create DB----------------------------------------------
use BookStoreDB;

------------------------------------------------------------------------------------------------------------------------
--******************************************** Creating Book Table ***************************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Books(
	BookId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	BookName VARCHAR(255) NOT NULL,
	AuthorName VARCHAR(255) NOT NULL,
	Rating FLOAT ,
	RatingCount INT ,
	DiscountPrice FLOAT NOT NULL,
	ActualPrice FLOAT NOT NULL,
	BookImage VARCHAR(MAX) NOT NULL,
	BookQuantity INT NOT NULL,
	BookDetails VARCHAR(MAX) NOT NULL,
);

SELECT * FROM Books;

------------------------------------------------------------------------------------------------------------------------
--**************************************** Creating Add Book Procedure *********************************************----
-------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE spAddBook 
	-- Add the parameters for the stored procedure here
	@BookName VARCHAR(255),
	@AuthorName VARCHAR(255),
	@Rating FLOAT,
	@RatingCount INT,
	@DiscountPrice FLOAT,
	@ActualPrice FLOAT,
	@BookImage VARCHAR(255),
	@BookQuantity INT,
	@BookDetails VARCHAR(max),
	@BookId INT OUTPUT
AS
BEGIN TRY
	INSERT INTO Books (BookName, AuthorName, Rating, RatingCount, DiscountPrice, ActualPrice, BookImage, BookQuantity, BookDetails) 
				VALUES(@BookName,@AuthorName,@Rating,@RatingCount,@DiscountPrice,@ActualPrice,@BookImage,@BookQuantity,@BookDetails);
	SET @BookId= SCOPE_IDENTITY()
	RETURN @BookId;
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
--**************************************** UPDATE Book Procedure *********************************************----
-------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE spUpdateBook 
	-- Add the parameters for the stored procedure here
	@BookId INT,
	@BookName VARCHAR(255),
	@AuthorName VARCHAR(255),
	@Rating FLOAT,
	@RatingCount INT,
	@DiscountPrice FLOAT,
	@ActualPrice FLOAT,
	@BookImage VARCHAR(255),
	@BookQuantity INT,
	@BookDetails VARCHAR(max)
AS
BEGIN TRY
	UPDATE Books SET 
		BookName = @BookName,
		AuthorName= @AuthorName,
		Rating = @Rating,
		RatingCount = @RatingCount,
		DiscountPrice = @DiscountPrice,
		ActualPrice = @ActualPrice,
		BookImage = @BookImage,
		BookQuantity = @BookQuantity,
		BookDetails = @BookDetails
		WHERE BookId = @BookId
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
----****************************************** Delete Book Procedure ***********************************************----
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE spDeleteBook 
	-- Add the parameters for the stored procedure here
	@BookId INT
AS
BEGIN TRY
	DELETE FROM Books WHERE BookId = @BookId
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
----****************************************** Get All Book Procedure **********************************************----
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE spGetAllBook 
AS
BEGIN TRY
	SELECT * FROM Books
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
----****************************************** Get Book By Id Procedure **********************************************----
-------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE spGetBookById 
	-- Add the parameters for the stored procedure here
	@BookId INT
AS
BEGIN TRY
	SELECT * FROM Books WHERE BookId = @BookId
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

