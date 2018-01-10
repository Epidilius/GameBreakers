CREATE TABLE [dbo].[MtG] (
    [ID]            VARCHAR (50)  NOT NULL,
    [layout]        VARCHAR (50)  NULL,
    [name]          VARCHAR (50)  NOT NULL,
    [manaCost]      VARCHAR (50)  NULL,
    [cmc]           INT           NULL,
    [colors]        VARCHAR (50)  NULL,
    [type]          VARCHAR (50)  NULL,
    [types]         VARCHAR (50)  NULL,
    [subtypes]      VARCHAR (50)  NULL,
    [text]          VARCHAR (100) NULL,
    [power]         VARCHAR (50)  NULL,
    [toughness]     VARCHAR (50)  NULL,
    [imageName]     VARCHAR (50)  NULL,
    [colorIdentity] VARCHAR (50)  NULL,
    [multiverseID]  VARCHAR (50)  NULL,
    [expansion]     VARCHAR (50)  NOT NULL,
    [price]         INT           NOT NULL,
    [inventory]     INT           NOT NULL,
    [foilPrice] INT NOT NULL, 
    [foilInventory] INT NOT NULL, 
    CONSTRAINT [PK_MtG] PRIMARY KEY CLUSTERED ([ID] ASC)
);

