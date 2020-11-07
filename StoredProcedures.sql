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
@StartDate Date,
@EmpId int out
)
as
begin
insert into employee_payroll values
(
@EmployeeName,@BasicPay,@StartDate,@Gender,@PhoneNumber,@Address,@Department,@Deductions,@TaxablePay,@Tax,@NetPay
)
select @EmpId = SCOPE_IDENTITY()
end

create procedure SpUpdateSalary
(
@EmployeeName varchar(255),
@BasicPay money
)
as
begin
update employee_payroll set basic_pay = @BasicPay where name = @EmployeeName

END

create or Alter procedure SpGetEmployeesByStartDateRange
(
@StartDate1 date,
@StartDate2 date
)
AS
BEGIN

select id,name,basic_pay,start_date,gender,phone,address,department,
deduction,taxable_pay,income_tax,net_pay from employee_payroll 
where start_date between @StartDate1 and @StartDate2;

END
