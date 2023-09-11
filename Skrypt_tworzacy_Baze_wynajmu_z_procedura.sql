CREATE DATABASE [Baza Wynajmu]
GO


CREATE TABLE [Baza Wynajmu].[dbo].[Bilans] (
  [idB] int IDENTITY(1,1) PRIMARY KEY,
  [idM] int,
  [dataTransakcji] datetime,
  [kwota] decimal,
  [kategoria] varchar(255),
  [notka] nvarchar(255)
)
GO

CREATE TABLE [Baza Wynajmu].[dbo].[Klient] (
  [idK] int IDENTITY(1,1) PRIMARY KEY,
  [email] varchar(255),
  [nrKonta] int,
  [imie] nvarchar(255),
  [nazwisko] nvarchar(255)
)
GO

CREATE TABLE [Baza Wynajmu].[dbo].[Wlasciciel] (
  [idW] int IDENTITY(1,1) PRIMARY KEY,
  [email] varchar(255),
  [nrKonta] int,
  [imie] nvarchar(255),
  [nazwisko] nvarchar(255)
)
GO

CREATE TABLE [Baza Wynajmu].[dbo].[DaneMieszkania] (
  [idM] int IDENTITY(1,1) PRIMARY KEY,
  [idW] int,
  [idK] int,
  [Miasto] nvarchar(255),
  [kodPocztowy] varchar(255),
  [Ulica] nvarchar(255),
  [nrBudynku] int,
  [nrMieszkania] int,
  [doWynajecia] bit,
  [doRemontu] bit,
  [kosztaRemontow] decimal,
  [poczatekWynajmu] datetime,
  [koniecWynajmu] datetime
)
GO

CREATE TABLE [Baza Wynajmu].[dbo].[Log] (
  [idL] int IDENTITY(1,1) PRIMARY KEY,
  [email] varchar(255),
  [haslo] nvarchar(255),
  [salt] nvarchar(255),
  [idK] int,
  [idW] int,
  [idA] int
)
GO

CREATE TABLE [Baza Wynajmu].[dbo].[Administrator] (
  [idA] int IDENTITY(1,1) PRIMARY KEY,
  [mail] varchar
)
GO

CREATE TABLE [Baza Wynajmu].[dbo].[Oferta] (
  [idO] int IDENTITY(1,1) PRIMARY KEY,
  [dataWystawienia] datetime,
  [idM] int,
  [opis] nvarchar(255),
  [cenaZaMiesiac] decimal,
  [wyposazenie] nvarchar(255),
  [metraz] float,
  [aktualne] bit
)
GO

CREATE TABLE [Baza Wynajmu].[dbo].[Zainteresowany] (
  [idZ] int IDENTITY(1,1) PRIMARY KEY,
  [idO] int,
  [idK] int,
  [daneKontaktowe] nvarchar(255),
)
GO

ALTER TABLE [Baza Wynajmu].[dbo].[Zainteresowany] ADD FOREIGN KEY ([idO]) REFERENCES [Baza Wynajmu].[dbo].[Oferta] ([idO])
GO

ALTER TABLE [Baza Wynajmu].[dbo].[Zainteresowany] ADD FOREIGN KEY ([idK]) REFERENCES [Baza Wynajmu].[dbo].[Klient] ([idK])
GO

ALTER TABLE [Baza Wynajmu].[dbo].[Log] ADD FOREIGN KEY ([idK]) REFERENCES [Baza Wynajmu].[dbo].[Klient] ([idK])
GO

ALTER TABLE [Baza Wynajmu].[dbo].[Log] ADD FOREIGN KEY ([idW]) REFERENCES [Baza Wynajmu].[dbo].[Wlasciciel] ([idW])
GO

ALTER TABLE [Baza Wynajmu].[dbo].[Log] ADD FOREIGN KEY ([idA]) REFERENCES [Baza Wynajmu].[dbo].[Administrator] ([idA])
GO

ALTER TABLE [Baza Wynajmu].[dbo].[Oferta] ADD FOREIGN KEY ([idM]) REFERENCES [Baza Wynajmu].[dbo].[DaneMieszkania] ([idM])
GO

ALTER TABLE [Baza Wynajmu].[dbo].[Bilans] ADD FOREIGN KEY ([idM]) REFERENCES [Baza Wynajmu].[dbo].[DaneMieszkania] ([idM])
GO

ALTER TABLE [Baza Wynajmu].[dbo].[DaneMieszkania] ADD FOREIGN KEY ([idW]) REFERENCES [Baza Wynajmu].[dbo].[Wlasciciel] ([idW])
GO

ALTER TABLE [Baza Wynajmu].[dbo].[DaneMieszkania] ADD FOREIGN KEY ([idK]) REFERENCES [Baza Wynajmu].[dbo].[Klient] ([idK])
GO

Use [Baza Wynajmu]
GO
CREATE PROCEDURE [wypowiedz_wynajem]
AS
UPDATE [DaneMieszkania]
SET idK = null, doWynajecia = 1, poczatekWynajmu = null, koniecWynajmu = null
WHERE cast(GETDATE() as date) > cast(koniecWynajmu as date);