CREATE PROCEDURE [dbo].[UserRead] ( @Id AS int = NULL )
AS BEGIN
  IF @Id IS NULL BEGIN
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
    WHERE [Id] <> 1
      AND [DeleteDate] IS NULL
  END
  ELSE BEGIN
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
    WHERE [DeleteDate] IS NULL 
      AND [Id] = @Id
      AND [Id] <> 1
  END
END
GO
