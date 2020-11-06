﻿using System;

namespace EmployeePayrollService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Employee Payroll Service");
            EmployeeRepo employeeRepo = new EmployeeRepo();

            Console.WriteLine("1.Get all values\n2.Insert value\n3.Update salary");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1: employeeRepo.GetAllEmployee();
                        break;
                case 2:Console.WriteLine("Enter Name ,Department, Phone no, Address, Gender, BasicPay, Deduction, Taxable Pay, Tax, NetPay ");
                    string[] details = Console.ReadLine().Split(",");

                    EmployeeModel employee = new EmployeeModel();
                    employee.EmployeeFirstName = details[0];
                    employee.Department = details[1];
                    employee.PhoneNumber = details[2];
                    employee.Address = details[3];
                    employee.Gender = Convert.ToChar(details[4]);
                    employee.StartDate = DateTime.Today;
                    employee.BasicPay = Convert.ToDecimal(details[5]);
                    employee.Deductions = Convert.ToDecimal(details[6]);
                    employee.TaxablePay = Convert.ToDecimal(details[7]);
                    employee.Tax = Convert.ToDecimal(details[8]);
                    employee.NetPay = Convert.ToDecimal(details[5]);

                    employeeRepo.AddEmployee(employee);

                    Console.WriteLine("Records added successfully");
                    break;

                case 3:Console.WriteLine("Enter employee name");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter new salary");
                    decimal salary = Convert.ToDecimal(Console.ReadLine());

                    employeeRepo.UpdateSalary(name, salary);
                    Console.WriteLine("Salary updated successfully");
                    break;
            }

        }
    }
}
