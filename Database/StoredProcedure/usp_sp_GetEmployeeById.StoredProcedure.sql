USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_GetEmployeeById')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_GetEmployeeById
GO

USE EnterpriseHRDB;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetEmployeeById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id, EmployeeNumber, FullName, Email, DepartmentId,
        PositionId, ManagerId, Salary, JoinDate, IsActive
    FROM dbo.Employees
    WHERE Id = @Id;
END;
GO