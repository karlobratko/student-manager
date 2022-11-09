CREATE PROCEDURE [dbo].[AssistantUpdate] ( @Guid       AS uniqueidentifier
                                         , @LecturerFK AS int
                                         , @CourseFK   AS int
                                         , @UpdatedBy  AS int )
AS BEGIN
  IF @UpdatedBy IS NULL BEGIN
    SET @UpdatedBy = 1
  END

  DECLARE @Id         AS int
  DECLARE @DeleteDate AS datetime
  SELECT ALL TOP 1
      @Id         = [Id]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Assistants]
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
  FROM [dbo].[Assistants]
  WHERE ( [LecturerFK] = @LecturerFK
      AND [CourseFK]   = @CourseFK )
      AND [Guid] <> @Guid

  IF  @Id IS NOT NULL
  AND @DeleteDate IS NULL BEGIN
    RETURN 4
  END
  ELSE IF @Id IS NOT NULL
      AND @DeleteDate IS NOT NULL BEGIN
    RETURN 3
  END

  UPDATE [dbo].[Assistants]
  SET [UpdatedBy]       = @UpdatedBy
    , [UpdateDate]      = GETDATE()
    , [LecturerFK]      = @LecturerFK
    , [CourseFK]        = @CourseFK
  WHERE [Guid] = @Guid

  IF @@ROWCOUNT = 1 BEGIN
    RETURN 1
  END
  ELSE BEGIN
    RETURN -1
  END
END
GO