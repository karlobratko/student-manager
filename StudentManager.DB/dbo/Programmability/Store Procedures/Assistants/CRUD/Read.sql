CREATE PROCEDURE [dbo].[AssistantRead] ( @Id AS int = NULL )
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
      , [LecturerFK]
      , [CourseFK]
    FROM [dbo].[Assistants]
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
      , [LecturerFK]
      , [CourseFK]
    FROM [dbo].[Assistants]
    WHERE [DeleteDate] IS NULL 
      AND [Id] = @Id
  END
END
GO