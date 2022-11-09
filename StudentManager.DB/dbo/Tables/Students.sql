CREATE TABLE [dbo].[Students]
  ( [Id]         int              NOT NULL IDENTITY (1, 1)
  , [Guid]       uniqueidentifier NOT NULL
      CONSTRAINT [DF_Students_Guid] DEFAULT NEWSEQUENTIALID()
  , [CreateDate] datetime         NOT NULL
      CONSTRAINT [DF_Students_CreateDate] DEFAULT GETDATE()
  , [CreatedBy]  int              NOT NULL
  , [UpdateDate] datetime         NOT NULL
      CONSTRAINT [DF_Students_UpdateDate] DEFAULT GETDATE()
  , [UpdatedBy]  int              NOT NULL
  , [DeleteDate] datetime         NULL
  , [DeletedBy]  int              NULL

  , [UserFK]      int          NOT NULL
  , [JMBAG]       nvarchar(32) NOT NULL
  , [YearOfStudy] tinyint      NOT NULL

  , CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([Id] ASC)

  , CONSTRAINT [FK_Students_CreatedBy] FOREIGN KEY ( [CreatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Students_UpdatedBy] FOREIGN KEY ( [UpdatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Students_DeletedBy] FOREIGN KEY ( [DeletedBy] ) REFERENCES [dbo].[Users] ( [Id] )

  , CONSTRAINT [FK_Students_UserFK] FOREIGN KEY ( [UserFK] ) REFERENCES [dbo].[Users] ( [Id] ) )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Students_Guid] ON [dbo].[Students] ( [Guid] ASC )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Students_UserFK] ON [dbo].[Students] ( [UserFK] ASC )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Students_JMBAG] ON [dbo].[Students] ( [JMBAG] ASC )
GO