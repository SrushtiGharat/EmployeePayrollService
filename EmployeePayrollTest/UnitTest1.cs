using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeePayrollService;
using System;

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
            employee.EmployeeFirstName = "Manasi";
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
            string name = "Terissa";
            decimal salary = 3000000M;

            bool result = employeeRepo.UpdateSalary(name, salary);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Given_Id_RemoveEmployeeDetails_Should_Return_True()
        {
            int id = 4;

            bool result = employeeRepo.RemoveEmployee(4);

            Assert.AreEqual(true, result);
        }
    }
}
