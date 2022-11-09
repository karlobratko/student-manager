CREATE TABLE [dbo].[CourseParticipants]
  ( [Id]         int              NOT NULL IDENTITY (1, 1)
  , [Guid]       uniqueidentifier NOT NULL
      CONSTRAINT [DF_CourseParticipants_Guid] DEFAULT NEWSEQUENTIALID()
  , [CreateDate] datetime         NOT NULL
      CONSTRAINT [DF_CourseParticipants_CreateDate] DEFAULT GETDATE()
  , [CreatedBy]  int              NOT NULL
  , [UpdateDate] datetime         NOT NULL
      CONSTRAINT [DF_CourseParticipants_UpdateDate] DEFAULT GETDATE()
  , [UpdatedBy]  int              NOT NULL
  , [DeleteDate] datetime         NULL
  , [DeletedBy]  int              NULL

  , [StudentFK]             int     NOT NULL
  , [CourseFK]              int     NOT NULL
  , [AttendedLectureHours]  tinyint NOT NULL
      CONSTRAINT [DF_CourseParticipants_AttendedLectureHours] DEFAULT 0
  , [AttendedPracticeHours] tinyint NOT NULL
      CONSTRAINT [DF_CourseParticipants_AttendedPracticeHours] DEFAULT 0
  , [IsSigned]              bit     NOT NULL
      CONSTRAINT [DF_CourseParticipants_IsSigned] DEFAULT 0
  , [Grade]                 tinyint NULL

  , CONSTRAINT [PK_CourseParticipants] PRIMARY KEY CLUSTERED ([Id] ASC)

  , CONSTRAINT [FK_CourseParticipants_CreatedBy] FOREIGN KEY ( [CreatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_CourseParticipants_UpdatedBy] FOREIGN KEY ( [UpdatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_CourseParticipants_DeletedBy] FOREIGN KEY ( [DeletedBy] ) REFERENCES [dbo].[Users] ( [Id] )

  , CONSTRAINT [FK_CourseParticipants_StudentFK] FOREIGN KEY ( [StudentFK] ) REFERENCES [dbo].[Students] ( [Id] )
  , CONSTRAINT [FK_CourseParticipants_CourseFK]  FOREIGN KEY ( [CourseFK] )  REFERENCES [dbo].[Courses]  ( [Id] ) )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_CourseParticipants_Guid] ON [dbo].[CourseParticipants] ( [Guid] ASC )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_CourseParticipants_CourseParticipant] ON [dbo].[CourseParticipants] ( [StudentFK] ASC, [CourseFK] ASC )
GO