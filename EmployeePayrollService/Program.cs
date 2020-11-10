using System;
using System.Collections.Generic;
namespace EmployeePayrollService
{
    class Program
    {
        static void Main(string[] args)
        {
            List<EmployeeModel> empList = new List<EmployeeModel>();

            Console.WriteLine("Welcome to Employee Payroll Service");
            EmployeeRepo employeeRepo = new EmployeeRepo();
            bool result;
            Console.WriteLine("1.Get all values\n2.Add employees\n3.Update salary\n4.Get employees joined within a date range\n" +
                "5.Get Aggregate Salary Details By Gender\n6.Remove Employee");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1: employeeRepo.GetAllEmployee();
                        break;
                case 2:
                    while (true)
                    {
                        EmployeeModel e = new EmployeeModel();
                        
                        Console.WriteLine("Enter Name");
                        e.EmployeeName = Console.ReadLine();
                        Console.WriteLine("Enter Address");
                        e.Address = Console.ReadLine();
                        Console.WriteLine("Enter PhoneNo");
                        e.PhoneNumber= Console.ReadLine();
                        Console.WriteLine("Enter Gender");
                        e.Gender = Convert.ToChar(Console.ReadLine());
                        Console.WriteLine("Enter Department");
                        e.Department = Console.ReadLine();
                        Console.WriteLine("Enter Start Date");
                        e.StartDate = Convert.ToDateTime(Console.ReadLine());
                        Console.WriteLine("Enter Basic Pay");
                        e.BasicPay = Convert.ToDecimal(Console.ReadLine());
                        e.Deductions = 0.2M * e.BasicPay;
                        e.TaxablePay = e.BasicPay - e.Deductions;
                        e.Tax = 0.1M * e.TaxablePay;
                        e.NetPay = e.BasicPay - e.Tax;
                        empList.Add(e);

                        Console.WriteLine("Do you want to add more contacts ? Yes / No");
                        string ans = Console.ReadLine();
                        if (ans.ToUpper() == "NO")
                            break;
                    }
                    employeeRepo.AddMultipleEmployees(empList);
                    break;

                case 3:
                    Console.WriteLine("Enter Employee Id");
                    int empid = Convert.ToInt32("Console.ReadLine()");
                    Console.WriteLine("Enter new salary");
                    decimal salary = Convert.ToDecimal(Console.ReadLine());

                    result = employeeRepo.UpdateSalary(empid, salary);
                    if(result == false)
                    {
                        Console.WriteLine("Salary update not successfull");
                        break;
                    }
                    Console.WriteLine("Salary updated successfully");
                    break;
                case 4:
                    Console.WriteLine("Enter Dates");
                    string[] dates = Console.ReadLine().Split(",");

                    employeeRepo.GetEmployeesGivenDateRange(Convert.ToDateTime(dates[0]), Convert.ToDateTime(dates[1]));
                    break;
                case 5:
                    employeeRepo.GetAggregateSalaryDetailsByGender();
                    break;
                case 6:
                    Console.WriteLine("Enter Employee Id");
                    int id = Convert.ToInt32("Console.ReadLine()");
                    employeeRepo.RemoveEmployee(id);
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }

        }
    }
}
