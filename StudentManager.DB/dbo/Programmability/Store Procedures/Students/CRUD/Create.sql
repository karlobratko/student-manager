CREATE PROCEDURE [dbo].[StudentCreate] ( @FName       AS nvarchar(32)
                                       , @LName       AS nvarchar(32)
                                       , @BirthDate   AS date
                                       , @Email       AS nvarchar(256)
                                       , @PhoneNumber AS nvarchar(32)
                                       , @Address     AS nvarchar(128)
                                       , @Image       AS varbinary(MAX)
                                       , @JMBAG       AS nvarchar(32)
                                       , @YearOfStudy AS tinyint
                                       , @CreatedBy   AS int )
AS BEGIN
  IF @CreatedBy IS NULL BEGIN
    SET @CreatedBy = 1
  END

  DECLARE @StudentGuid AS uniqueidentifier
  DECLARE @UserGuid    AS uniqueidentifier
  DECLARE @DeleteDate  AS datetime
  SELECT ALL TOP 1
      @StudentGuid = [Students].[Guid]
    , @UserGuid    = [Users].[Guid]
    , @DeleteDate  = [Students].[DeleteDate]
  FROM [dbo].[Students]
  INNER JOIN [dbo].[Users]
    ON [Students].[UserFK] = [Users].[Id]
  WHERE [Users].[Email] = @Email
     OR [Students].[JMBAG] = @JMBAG

  IF @StudentGuid IS NULL BEGIN
    DECLARE @UserFK AS int

    IF @UserGuid IS NULL BEGIN
      INSERT INTO [dbo].[Users]
      ( [CreatedBy]
      , [UpdatedBy]
      , [FName]
      , [LName]
      , [BirthDate]
      , [Email]
      , [PhoneNumber]
      , [Address]
      , [Image] )
      VALUES
      ( @CreatedBy
      , @CreatedBy
      , @FName
      , @LName
      , @BirthDate
      , @Email
      , @PhoneNumber
      , @Address
      , @Image)

      SET @UserFK = SCOPE_IDENTITY()
    END
    ELSE BEGIN
      SELECT ALL TOP 1
        @UserFK = [Id]
      FROM [dbo].[Users]
      WHERE [Guid] = @UserGuid
    END

    INSERT INTO [dbo].[Students]
    ( [CreatedBy]
    , [UpdatedBy]
    , [UserFK]
    , [JMBAG]
    , [YearOfStudy] )
    VALUES 
    ( @CreatedBy
    , @CreatedBy
    , @UserFK
    , @JMBAG
    , @YearOfStudy )

    DECLARE @Id AS int = SCOPE_IDENTITY()

    SELECT ALL
        [Students].[Id]
      , [Students].[Guid]
      , [Students].[CreateDate]
      , [Students].[CreatedBy]
      , [Students].[UpdateDate]
      , [Students].[UpdatedBy]
      , [Students].[DeleteDate]
      , [Students].[DeletedBy]
      , [Users].[FName]
      , [Users].[LName]
      , [Users].[BirthDate]
      , [Users].[Email]
      , [Users].[PhoneNumber]
      , [Users].[Address]
      , [Users].[Image]
      , [Students].[JMBAG]
      , [Students].[YearOfStudy]
    FROM [dbo].[Students]
    INNER JOIN [dbo].[Users]
      ON [Students].[UserFK] = [Users].[Id]
    WHERE [Students].[Id] = @Id

    RETURN 1
  END
  ELSE IF @StudentGuid IS NOT NULL
      AND @DeleteDate  IS NOT NULL BEGIN
    UPDATE [dbo].[Users]
    SET [DeleteDate]  = NULL
      , [DeletedBy]   = NULL
      , [UpdateDate]  = GETDATE()
      , [UpdatedBy]   = @CreatedBy
      , [FName]       = @FName
      , [LName]       = @LName
      , [Email]       = @Email
      , [BirthDate]   = @BirthDate
      , [PhoneNumber] = @PhoneNumber
      , [Address]     = @Address
      , [Image]       = @Image
    WHERE [Guid] = @UserGuid

    UPDATE [dbo].[Students]
    SET [DeleteDate]  = NULL
      , [DeletedBy]   = NULL
      , [UpdateDate]  = GETDATE()
      , [UpdatedBy]   = @CreatedBy
      , [JMBAG]       = @JMBAG
      , [YearOfStudy] = @YearOfStudy
    WHERE [Guid] = @StudentGuid

    SELECT ALL
        [Students].[Id]
      , [Students].[Guid]
      , [Students].[CreateDate]
      , [Students].[CreatedBy]
      , [Students].[UpdateDate]
      , [Students].[UpdatedBy]
      , [Students].[DeleteDate]
      , [Students].[DeletedBy]
      , [Users].[FName]
      , [Users].[LName]
      , [Users].[BirthDate]
      , [Users].[Email]
      , [Users].[PhoneNumber]
      , [Users].[Address]
      , [Users].[Image]
      , [Students].[JMBAG]
      , [Students].[YearOfStudy]
    FROM [dbo].[Students]
    INNER JOIN [dbo].[Users]
      ON [Students].[UserFK] = [Users].[Id]
    WHERE [Students].[Guid] = @StudentGuid

    RETURN 3
  END
  ELSE IF @StudentGuid IS NOT NULL
      AND @DeleteDate  IS NULL BEGIN
    UPDATE [dbo].[Users]
    SET [UpdateDate] = GETDATE()
      , [UpdatedBy]  = @CreatedBy
    WHERE [Guid] = @UserGuid

    UPDATE [dbo].[Students]
    SET [UpdateDate] = GETDATE()
      , [UpdatedBy]  = @CreatedBy
    WHERE [Guid] = @StudentGuid

    SELECT ALL
        [Students].[Id]
      , [Students].[Guid]
      , [Students].[CreateDate]
      , [Students].[CreatedBy]
      , [Students].[UpdateDate]
      , [Students].[UpdatedBy]
      , [Students].[DeleteDate]
      , [Students].[DeletedBy]
      , [Users].[FName]
      , [Users].[LName]
      , [Users].[BirthDate]
      , [Users].[Email]
      , [Users].[PhoneNumber]
      , [Users].[Address]
      , [Users].[Image]
      , [Students].[JMBAG]
      , [Students].[YearOfStudy]
    FROM [dbo].[Students]
    INNER JOIN [dbo].[Users]
      ON [Students].[UserFK] = [Users].[Id]
    WHERE [Students].[Guid] = @StudentGuid

    RETURN 2
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO