USE EnterpriseHRDB;
GO

CREATE TABLE Departments
(
    Id INT IDENTITY(1,1) NOT NULL,
    DepartmentCode VARCHAR(10) NOT NULL,
    DepartmentName NVARCHAR(100) NOT NULL,
    Budget DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_Departments PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Departments_DepartmentCode UNIQUE (DepartmentCode)
);