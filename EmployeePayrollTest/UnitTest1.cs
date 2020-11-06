using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeePayrollService;

namespace EmployeePayrollTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Given_NameAndSalary_UpdateSalary_Should_Return_True()
        {
            EmployeeRepo employeeRepo = new EmployeeRepo();
            string name = "Terissa";
            decimal salary = 3000000M;

            bool result = employeeRepo.UpdateSalary(name, salary);

            Assert.AreEqual(true, result);
        }
    }
}
