CREATE PROCEDURE [dbo].[CourseCreate] ( @HeadLecturerFK   AS int
                                      , @Name             AS nvarchar(64)
                                      , @ECTS             AS tinyint
                                      , @Description      AS nvarchar(1024)
                                      , @MaxLectureHours  AS tinyint
                                      , @MaxPracticeHours AS tinyint
                                      , @CreatedBy        AS int )
AS BEGIN
  IF @CreatedBy IS NULL BEGIN
    SET @CreatedBy = 1
  END

  DECLARE @Guid       AS uniqueidentifier
  DECLARE @DeleteDate AS datetime
  SELECT ALL TOP 1
      @Guid       = [Guid]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Courses]
  WHERE [Name] = @Name

  IF @Guid IS NULL BEGIN
    INSERT INTO [dbo].[Courses]
    ( [CreatedBy]
    , [UpdatedBy]
    , [HeadLecturerFK]
    , [Name]
    , [ECTS]
    , [Description]
    , [MaxLectureHours]
    , [MaxPracticeHours] )
    VALUES
    ( @CreatedBy
    , @CreatedBy
    , @HeadLecturerFK
    , @Name
    , @ECTS
    , @Description
    , @MaxLectureHours
    , @MaxPracticeHours )

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
      , [HeadLecturerFK]
      , [Name]
      , [ECTS]
      , [Description]
      , [MaxLectureHours]
      , [MaxPracticeHours]
    FROM [dbo].[Courses]
    WHERE [Id] = @Id

    RETURN 1
  END
  ELSE IF @Guid       IS NOT NULL
      AND @DeleteDate IS NOT NULL BEGIN
    UPDATE [dbo].[Courses]
    SET [DeleteDate]       = NULL
      , [DeletedBy]        = NULL
      , [UpdateDate]       = GETDATE()
      , [UpdatedBy]        = @CreatedBy
      , [HeadLecturerFK]   = @HeadLecturerFK
      , [ECTS]             = @ECTS
      , [Description]      = @Description
      , [MaxLectureHours]  = @MaxLectureHours
      , [MaxPracticeHours] = @MaxPracticeHours
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
      , [HeadLecturerFK]
      , [Name]
      , [ECTS]
      , [Description]
      , [MaxLectureHours]
      , [MaxPracticeHours]
    FROM [dbo].[Courses]
    WHERE [Guid] = @Guid

    RETURN 3
  END
  ELSE IF @Guid       IS NOT NULL
      AND @DeleteDate IS NULL BEGIN
    UPDATE [dbo].[Courses]
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
      , [HeadLecturerFK]
      , [Name]
      , [ECTS]
      , [Description]
      , [MaxLectureHours]
      , [MaxPracticeHours]
    FROM [dbo].[Courses]
    WHERE [Guid] = @Guid

    RETURN 2
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO