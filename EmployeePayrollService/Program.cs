using System;

namespace EmployeePayrollService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Employee Payroll Service");
            EmployeeRepo employeeRepo = new EmployeeRepo();

            EmployeeModel employee = new EmployeeModel();
            employee.EmployeeFirstName = "Manasi";
            employee.Department = "Tech";
            employee.PhoneNumber = "5555555555";
            employee.Address = "02-Nerul";
            employee.Gender = 'F';
            employee.BasicPay = 10000.00M;
            employee.Deductions = 1500.000M;
            employee.StartDate = employee.StartDate = Convert.ToDateTime("2020-11-03");

            if (employeeRepo.AddEmployee(employee))
                Console.WriteLine("Records added successfully");

            employeeRepo.GetAllEmployee();

        }
    }
}
