CREATE PROCEDURE [dbo].[AssistantCreate] ( @LecturerFK AS int
                                         , @CourseFK   AS int
                                         , @CreatedBy  AS int )
AS BEGIN
  IF @CreatedBy IS NULL BEGIN
    SET @CreatedBy = 1
  END

  DECLARE @Guid       AS uniqueidentifier
  DECLARE @DeleteDate AS datetime
  SELECT ALL TOP 1
      @Guid       = [Guid]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Assistants]
  WHERE [LecturerFK] = @LecturerFK
    AND [CourseFK]   = @CourseFK

  IF @Guid IS NULL BEGIN
    INSERT INTO [dbo].[Assistants]
    ( [CreatedBy]
    , [UpdatedBy]
    , [LecturerFK]
    , [CourseFK] )
    VALUES
    ( @CreatedBy
    , @CreatedBy
    , @LecturerFK
    , @CourseFK )

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
      , [LecturerFK]
      , [CourseFK]
    FROM [dbo].[Assistants]
    WHERE [Id] = @Id

    RETURN 1
  END
  ELSE IF @Guid       IS NOT NULL
      AND @DeleteDate IS NOT NULL BEGIN
    UPDATE [dbo].[Assistants]
    SET [DeleteDate] = NULL
      , [DeletedBy]  = NULL
      , [UpdateDate] = GETDATE()
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
      , [LecturerFK]
      , [CourseFK]
    FROM [dbo].[Assistants]
    WHERE [Guid] = @Guid

    RETURN 3
  END
  ELSE IF @Guid       IS NOT NULL
      AND @DeleteDate IS NULL BEGIN
    UPDATE [dbo].[Assistants]
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
      , [LecturerFK]
      , [CourseFK]
    FROM [dbo].[Assistants]
    WHERE [Guid] = @Guid

    RETURN 2
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO