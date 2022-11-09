﻿CREATE PROCEDURE [dbo].[UserUpdate] ( @Guid        AS uniqueidentifier
                                    , @FName       AS nvarchar(32)
                                    , @LName       AS nvarchar(32)
                                    , @BirthDate   AS date
                                    , @Email       AS nvarchar(256)
                                    , @PhoneNumber AS nvarchar(32)
                                    , @Address     AS nvarchar(128)
                                    , @Image       AS varbinary(MAX)
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
  FROM [dbo].[Users]
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
      @Id       = [Id]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Users]
  WHERE [Email] = @Email
    AND [Guid] <> @Guid

  IF  @Id IS NOT NULL
  AND @DeleteDate IS NULL BEGIN
    RETURN 4
  END
  ELSE IF @Id IS NOT NULL
      AND @DeleteDate IS NOT NULL BEGIN
    RETURN 3
  END

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
  WHERE [Guid] = @Guid

  IF @@ROWCOUNT = 1 BEGIN
    RETURN 1
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO