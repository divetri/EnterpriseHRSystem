USE EnterpriseHRDB;
GO

CREATE TABLE dbo.SalaryHistories
(
    Id INT IDENTITY(1,1) NOT NULL,
    EmployeeId INT NOT NULL,
    OldSalary DECIMAL(18,2) NOT NULL,
    NewSalary DECIMAL(18,2) NOT NULL,
    ChangeReason NVARCHAR(255) NOT NULL,
    ChangedAt DATETIME NOT NULL CONSTRAINT DF_SalaryHistories_ChangedAt DEFAULT (GETDATE()),

    CONSTRAINT PK_SalaryHistories PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_SalaryHistories_Employees FOREIGN KEY (EmployeeId)
        REFERENCES dbo.Employees (Id)
);
GO