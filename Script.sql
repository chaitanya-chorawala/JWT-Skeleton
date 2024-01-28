USE MERC
GO

CREATE TABLE TestJwtUser(
	  Id uniqueidentifier primary key
	, Username varchar(20)
	, Email varchar(100)
	, Password varchar(20)
	, Role varchar(10)
)
GO

INSERT INTO TestJwtUser(Id, Username, Email, Password, Role)
SELECT NEWID(), 'Admin', 'Chaitanyachorawala@ltfs.com', 'Admin', 'Admin'

CREATE TABLE TestJwtRefreshToken(
	  UserId uniqueidentifier primary key
	, Token varchar(100)
)
GO

CREATE PROCEDURE TestJwt_LoginUser
	  @Username varchar(20)
	, @Password varchar(20)
AS
BEGIN 
	SELECT
		  A.Id
		, A.Username
		, A.Email
		, A.Password
		, A.Role
	FROM TestJwtUser A WITH(NOLOCK)
	WHERE A.Username = @Username AND A.Password = @Password
END
GO


CREATE PROCEDURE TestJwt_RegisterUser
	  @Username varchar(20)
	, @Email varchar(100)
	, @Password varchar(20)
AS
BEGIN 
	
	IF EXISTS (SELECT 1 FROM TestJwtUser A WITH(NOLOCK) WHERE A.Username = @Username)
	BEGIN 
		SELECT 'User already exists!' AS Message
		RETURN
	END

	INSERT INTO TestJwtUser(Id, Username, Email, Password, Role)
	SELECT NEWID(), @Username, @Email, @Password, 'User'

	SELECT 'Registration successful!' AS Message
END
GO

CREATE PROCEDURE TestJwt_VerifyRefreshToken
	  @UserId uniqueidentifier
	, @Token varchar(100)	
AS
BEGIN
	SELECT 
		Token 
	FROM TestJwtRefreshToken A WITH(NOLOCK)
	WHERE A.UserId = @UserId AND A.Token = @Token
END
GO


CREATE PROCEDURE TestJwt_UpdateRefreshToken
	  @UserId uniqueidentifier
	, @Token varchar(100)	
AS
BEGIN
	IF EXISTS (SELECT 1 FROM TestJwtRefreshToken A WITH(NOLOCK) WHERE A.UserId = @UserId)
	BEGIN 
		UPDATE A SET
			A.Token = @Token
		FROM TestJwtRefreshToken A WITH(NOLOCK)
		WHERE A.UserId = @UserId
	END
	ELSE BEGIN 
		INSERT INTO TestJwtRefreshToken(UserId, Token)
		SELECT @UserId, @Token
	END
	
END
GO


CREATE PROCEDURE TestJwt_UserList	  
AS
BEGIN 
	SELECT
		  A.Id
		, A.Username
		, A.Email
		, A.Password
		, A.Role
	FROM TestJwtUser A WITH(NOLOCK)	
END
GO


CREATE PROCEDURE TestJwt_GetUser
	  @Userid uniqueidentifier
AS
BEGIN 
	SELECT
		  A.Id
		, A.Username
		, A.Email
		, A.Password
		, A.Role
	FROM TestJwtUser A WITH(NOLOCK)
	WHERE A.Id = @Userid
END
GO

SELECT * FROM TestJwtUser
SELECT * FROM TestJwtRefreshToken