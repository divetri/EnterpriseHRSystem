USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_CreateProject')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_CreateProject
GO

CREATE PROCEDURE dbo.sp_CreateProject
    @ProjectCode VARCHAR(20),
    @ProjectName NVARCHAR(200),
    @DepartmentId INT,
    @StartDate DATE,
    @EndDate DATE = NULL,
    @Status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Projects
        (ProjectCode, ProjectName, DepartmentId, StartDate, EndDate, Status)
    VALUES
        (@ProjectCode, @ProjectName, @DepartmentId, @StartDate, @EndDate, @Status);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO