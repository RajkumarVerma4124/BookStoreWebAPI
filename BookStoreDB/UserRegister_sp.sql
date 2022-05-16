-- ================================================
--User Registration Stored Procedure
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spUserRegistation 
	@FullName varchar(100),
	@EmailId varchar(100),
	@Password varchar(100),
	@MobileNum bigint
AS
BEGIN TRY
	Insert Into Users Values(@FullName, @EmailId, @Password, @MobileNum) 
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

