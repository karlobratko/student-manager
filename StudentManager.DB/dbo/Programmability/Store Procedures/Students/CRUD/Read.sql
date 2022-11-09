CREATE PROCEDURE [dbo].[StudentRead] ( @Id AS int = NULL )
AS BEGIN
  IF @Id IS NULL BEGIN
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
    WHERE [Students].[DeleteDate] IS NULL
  END
  ELSE BEGIN
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
    WHERE [Students].[DeleteDate] IS NULL
      AND [Students].[Id] = @Id
  END
END
GO
