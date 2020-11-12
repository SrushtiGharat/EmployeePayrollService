--Create database--
create database payroll_service
select * from sys.databases where name = 'payroll_service'
use payroll_service

--Create table--
create table employee_payroll
(
id int identity(1,1) not null,
name varchar(25) not null,
salary money not null,
start_date date not null
)

select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'employee_payroll'

--Insert values in table--
insert into employee_payroll values
('Bill',100000.00,'2018-01-03'),
('Terissa',200000.00,'2019-11-13'),
('Charlie',300000.00,'2020-05-21');

--Retrieve all values from table--
select * from employee_payroll

--Retrieve values based on conditions--
select salary from employee_payroll where name = 'Bill'
select * from employee_payroll where start_date between '2018-01-01' and GETDATE();

--Add a gender column to table--
Alter table employee_payroll add Gender char;

Update employee_payroll set gender = 'M' where name = 'Bill' or name = 'Charlie';
Update employee_payroll set gender = 'F' where name = 'Terissa';

Select * from employee_payroll;

--Use SUM,AVG,MAX,MIN,COUNT operations--
select gender,SUM(salary) as sum from employee_payroll  group by gender;
select gender,AVG(salary) as avg from employee_payroll group by gender;
select gender,MIN(salary) as min from employee_payroll group by gender;
select gender,MAX(salary) as max from employee_payroll group by gender;
select gender,COUNT(gender) as count from employee_payroll group by gender;

--Add phone_no,address and department columns--
Alter table employee_payroll add phone varchar(15)
Alter table employee_payroll add department varchar(20)
Alter table employee_payroll add address varchar(100)
Alter table employee_payroll add constraint df_address default 'India' for address

Update employee_payroll set phone = '888888888', department = 'Sales',address = 'Mumbai' where name = 'Bill'
Update employee_payroll set phone = '999999999', department = 'Marketing',address = 'Delhi' where name = 'Terissa'
Update employee_payroll set phone = '777777777', department = 'Finance',address = 'Bangalore' where name = 'Charlie'

Alter table employee_payroll  alter column department varchar(20) not null

--Add BasicPay,Decductions,TaxablePay,IncomeTax and NetPay columns--
sp_rename 'employee_payroll.salary', 'basic_pay';
alter table employee_payroll add deduction money,taxable_pay money,income_tax money,net_pay money;

--Make Terissa part of sales and marketing department--
insert into employee_payroll (name,basic_pay,start_date,gender,phone,department,address) values
('Terissa','200000.00','2019-11-13','F','999999999','Sales','Delhi');

select * from employee_payroll;

--Implement ER Diagram--

/*Create Employee Table*/
Create table Employee
(
EId int identity(1,1) primary key,
EName varchar(20) not null,
Gender char(1) not null,
PhoneNo varchar(15) not null,
Address varchar(50) not null,
StartDate date not null,
)

Insert into Employee values
('Bill','M','9999999999','Mumbai','2018-01-03'),
('Terissa','F','8888888888','Bangalore','2019-05-04'),
('Charlie','M','5555555555','Delhi','2020-02-01');

select * from Employee

/*Create Department Table*/
Create table Department
(
DeptId varchar(5) not null primary key,
DeptName varchar(20) not null
)

Insert into Department values
('D01','Marketing'),
('D02','Sales'),
('D03','Finance');

select * from Department

/*Create Employee_Department Table*/
Create table Employee_Department
(
EmpId int FOREIGN KEY REFERENCES Employee(EId),
DeptId varchar(5) FOREIGN KEY REFERENCES Department(DeptId),
)

Insert into Employee_Department values
(1,'D01'),
(2,'D01'),
(2,'D02'),
(3,'D03');

select * from Employee_Department

/*Create Employee Payroll Table*/
Create table Emp_Payroll
(
EId int not null FOREIGN KEY REFERENCES Employee(EId),
BasicPay money not null,
Deduction money not null,
TaxablePay money not null,
IncomeTax money not null,
NetPay money not null,
)

Insert into Emp_Payroll values
(1,20000,5000,15000,1000,14000),
(2,30000,6000,24000,3000,21000),
(3,40000,10000,30000,5000,25000);

select * from Emp_Payroll

select Gender,SUM(BasicPay) as SUM,
AVG(BasicPay) as AVG, MIN(BasicPay) as MIN,
MAX(BasicPay) as MAX from Employee INNER JOIN Emp_Payroll 
ON Employee.EId = Emp_Payroll.EId GROUP BY Gender;

select BasicPay from Emp_Payroll INNER JOIN Employee ON Emp_Payroll.EId = Employee.EId where EName = 'Bill';

select Employee.EId ,EName,BasicPay,StartDate,Gender,PhoneNo,Address,DeptName,Deduction,TaxablePay,IncomeTax,NetPay from Employee 
INNER JOIN Employee_Department ON Employee.EId = Employee_Department.EmpId
INNER JOIN  Emp_Payroll ON Employee.EId = Emp_Payroll.EId
INNER JOIN Department ON Department.DeptId = Employee_Department.DeptId;
