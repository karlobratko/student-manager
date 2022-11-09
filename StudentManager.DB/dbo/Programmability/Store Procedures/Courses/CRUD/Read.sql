CREATE PROCEDURE [dbo].[CourseRead] ( @Id AS int = NULL )
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
      , [HeadLecturerFK]
      , [Name]
      , [ECTS]
      , [Description]
      , [MaxLectureHours]
      , [MaxPracticeHours]
    FROM [dbo].[Courses]
    WHERE [DeleteDate] IS NULL
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
      , [HeadLecturerFK]
      , [Name]
      , [ECTS]
      , [Description]
      , [MaxLectureHours]
      , [MaxPracticeHours]
    FROM [dbo].[Courses]
    WHERE [DeleteDate] IS NULL 
      AND [Id] = @Id
  END
END
GO
