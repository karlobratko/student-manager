CREATE PROCEDURE [dbo].[AssistantDeleteByCourseFKAndLecturerFK] ( @CourseFK AS int
                                                                , @LecturerFK AS int
                                                                , @DeletedBy AS int )
AS BEGIN
  IF @DeletedBy IS NULL BEGIN
    SET @DeletedBy = 1
  END

  DECLARE @Id         AS int
  DECLARE @DeleteDate AS datetime
  SELECT ALL TOP 1
      @Id         = [Id]
    , @DeleteDate = [DeleteDate]
  FROM [dbo].[Assistants]
  WHERE [CourseFK] = @CourseFK
    AND [LecturerFK] = @LecturerFK

  IF @Id IS NULL BEGIN
    RETURN 2
  END
  ELSE IF @Id IS NOT NULL 
      AND @DeleteDate IS NOT NULL BEGIN
    RETURN 3
  END

  UPDATE [dbo].[Assistants]
  SET [DeletedBy]  = @DeletedBy
    , [DeleteDate] = GETDATE()
  WHERE [CourseFK] = @CourseFK
    AND [LecturerFK] = @LecturerFK

  IF @@ROWCOUNT = 1 BEGIN
    RETURN 1
  END
  ELSE BEGIN
    RETURN -1
  END
END
