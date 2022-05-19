-----------------------------Welcome To The Book STore DataBase---------------------------------

------------------------------------------Create DB----------------------------------------------
use BookStoreDB;

------------------------------------------------------------------------------------------------------------------------
--********************************************** Creating Feedback  Table *********************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Feedback (
	FeedbackId INT IDENTITY(1,1) PRIMARY KEY,
	Comment VARCHAR(MAX) NOT NULL,
	Rating INT NOT NULL,
	BookId INT NOT NULL FOREIGN KEY (BookId) REFERENCES Books(BookId),
	UserId INT NOT NULL FOREIGN KEY (USerId) REFERENCES Users(USerId)
)

SELECT * FROM Feedback
------------------------------------------------------------------------------------------------------------------------
--************************************************ Add Feedback  Table ***********************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spAddFeedback
	@Comment VARCHAR(MAX),
	@Rating INT,
	@BookId INT,
	@UserId INT
AS
DECLARE @AverageRating FLOAT;
BEGIN TRY
	IF (EXISTS(SELECT * FROM Feedback WHERE BookId = @BookId and UserId=@UserId))
		SELECT 1;
	Else
	BEGIN
		IF (EXISTS(SELECT * FROM Books WHERE BookId = @BookId))
		BEGIN 
			BEGIN TRY
				BEGIN TRANSACTION
					INSERT INTO Feedback VALUES(@Comment, @Rating, @BookId, @UserId);		
					SET @AverageRating = (SELECT  CAST(AVG(Rating) AS DECIMAL(10,1)) FROM Feedback WHERE BookId = @BookId);
					UPDATE Books SET Rating = @AverageRating,  RatingCount = RatingCount + 1  WHERE BookId = @BookId;
				COMMIT TRANSACTION
			End TRY
			BEGIN CATCH
				ROLLBACK TRANSACTION
			END CATCH
		END
		ELSE
		BEGIN
			SELECT 2; 
		END
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
--************************************************ Get ALL  Feedback  Table ***********************************************--
-------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE spGetALLFeedback
	@BookId int
AS
BEGIN TRY
	SELECT Feedback.FeedbackId,Feedback.UserId,Feedback.BookId,Feedback.Comment,Feedback.Rating, Users.FullName 
	FROM Users INNER JOIN Feedback ON Feedback.UserId = Users.UserId 
	WHERE BookId=@BookId
END TRY
BEGIN CATCH
SELECT
    ERROR_NUMBER() as ErrorNumber,
    ERROR_STATE() as ErrorState,
    ERROR_PROCEDURE() as ErrorProcedure,
    ERROR_LINE() as ErrorLine,
    ERROR_MESSAGE() as ErrorMessage;
END CATCH
