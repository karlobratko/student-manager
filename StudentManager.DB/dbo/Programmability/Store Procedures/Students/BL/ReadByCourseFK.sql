CREATE PROCEDURE [dbo].[StudentReadByCourseFK] (@CourseFK AS int)
AS BEGIN
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
  INNER JOIN [dbo].[CourseParticipants]
    ON [Students].[Id] = [CourseParticipants].[StudentFK]
  WHERE [Students].[DeleteDate] IS NULL
    AND [CourseParticipants].[CourseFK] = @CourseFK
END
