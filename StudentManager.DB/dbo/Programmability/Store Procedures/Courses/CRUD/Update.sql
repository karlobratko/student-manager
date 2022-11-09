CREATE PROCEDURE [dbo].[CourseUpdate] ( @Guid             AS uniqueidentifier
                                      , @HeadLecturerFK   AS int
                                      , @Name             AS nvarchar(64)
                                      , @ECTS             AS tinyint
                                      , @Description      AS nvarchar(1024)
                                      , @MaxLectureHours  AS tinyint
                                      , @MaxPracticeHours AS tinyint
                                      , @UpdatedBy        AS int )
AS BEGIN
  IF @UpdatedBy IS NULL BEGIN
    SET @UpdatedBy = 1
  END

  DECLARE @Id         AS int
  DECLARE @DeleteDate AS datetime
  SELECT ALL TOP 1
      @Id         = [Id]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Courses]
  WHERE [Guid] = @Guid

  IF @Id IS NULL BEGIN
    RETURN 2
  END
  ELSE IF @Id IS NOT NULL 
      AND @DeleteDate IS NOT NULL BEGIN
    RETURN 3
  END

  SET @Id         = NULL
  SET @DeleteDate = NULL

 SELECT ALL TOP 1
      @Id       = [Id]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Courses]
  WHERE [Name] = @Name
    AND [Guid] <> @Guid

  IF  @Id IS NOT NULL
  AND @DeleteDate IS NULL BEGIN
    RETURN 4
  END
  ELSE IF @Id IS NOT NULL
      AND @DeleteDate IS NOT NULL BEGIN
    RETURN 3
  END

  UPDATE [dbo].[Courses]
  SET [UpdatedBy]        = @UpdatedBy
    , [UpdateDate]       = GETDATE()
    , [HeadLecturerFK]   = @HeadLecturerFK
    , [Name]             = @Name
    , [ECTS]             = @ECTS
    , [Description]      = @Description
    , [MaxLectureHours]  = @MaxLectureHours
    , [MaxPracticeHours] = @MaxPracticeHours
  WHERE [Guid] = @Guid

  IF @@ROWCOUNT = 1 BEGIN
    RETURN 1
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO