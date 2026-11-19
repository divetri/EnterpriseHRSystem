USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_CreateEmployee')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_CreateEmployee
GO

CREATE OR ALTER PROCEDURE dbo.sp_CreateEmployee
    @EmployeeNumber VARCHAR(20),
    @FullName NVARCHAR(255),
    @Email NVARCHAR(150),
    @DepartmentId INT,
    @PositionId INT,
    @ManagerId INT = NULL,
    @Salary DECIMAL(18,2),
    @JoinDate DATETIME,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Employees
        (EmployeeNumber, FullName, Email, DepartmentId, PositionId, ManagerId, Salary, JoinDate, IsActive)
    VALUES
        (@EmployeeNumber, @FullName, @Email, @DepartmentId, @PositionId, @ManagerId, @Salary, @JoinDate, @IsActive);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO