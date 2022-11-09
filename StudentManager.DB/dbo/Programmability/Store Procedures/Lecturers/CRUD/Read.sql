CREATE PROCEDURE [dbo].[LecturerRead] ( @Id AS int = NULL )
AS BEGIN
  IF @Id IS NULL BEGIN
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
    WHERE [Lecturers].[DeleteDate] IS NULL
  END
  ELSE BEGIN
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
    WHERE [Lecturers].[DeleteDate] IS NULL
      AND [Lecturers].[Id] = @Id
  END
END
GO
