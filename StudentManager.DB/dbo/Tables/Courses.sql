CREATE TABLE [dbo].[Courses]
  ( [Id]         int              NOT NULL IDENTITY (1, 1)
  , [Guid]       uniqueidentifier NOT NULL
      CONSTRAINT [DF_Courses_Guid] DEFAULT NEWSEQUENTIALID()
  , [CreateDate] datetime         NOT NULL
      CONSTRAINT [DF_Courses_CreateDate] DEFAULT GETDATE()
  , [CreatedBy]  int              NOT NULL
  , [UpdateDate] datetime         NOT NULL
      CONSTRAINT [DF_Courses_UpdateDate] DEFAULT GETDATE()
  , [UpdatedBy]  int              NOT NULL
  , [DeleteDate] datetime         NULL
  , [DeletedBy]  int              NULL

  , [HeadLecturerFK]   int            NOT NULL
  , [Name]             nvarchar(64)   NOT NULL
  , [ECTS]             tinyint        NOT NULL
  , [Description]      nvarchar(1024) NULL
  , [MaxLectureHours]  tinyint        NOT NULL
  , [MaxPracticeHours] tinyint        NOT NULL

  , CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED ([Id] ASC)

  , CONSTRAINT [FK_Courses_CreatedBy] FOREIGN KEY ( [CreatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Courses_UpdatedBy] FOREIGN KEY ( [UpdatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Courses_DeletedBy] FOREIGN KEY ( [DeletedBy] ) REFERENCES [dbo].[Users] ( [Id] )

  , CONSTRAINT [FK_Courses_HeadLecturerFK] FOREIGN KEY ( [HeadLecturerFK] ) REFERENCES [dbo].[Lecturers] ( [Id] ) )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Courses_Guid] ON [dbo].[Courses] ( [Guid] ASC )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Courses_Name] ON [dbo].[Courses] ( [Name] ASC )
GO