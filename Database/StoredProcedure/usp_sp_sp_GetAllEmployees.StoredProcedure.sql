USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_GetAllEmployees')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_GetAllEmployees
GO

CREATE PROCEDURE dbo.sp_GetAllEmployees
    @Search NVARCHAR(255) = NULL,
    @DepartmentId INT = NULL,
    @ActiveOnly BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LikeSearch NVARCHAR(260) =
        CASE
            WHEN @Search IS NULL OR LTRIM(RTRIM(@Search)) = '' THEN NULL
            ELSE N'%' + LTRIM(RTRIM(@Search)) + N'%'
        END;

    SELECT
        e.Id
        , e.EmployeeNumber
        , e.FullName
        , e.Email
        , e.DepartmentId
        , e.PositionId
        , e.ManagerId
        , e.Salary
        , e.JoinDate
        , e.IsActive
        , d.Id
        , d.DepartmentCode
        , d.DepartmentName
        , d.Budget
        , p.Id
        , p.Title
        , p.MinSalary
        , p.MaxSalary
        , m.Id
        , m.EmployeeNumber
        , m.FullName
        , m.Email
        , m.DepartmentId
        , m.PositionId
        , m.ManagerId
        , m.Salary
        , m.JoinDate
        , m.IsActive
    FROM dbo.Employees e
    INNER JOIN dbo.Departments d ON e.DepartmentId = d.Id
    INNER JOIN dbo.Positions p ON e.PositionId = p.Id
    LEFT JOIN dbo.Employees m ON e.ManagerId = m.Id
    WHERE (@ActiveOnly = 0 OR e.IsActive = 1)
      AND (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId)
      AND (@LikeSearch IS NULL OR e.FullName LIKE @LikeSearch OR e.EmployeeNumber LIKE @LikeSearch)
    ORDER BY e.FullName;
END;
GO