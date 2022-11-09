CREATE PROCEDURE [dbo].[LecturerCreate] ( @FName       AS nvarchar(32)
                                        , @LName       AS nvarchar(32)
                                        , @BirthDate   AS date
                                        , @Email       AS nvarchar(256)
                                        , @PhoneNumber AS nvarchar(32)
                                        , @Address     AS nvarchar(128)
                                        , @Image       AS varbinary(MAX)
                                        , @CreatedBy   AS int )
AS BEGIN
  IF @CreatedBy IS NULL BEGIN
    SET @CreatedBy = 1
  END

  DECLARE @LecturerGuid AS uniqueidentifier
  DECLARE @UserGuid     AS uniqueidentifier
  DECLARE @DeleteDate   AS datetime
  SELECT ALL TOP 1
      @LecturerGuid = [Lecturers].[Guid]
    , @UserGuid     = [Users].[Guid]
    , @DeleteDate   = [Lecturers].[DeleteDate]
  FROM [dbo].[Lecturers]
  INNER JOIN [dbo].[Users]
    ON [Lecturers].[UserFK] = [Users].[Id]
  WHERE [Users].[Email] = @Email

  IF @LecturerGuid IS NULL BEGIN
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
      , @Image )

      SET @UserFK = SCOPE_IDENTITY()
    END
    ELSE BEGIN
      SELECT ALL TOP 1
        @UserFK = [Id]
      FROM [dbo].[Users]
      WHERE [Guid] = @UserGuid
    END

    INSERT INTO [dbo].[Lecturers]
    ( [CreatedBy]
    , [UpdatedBy]
    , [UserFK] )
    VALUES 
    ( @CreatedBy
    , @CreatedBy
    , @UserFK )

    DECLARE @Id AS int = SCOPE_IDENTITY()

    SELECT ALL
        [Lecturers].[Id]
      , [Lecturers].[Guid]
      , [Lecturers].[CreateDate]
      , [Lecturers].[CreatedBy]
      , [Lecturers].[UpdateDate]
      , [Lecturers].[UpdatedBy]
      , [Lecturers].[DeleteDate]
      , [Lecturers].[DeletedBy]
      , [Users].[FName]
      , [Users].[LName]
      , [Users].[BirthDate]
      , [Users].[Email]
      , [Users].[PhoneNumber]
      , [Users].[Address]
      , [Users].[Image]
    FROM [dbo].[Lecturers]
    INNER JOIN [dbo].[Users]
      ON [Lecturers].[UserFK] = [Users].[Id]
    WHERE [Lecturers].[Id] = @Id

    RETURN 1
  END
  ELSE IF @LecturerGuid IS NOT NULL
      AND @DeleteDate   IS NOT NULL BEGIN
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

    UPDATE [dbo].[Lecturers]
    SET [DeleteDate]  = NULL
      , [DeletedBy]   = NULL
      , [UpdateDate]  = GETDATE()
      , [UpdatedBy]   = @CreatedBy
    WHERE [Guid] = @LecturerGuid

    SELECT ALL
        [Lecturers].[Id]
      , [Lecturers].[Guid]
      , [Lecturers].[CreateDate]
      , [Lecturers].[CreatedBy]
      , [Lecturers].[UpdateDate]
      , [Lecturers].[UpdatedBy]
      , [Lecturers].[DeleteDate]
      , [Lecturers].[DeletedBy]
      , [Users].[FName]
      , [Users].[LName]
      , [Users].[BirthDate]
      , [Users].[Email]
      , [Users].[PhoneNumber]
      , [Users].[Address]
      , [Users].[Image]
    FROM [dbo].[Lecturers]
    INNER JOIN [dbo].[Users]
      ON [Lecturers].[UserFK] = [Users].[Id]
    WHERE [Lecturers].[Guid] = @LecturerGuid

    RETURN 3
  END
  ELSE IF @LecturerGuid IS NOT NULL
      AND @DeleteDate   IS NULL BEGIN
    UPDATE [dbo].[Users]
    SET [UpdateDate] = GETDATE()
      , [UpdatedBy]  = @CreatedBy
    WHERE [Guid] = @UserGuid

    UPDATE [dbo].[Lecturers]
    SET [UpdateDate] = GETDATE()
      , [UpdatedBy]  = @CreatedBy
    WHERE [Guid] = @LecturerGuid

    SELECT ALL
        [Lecturers].[Id]
      , [Lecturers].[Guid]
      , [Lecturers].[CreateDate]
      , [Lecturers].[CreatedBy]
      , [Lecturers].[UpdateDate]
      , [Lecturers].[UpdatedBy]
      , [Lecturers].[DeleteDate]
      , [Lecturers].[DeletedBy]
      , [Users].[FName]
      , [Users].[LName]
      , [Users].[BirthDate]
      , [Users].[Email]
      , [Users].[PhoneNumber]
      , [Users].[Address]
      , [Users].[Image]
    FROM [dbo].[Lecturers]
    INNER JOIN [dbo].[Users]
      ON [Lecturers].[UserFK] = [Users].[Id]
    WHERE [Lecturers].[Guid] = @LecturerGuid

    RETURN 2
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO