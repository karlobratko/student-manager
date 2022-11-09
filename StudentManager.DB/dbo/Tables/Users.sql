CREATE TABLE [dbo].[Users]
  ( [Id]         int              NOT NULL IDENTITY (1, 1)
  , [Guid]       uniqueidentifier NOT NULL
      CONSTRAINT [DF_Users_Guid] DEFAULT NEWSEQUENTIALID()
  , [CreateDate] datetime         NOT NULL
      CONSTRAINT [DF_Users_CreateDate] DEFAULT GETDATE()
  , [CreatedBy]  int              NOT NULL
  , [UpdateDate] datetime         NOT NULL
      CONSTRAINT [DF_Users_UpdateDate] DEFAULT GETDATE()
  , [UpdatedBy]  int              NOT NULL
  , [DeleteDate] datetime         NULL
  , [DeletedBy]  int              NULL

  , [FName]       nvarchar(32)   NOT NULL
  , [LName]       nvarchar(32)   NOT NULL
  , [BirthDate]   date           NOT NULL
  , [Email]       nvarchar(256)  NOT NULL
  , [PhoneNumber] nvarchar(32)   NULL
  , [Address]     nvarchar(128)  NULL
  , [Image]       varbinary(MAX) NULL

  , CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)

  , CONSTRAINT [FK_Users_CreatedBy] FOREIGN KEY ( [CreatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Users_UpdatedBy] FOREIGN KEY ( [UpdatedBy] ) REFERENCES [dbo].[Users] ( [Id] )
  , CONSTRAINT [FK_Users_DeletedBy] FOREIGN KEY ( [DeletedBy] ) REFERENCES [dbo].[Users] ( [Id] ) )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Guid]  ON [dbo].[Users] ( [Guid] ASC )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users] ( [Email] ASC )
GO