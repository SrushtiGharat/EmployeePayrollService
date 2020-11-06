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

create procedure SpUpdateSalary
(
@EmployeeName varchar(255),
@BasicPay money
)
as
begin
update employee_payroll set basic_pay = @BasicPay where name = @EmployeeName

END
