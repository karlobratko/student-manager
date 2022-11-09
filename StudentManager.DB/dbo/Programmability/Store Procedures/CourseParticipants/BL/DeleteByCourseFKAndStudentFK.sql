CREATE PROCEDURE [dbo].[CourseParticipantDeleteByCourseFKAndLecturerFK] ( @CourseFK AS int
                                                                        , @StudentFK AS int
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
  FROM [dbo].[CourseParticipants]
  WHERE [CourseFK] = @CourseFK
    AND [StudentFK] = @StudentFK

  IF @Id IS NULL BEGIN
    RETURN 2
  END
  ELSE IF @Id IS NOT NULL 
      AND @DeleteDate IS NOT NULL BEGIN
    RETURN 3
  END

  UPDATE [dbo].[CourseParticipants]
  SET [DeletedBy]  = @DeletedBy
    , [DeleteDate] = GETDATE()
  WHERE [CourseFK] = @CourseFK
    AND [StudentFK] = @StudentFK

  IF @@ROWCOUNT = 1 BEGIN
    RETURN 1
  END
  ELSE BEGIN
    RETURN -1
  END
END
