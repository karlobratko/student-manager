CREATE PROCEDURE [dbo].[CourseParticipantRead] ( @Id AS int = NULL )
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
      , [StudentFK]
      , [CourseFK]
      , [AttendedLectureHours]
      , [AttendedPracticeHours]
      , [IsSigned]
      , [Grade]
    FROM [dbo].[CourseParticipants]
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
      , [StudentFK]
      , [CourseFK]
      , [AttendedLectureHours]
      , [AttendedPracticeHours]
      , [IsSigned]
      , [Grade]
    FROM [dbo].[CourseParticipants]
    WHERE [DeleteDate] IS NULL 
      AND [Id] = @Id
  END
END
GO
