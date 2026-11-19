USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_AssignEmployeeToProject')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_AssignEmployeeToProject
GO

CREATE PROCEDURE dbo.sp_AssignEmployeeToProject
    @ProjectId INT,
    @EmployeeId INT,
    @RoleInProject NVARCHAR(100),
    @AllocationPercentage INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @AllocationPercentage < 1 OR @AllocationPercentage > 100
        THROW 50010, 'Allocation percentage must be between 1 and 100.', 1;

    DECLARE @EmployeeActive BIT;
    SELECT @EmployeeActive = IsActive
    FROM dbo.Employees
    WHERE Id = @EmployeeId;

    IF @EmployeeActive IS NULL
        THROW 50011, 'Employee not found.', 1;

    IF @EmployeeActive = 0
        THROW 50012, 'Inactive employees cannot be assigned to a project.', 1;

    DECLARE @ProjectStatus VARCHAR(20);
    SELECT @ProjectStatus = Status
    FROM dbo.Projects
    WHERE Id = @ProjectId;

    IF @ProjectStatus IS NULL
        THROW 50013, 'Project not found.', 1;

    IF @ProjectStatus NOT IN ('Active', 'Planning')
        THROW 50014, 'Employees can only be assigned to projects with status Active or Planning.', 1;

    IF EXISTS (
        SELECT 1
        FROM dbo.ProjectAssignments
        WHERE ProjectId = @ProjectId AND EmployeeId = @EmployeeId
    )
        THROW 50015, 'Employee is already assigned to this project.', 1;

    DECLARE @CurrentAllocation INT = 0;
    SELECT @CurrentAllocation = COALESCE(SUM(pa.AllocationPercentage), 0)
    FROM dbo.ProjectAssignments pa
    INNER JOIN dbo.Projects p ON pa.ProjectId = p.Id
    WHERE pa.EmployeeId = @EmployeeId
      AND p.Status IN ('Active', 'Planning');

    IF (@CurrentAllocation + @AllocationPercentage) > 100
        THROW 50016, 'Total allocation across Active or Planning projects would exceed 100%.', 1;

    BEGIN TRANSACTION;

    INSERT INTO dbo.ProjectAssignments
        (ProjectId, EmployeeId, RoleInProject, AllocationPercentage)
    VALUES
        (@ProjectId, @EmployeeId, @RoleInProject, @AllocationPercentage);

    COMMIT TRANSACTION;
END;
GO