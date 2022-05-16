-----------------------------Welcome To The Book STore DataBase---------------------------------

------------------------------------------Create DB----------------------------------------------
create database BookStoreDB;
use BookStoreDB;


------------------------------------------------------------------------------------------------------------------------
--**************************************** Creating BookStore User Table *********************************************--
-------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Users (
	UserId INT IDENTITY (1, 1) PRIMARY KEY,
	FullName VARCHAR (100) NOT NULL,
	EmailId VARCHAR (255) NOT NULL UNIQUE,
	Password VARCHAR (100) NOT NULL,
	MobileNumber bigint NOT NULL
);


