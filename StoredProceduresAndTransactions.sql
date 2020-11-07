--Insert values in the table--
create or alter procedure SpAddEmployeeDetails
(
@EmployeeName varchar(255),
@PhoneNumber varchar(255),
@Address varchar(255),
@Department varchar(255),
@Gender char(1),
@BasicPay float,
@Deductions float,
@TaxablePay float,
@Tax float,
@NetPay float,
@StartDate Date,
@EmpId int out
)
as
begin
set XACT_ABORT on;
begin try
begin TRANSACTION;
insert into Employee values
(
@EmployeeName,@Gender,@PhoneNumber,@Address,@StartDate
)
set @EmpId = SCOPE_IDENTITY()
insert into Emp_Payroll values
(
@EmpId,@BasicPay,@Deductions,@TaxablePay,@Tax,@NetPay
)
insert into Employee_Department values
(
@EmpId , (select DeptId from Department where DeptName = @Department)
)

COMMIT TRANSACTION;
END TRY
BEGIN CATCH
select ERROR_NUMBER() AS ErrorNumber, ERROR_MESSAGE() AS ErrorMessage;
IF(XACT_STATE())=-1
BEGIN
  PRINT N'The transaction is in an uncommitable state.'+'Rolling back transaction.'
  ROLLBACK TRANSACTION;
  END;

  IF(XACT_STATE())=1
  BEGIN
    PRINT 
	    N'The transaction is committable. '+'Committing transaction.'
       COMMIT TRANSACTION;
	END;
	END CATCH
END

--Remove employee--
Create or alter procedure SpRemoveEmployeeData
(
@EmpId int
)
as
begin
set XACT_ABORT on;
begin try
begin TRANSACTION;

Delete from Emp_Payroll where EId = @EmpId;
Delete from Employee_Department where EmpId = @EmpId
Delete from Employee where EId = @EmpId;

COMMIT TRANSACTION;
END TRY
BEGIN CATCH
select ERROR_NUMBER() AS ErrorNumber, ERROR_MESSAGE() AS ErrorMessage;
IF(XACT_STATE())=-1
BEGIN
  PRINT N'The transaction is in an uncommitable state.'+'Rolling back transaction.'
  ROLLBACK TRANSACTION;
  END;

  IF(XACT_STATE())=1
  BEGIN
    PRINT 
	    N'The transaction is committable. '+'Committing transaction.'
       COMMIT TRANSACTION;
	END;
	END CATCH
END

--Update Salary in table--
create or ALTER procedure SpUpdateSalary
(
@EmployeeName varchar(255),
@BasicPay money
)
AS
BEGIN
update Emp_Payroll
set BasicPay = @BasicPay 
from Emp_Payroll Inner Join Employee on
Emp_Payroll.EId = Employee.EId where Employee.EName = @EmployeeName

END

--Get Employees By Start Date Range--
create or Alter procedure SpGetEmployeesByStartDateRange
(
@StartDate1 date,
@StartDate2 date
)
AS
BEGIN

select Employee.EId ,EName,BasicPay,StartDate,Gender,PhoneNo,Address,DeptName,Deduction,TaxablePay,IncomeTax,NetPay
from Employee INNER JOIN Employee_Department ON Employee.EId = Employee_Department.EmpId
INNER JOIN  Emp_Payroll ON Employee.EId = Emp_Payroll.EId
INNER JOIN Department ON Department.DeptId = Employee_Department.DeptId
where StartDate between @StartDate1 and @StartDate2;

END