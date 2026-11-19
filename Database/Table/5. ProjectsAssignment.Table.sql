USE EnterpriseHRDB;
GO

CREATE TABLE dbo.ProjectAssignments
(
    Id INT IDENTITY(1,1) NOT NULL,
    ProjectId INT NOT NULL,
    EmployeeId INT NOT NULL,
    RoleInProject NVARCHAR(100) NOT NULL,
    AllocationPercentage INT NOT NULL,
    AssignedDate DATETIME NOT NULL CONSTRAINT DF_ProjectAssignments_AssignedDate DEFAULT (GETDATE()),

    CONSTRAINT PK_ProjectAssignments PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_ProjectAssignments_Project_Employee UNIQUE (ProjectId, EmployeeId),
    CONSTRAINT FK_ProjectAssignments_Projects FOREIGN KEY (ProjectId)
        REFERENCES dbo.Projects (Id),
    CONSTRAINT FK_ProjectAssignments_Employees FOREIGN KEY (EmployeeId)
        REFERENCES dbo.Employees (Id),
    CONSTRAINT CK_ProjectAssignments_AllocationPercentage CHECK (AllocationPercentage BETWEEN 1 AND 100)
);
GO