CREATE TABLE [dbo].[Assistants]
  ( [Id]         int              NOT NULL IDENTITY (1, 1)
  , [Guid]       uniqueidentifier NOT NULL
      CONSTRAINT [DF_Assistants_Guid] DEFAULT NEWSEQUENTIALID()
  , [CreateDate] datetime         NOT NULL
      CONSTRAINT [DF_Assistants_CreateDate] DEFAULT GETDATE()
  , [CreatedBy]  int              NOT NULL
  , [UpdateDate] datetime         NOT NULL
      CONSTRAINT [DF_Assistants_UpdateDate] DEFAULT GETDATE()
  , [UpdatedBy]  int              NOT NULL
  , [DeleteDate] datetime         NULL
  , [DeletedBy]  int              NULL

  , [LecturerFK] int NOT NULL
  , [CourseFK]   int NOT NULL

  , CONSTRAINT [PK_Assistants] PRIMARY KEY CLUSTERED ([Id] ASC)

  , CONSTRAINT [FK_Assistants_CreatedBy] FOREIGN KEY ( [CreatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Assistants_UpdatedBy] FOREIGN KEY ( [UpdatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Assistants_DeletedBy] FOREIGN KEY ( [DeletedBy] ) REFERENCES [dbo].[Users] ( [Id] )

  , CONSTRAINT [FK_Assistants_LecturerFK] FOREIGN KEY ( [LecturerFK] ) REFERENCES [dbo].[Lecturers] ( [Id] )
  , CONSTRAINT [FK_Assistants_CourseFK]   FOREIGN KEY ( [CourseFK] )   REFERENCES [dbo].[Courses]   ( [Id] ) )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Assistants_Guid] ON [dbo].[Assistants] ( [Guid] ASC )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Assistants_Assistant] ON [dbo].[Assistants] ( [LecturerFK] ASC, [CourseFK] ASC )
GO