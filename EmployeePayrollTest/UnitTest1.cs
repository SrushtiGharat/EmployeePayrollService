using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeePayrollService;
using System;
using System.Collections.Generic;

namespace EmployeePayrollTest
{
    [TestClass]
    public class UnitTest1
    {
        EmployeeRepo employeeRepo = new EmployeeRepo();
        [TestMethod]
        public void Given_NewEmployeeWhenAdded_Should_SyncWithDB()
        {
            EmployeeModel employee = new EmployeeModel();
            employee.EmployeeName = "Manasi";
            employee.Department = "Sales";
            employee.PhoneNumber = "4444444444";
            employee.Address = "03-Pancham Society";
            employee.Gender = Convert.ToChar("F");
            employee.StartDate = DateTime.Today;
            employee.BasicPay = Convert.ToDecimal(200000);
            employee.Deductions = 0.2M * employee.BasicPay;
            employee.TaxablePay = employee.BasicPay - employee.Deductions;
            employee.Tax = 0.1M * employee.TaxablePay;
            employee.NetPay = employee.BasicPay - employee.Tax;

            bool result = employeeRepo.AddEmployee(employee);
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void Given_NameAndSalary_UpdateSalary_Should_Return_True()
        {
            int id = 2;
            decimal salary = 3000000M;

            bool result = employeeRepo.UpdateSalary(id, salary);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Given_Id_RemoveEmployeeDetails_Should_Return_True()
        {
            int id = 4;

            bool result = employeeRepo.RemoveEmployee(id);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Given_MultipleEmployees_WhenAdded_SholdReturn_Count()
        {
            List<EmployeeModel> empList = new List<EmployeeModel>();
            
            EmployeeModel employee1 = new EmployeeModel();
            employee1.EmployeeName = "Shreya";
            employee1.Department = "Finance";
            employee1.PhoneNumber = "9823439977";
            employee1.Address = "Delhi";
            employee1.Gender = Convert.ToChar("F");
            employee1.StartDate = DateTime.Today;
            employee1.BasicPay = Convert.ToDecimal(100000);
            employee1.Deductions = 0.2M * employee1.BasicPay;
            employee1.TaxablePay = employee1.BasicPay - employee1.Deductions;
            employee1.Tax = 0.1M * employee1.TaxablePay;
            employee1.NetPay = employee1.BasicPay - employee1.Tax;
            empList.Add(employee1);

            EmployeeModel employee2 = new EmployeeModel();
            employee2.EmployeeName = "Raghu";
            employee2.Department = "Marketing";
            employee2.PhoneNumber = "7028827730";
            employee2.Address = "Hyderabad";
            employee2.Gender = Convert.ToChar("M");
            employee2.StartDate = DateTime.Today;
            employee2.BasicPay = Convert.ToDecimal(300000);
            employee2.Deductions = 0.2M * employee2.BasicPay;
            employee2.TaxablePay = employee2.BasicPay - employee2.Deductions;
            employee2.Tax = 0.1M * employee2.TaxablePay;
            employee2.NetPay = employee2.BasicPay - employee2.Tax;
            empList.Add(employee2);

            DateTime startTime = DateTime.Now;
            int result = employeeRepo.AddMultipleEmployeesUsingThread(empList);
            DateTime stopTime = DateTime.Now;
            Console.WriteLine("Duration without thread :" + (stopTime - startTime));
            Assert.AreEqual(2, result);
        }
    }
}
