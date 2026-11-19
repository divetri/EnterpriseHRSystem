USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_UpdateEmployee')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_UpdateEmployee
GO

CREATE OR ALTER PROCEDURE dbo.sp_UpdateEmployee
    @Id INT,
    @EmployeeNumber VARCHAR(20),
    @FullName NVARCHAR(255),
    @Email NVARCHAR(150),
    @DepartmentId INT,
    @PositionId INT,
    @ManagerId INT = NULL,
    @Salary DECIMAL(18,2),
    @JoinDate DATETIME,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Employees
    SET EmployeeNumber = @EmployeeNumber,
        FullName = @FullName,
        Email = @Email,
        DepartmentId = @DepartmentId,
        PositionId = @PositionId,
        ManagerId = @ManagerId,
        Salary = @Salary,
        JoinDate = @JoinDate,
        IsActive = @IsActive
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO