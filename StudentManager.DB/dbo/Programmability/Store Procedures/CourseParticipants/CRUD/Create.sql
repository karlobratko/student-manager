CREATE PROCEDURE [dbo].[CourseParticipantCreate] ( @StudentFK             AS int
                                                 , @CourseFK              AS int
                                                 , @AttendedLectureHours  AS tinyint
                                                 , @AttendedPracticeHours AS tinyint
                                                 , @IsSigned              AS bit
                                                 , @Grade                 AS tinyint
                                                 , @CreatedBy             AS int )
AS BEGIN
  IF @CreatedBy IS NULL BEGIN
    SET @CreatedBy = 1
  END

  DECLARE @Guid       AS uniqueidentifier
  DECLARE @DeleteDate AS datetime
  SELECT ALL TOP 1
      @Guid       = [Guid]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[CourseParticipants]
  WHERE [StudentFK] = @StudentFK
    AND [CourseFK]  = @CourseFK

  IF @Guid IS NULL BEGIN
    INSERT INTO [dbo].[CourseParticipants]
    ( [CreatedBy]
    , [UpdatedBy]
    , [StudentFK]
    , [CourseFK]
    , [AttendedLectureHours]
    , [AttendedPracticeHours]
    , [IsSigned]
    , [Grade] )
    VALUES
    ( @CreatedBy
    , @CreatedBy
    , @StudentFK
    , @CourseFK
    , @AttendedLectureHours
    , @AttendedPracticeHours
    , @IsSigned
    , @Grade )

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
      , [StudentFK]
      , [CourseFK]
      , [AttendedLectureHours]
      , [AttendedPracticeHours]
      , [IsSigned]
      , [Grade]
    FROM [dbo].[CourseParticipants]
    WHERE [Id] = @Id

    RETURN 1
  END
  ELSE IF @Guid       IS NOT NULL
      AND @DeleteDate IS NOT NULL BEGIN
    UPDATE [dbo].[CourseParticipants]
    SET [DeleteDate]            = NULL
      , [DeletedBy]             = NULL
      , [UpdateDate]            = GETDATE()
      , [UpdatedBy]             = @CreatedBy
      , [AttendedLectureHours]  = @AttendedLectureHours
      , [AttendedPracticeHours] = @AttendedPracticeHours
      , [IsSigned]              = @IsSigned
      , [Grade]                 = @Grade
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
      , [StudentFK]
      , [CourseFK]
      , [AttendedLectureHours]
      , [AttendedPracticeHours]
      , [IsSigned]
      , [Grade]
    FROM [dbo].[CourseParticipants]
    WHERE [Guid] = @Guid

    RETURN 3
  END
  ELSE IF @Guid       IS NOT NULL
      AND @DeleteDate IS NULL BEGIN
    UPDATE [dbo].[CourseParticipants]
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
      , [StudentFK]
      , [CourseFK]
      , [AttendedLectureHours]
      , [AttendedPracticeHours]
      , [IsSigned]
      , [Grade]
    FROM [dbo].[CourseParticipants]
    WHERE [Guid] = @Guid

    RETURN 2
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO