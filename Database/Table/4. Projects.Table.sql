USE EnterpriseHRDB;
GO

CREATE TABLE dbo.Projects
(
    Id INT IDENTITY(1,1) NOT NULL,
    ProjectCode VARCHAR(20) NOT NULL,
    ProjectName NVARCHAR(200) NOT NULL,
    DepartmentId INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    Status VARCHAR(20) NOT NULL,

    CONSTRAINT PK_Projects PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Projects_ProjectCode UNIQUE (ProjectCode),
    CONSTRAINT FK_Projects_Departments FOREIGN KEY (DepartmentId)
        REFERENCES dbo.Departments (Id),
    CONSTRAINT CK_Projects_Status CHECK (Status IN ('Planning', 'Active', 'Completed', 'Cancelled'))
);
GO