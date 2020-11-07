create procedure SpAddEmployeeDetails
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
@StartDate Date
)
as
begin
insert into employee_payroll values
(
@EmployeeName,@BasicPay,@StartDate,@Gender,@PhoneNumber,@Address,@Department,@Deductions,@TaxablePay,@Tax,@NetPay
)
end

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