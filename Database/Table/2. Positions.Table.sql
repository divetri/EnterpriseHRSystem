USE EnterpriseHRDB;
GO

CREATE TABLE Positions
(
    Id INT IDENTITY(1,1) NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    MinSalary DECIMAL(18,2) NOT NULL,
    MaxSalary DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_Positions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_Positions_SalaryRange CHECK (MinSalary <= MaxSalary)
);