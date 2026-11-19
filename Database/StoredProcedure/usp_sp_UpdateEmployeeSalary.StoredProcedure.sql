USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_UpdateEmployeeSalary')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_UpdateEmployeeSalary
GO

USE EnterpriseHRDB;
GO

CREATE OR ALTER PROCEDURE dbo.sp_UpdateEmployeeSalary
    @EmployeeId INT,
    @NewSalary DECIMAL(18,2),
    @NewPositionId INT = NULL,
    @Reason NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @OldSalary DECIMAL(18,2);
    DECLARE @CurrentPositionId INT;
    DECLARE @TargetPositionId INT;
    DECLARE @MinSalary DECIMAL(18,2);
    DECLARE @MaxSalary DECIMAL(18,2);
    DECLARE @PositionTitle NVARCHAR(100);

    SELECT
        @OldSalary = Salary,
        @CurrentPositionId = PositionId
    FROM dbo.Employees
    WHERE Id = @EmployeeId;

    IF @@ROWCOUNT = 0
        THROW 50001, 'Employee not found.', 1;

    SET @TargetPositionId = COALESCE(@NewPositionId, @CurrentPositionId);

    SELECT
        @MinSalary = MinSalary,
        @MaxSalary = MaxSalary,
        @PositionTitle = Title
    FROM dbo.Positions
    WHERE Id = @TargetPositionId;

    IF @@ROWCOUNT = 0
        THROW 50002, 'Target position not found.', 1;

    IF @NewSalary < @MinSalary OR @NewSalary > @MaxSalary
        THROW 50003, 'New salary is outside the allowed salary range for the target position.', 1;

    BEGIN TRANSACTION;

    UPDATE dbo.Employees
    SET Salary = @NewSalary,
        PositionId = @TargetPositionId
    WHERE Id = @EmployeeId;

    INSERT INTO dbo.SalaryHistories
        (EmployeeId, OldSalary, NewSalary, ChangeReason)
    VALUES
        (@EmployeeId, @OldSalary, @NewSalary, @Reason);

    COMMIT TRANSACTION;
END;
GO