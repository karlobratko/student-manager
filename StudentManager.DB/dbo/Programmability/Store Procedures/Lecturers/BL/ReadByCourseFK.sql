CREATE PROCEDURE [dbo].[LecturerReadByCourseFK] (@CourseFK AS int)
AS BEGIN
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
  INNER JOIN [dbo].[Assistants]
    ON [Lecturers].[Id] = [Assistants].[LecturerFK]
  WHERE [Lecturers].[DeleteDate] IS NULL
    AND [Assistants].[CourseFK] = @CourseFK
END
