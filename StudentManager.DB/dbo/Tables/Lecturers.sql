CREATE TABLE [dbo].[Lecturers]
  ( [Id]         int              NOT NULL IDENTITY (1, 1)
  , [Guid]       uniqueidentifier NOT NULL
      CONSTRAINT [DF_Lecturers_Guid] DEFAULT NEWSEQUENTIALID()
  , [CreateDate] datetime         NOT NULL
      CONSTRAINT [DF_Lecturers_CreateDate] DEFAULT GETDATE()
  , [CreatedBy]  int              NOT NULL
  , [UpdateDate] datetime         NOT NULL
      CONSTRAINT [DF_Lecturers_UpdateDate] DEFAULT GETDATE()
  , [UpdatedBy]  int              NOT NULL
  , [DeleteDate] datetime         NULL
  , [DeletedBy]  int              NULL

  , [UserFK] int NOT NULL

  , CONSTRAINT [PK_Lecturers] PRIMARY KEY CLUSTERED ([Id] ASC)

  , CONSTRAINT [FK_Lecturers_CreatedBy] FOREIGN KEY ( [CreatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Lecturers_UpdatedBy] FOREIGN KEY ( [UpdatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Lecturers_DeletedBy] FOREIGN KEY ( [DeletedBy] ) REFERENCES [dbo].[Users] ( [Id] )

  , CONSTRAINT [FK_Lecturers_UserFK] FOREIGN KEY ( [UserFK] ) REFERENCES [dbo].[Users] ( [Id] ) )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Lecturers_Guid] ON [dbo].[Lecturers] ( [Guid] ASC )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Lecturers_UserFK] ON [dbo].[Lecturers] ( [UserFK] ASC )
GO