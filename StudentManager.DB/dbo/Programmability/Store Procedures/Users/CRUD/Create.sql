CREATE PROCEDURE [dbo].[UserCreate] ( @FName       AS nvarchar(32)
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

  DECLARE @Guid       AS uniqueidentifier
  DECLARE @DeleteDate AS datetime
  SELECT ALL TOP 1
      @Guid       = [Guid]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Users]
  WHERE [Email] = @Email

  IF @Guid IS NULL BEGIN
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

    DECLARE @Id AS int = SCOPE_IDENTITY()
    SELECT ALL
        [Id]
      , [Guid]
      , [CreateDate]
      , [CreatedBy]
      , [UpdateDate]
      , [UpdatedBy]
      , [DeleteDate]
      , [DeletedBy]
      , [FName]
      , [LName]
      , [BirthDate]
      , [Email]
      , [PhoneNumber]
      , [Address]
      , [Image]
    FROM [dbo].[Users]
    WHERE [Id] = @Id

    RETURN 1
  END
  ELSE IF @Guid       IS NOT NULL
      AND @DeleteDate IS NOT NULL BEGIN
    UPDATE [dbo].[Users]
    SET [DeleteDate]  = NULL
      , [DeletedBy]   = NULL
      , [UpdateDate]  = GETDATE()
      , [UpdatedBy]   = @CreatedBy
      , [FName]       = @FName
      , [LName]       = @LName
      , [BirthDate]   = @BirthDate
      , [PhoneNumber] = @PhoneNumber
      , [Address]     = @Address
      , [Image]       = @Image
    WHERE [Guid] = @Guid

    SELECT ALL
        [Id]
      , [Guid]
      , [CreateDate]
      , [CreatedBy]
      , [UpdateDate]
      , [UpdatedBy]
      , [DeleteDate]
      , [DeletedBy]
      , [FName]
      , [LName]
      , [BirthDate]
      , [Email]
      , [PhoneNumber]
      , [Address]
      , [Image]
    FROM [dbo].[Users]
    WHERE [Guid] = @Guid

    RETURN 3
  END
  ELSE IF @Guid       IS NOT NULL
      AND @DeleteDate IS NULL BEGIN
    UPDATE [dbo].[Users]
    SET [UpdateDate] = GETDATE()
      , [UpdatedBy]  = @CreatedBy
    WHERE [Guid] = @Guid

    SELECT ALL
        [Id]
      , [Guid]
      , [CreateDate]
      , [CreatedBy]
      , [UpdateDate]
      , [UpdatedBy]
      , [DeleteDate]
      , [DeletedBy]
      , [FName]
      , [LName]
      , [BirthDate]
      , [Email]
      , [PhoneNumber]
      , [Address]
      , [Image]
    FROM [dbo].[Users]
    WHERE [Guid] = @Guid

    RETURN 2
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO