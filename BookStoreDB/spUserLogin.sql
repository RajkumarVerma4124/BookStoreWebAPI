USE [BookStoreDB]
GO
/****** Object:  StoredProcedure [dbo].[spUserLogin]    Script Date: 5/16/2022 5:21:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUserLogin]
	@EmailId varchar(100)
AS
BEGIN TRY
	SELECT * FROM Users WHERE EmailId = @EmailId
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