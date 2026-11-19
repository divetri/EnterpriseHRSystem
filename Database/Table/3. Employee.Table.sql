USE EnterpriseHRDB;
GO

CREATE TABLE Employees
(
    Id INT IDENTITY(1,1) NOT NULL,
    EmployeeNumber VARCHAR(20) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    DepartmentId INT NOT NULL,
    PositionId INT NOT NULL,
    ManagerId INT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    JoinDate DATETIME NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Employees_IsActive DEFAULT (1),

    CONSTRAINT PK_Employees PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Employees_EmployeeNumber UNIQUE (EmployeeNumber),
    CONSTRAINT UQ_Employees_Email UNIQUE (Email),
    CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId)
        REFERENCES Departments (Id),
    CONSTRAINT FK_Employees_Positions FOREIGN KEY (PositionId)
        REFERENCES Positions (Id),
    CONSTRAINT FK_Employees_Manager FOREIGN KEY (ManagerId)
        REFERENCES Employees (Id)
);