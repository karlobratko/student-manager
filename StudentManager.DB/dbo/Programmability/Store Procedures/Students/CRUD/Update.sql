CREATE PROCEDURE [dbo].[StudentUpdate] ( @Guid        AS uniqueidentifier
                                       , @FName       AS nvarchar(32)
                                       , @LName       AS nvarchar(32)
                                       , @BirthDate   AS date
                                       , @Email       AS nvarchar(256)
                                       , @PhoneNumber AS nvarchar(32)
                                       , @Address     AS nvarchar(128)
                                       , @Image       AS varbinary(MAX)
                                       , @JMBAG       AS nvarchar(32)
                                       , @YearOfStudy AS tinyint
                                       , @UpdatedBy   AS int )
AS BEGIN
  IF @UpdatedBy IS NULL BEGIN
    SET @UpdatedBy = 1
  END

  DECLARE @Id         AS int
  DECLARE @DeleteDate AS datetime
  SELECT ALL TOP 1
      @Id         = [Id]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Students]
  WHERE [Guid] = @Guid

  IF @Id IS NULL BEGIN
    RETURN 2
  END
  ELSE IF @Id IS NOT NULL 
      AND @DeleteDate IS NOT NULL BEGIN
    RETURN 3
  END

  SET @Id         = NULL
  SET @DeleteDate = NULL

  SELECT ALL TOP 1
      @Id         = [Students].[Id]
    , @DeleteDate = [Students].[DeleteDate]
  FROM [dbo].[Students]
  INNER JOIN [dbo].[Users]
    ON [Students].[UserFK] = [Users].[Id]
  WHERE ( [Users].[Email]    = @Email
       OR [Students].[JMBAG] = @JMBAG )
    AND [Students].[Guid] <> @Guid

  IF  @Id IS NOT NULL
  AND @DeleteDate IS NULL BEGIN
    RETURN 4
  END
  ELSE IF @Id IS NOT NULL
      AND @DeleteDate IS NOT NULL BEGIN
    RETURN 3
  END

  DECLARE @UserGuid AS uniqueidentifier
  SELECT ALL TOP 1
      @UserGuid = [Users].[Guid]
  FROM [dbo].[Students]
  INNER JOIN [dbo].[Users]
    ON [Students].[UserFK] = [Users].[Id]
  WHERE [Students].[Guid] = @Guid

  UPDATE [dbo].[Users]
  SET [UpdatedBy]   = @UpdatedBy
    , [UpdateDate]  = GETDATE()
    , [FName]       = @FName
    , [LName]       = @LName
    , [Email]       = @Email
    , [BirthDate]   = @BirthDate
    , [PhoneNumber] = @PhoneNumber
    , [Address]     = @Address
    , [Image]       = @Image
  WHERE [Guid] = @UserGuid

  UPDATE [dbo].[Students]
  SET [UpdatedBy]   = @UpdatedBy
    , [UpdateDate]  = GETDATE()
    , [JMBAG]       = @JMBAG
    , [YearOfStudy] = @YearOfStudy
  WHERE [Guid] = @Guid

  IF @@ROWCOUNT = 1 BEGIN
    RETURN 1
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO