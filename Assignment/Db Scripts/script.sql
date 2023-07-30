CREATE DATABASE Assignment;

GO

CREATE TABLE [Transaction](
	TransactionId NVARCHAR(20) NOT NULL,
	Amount Decimal(18,2) NOT NULL,
	CurrencyCode VARCHAR(3) NOT NULL,
	TransactionDate DATETIME NOT NULL,
	[Status] NVARCHAR(8) NOT NULL,
    CONSTRAINT PK_Transaction PRIMARY KEY (TransactionId)
);