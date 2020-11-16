using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace RestSharpTest
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client = new RestClient("http://localhost:5000");

        /// <summary>
        /// Get list of employees using json server
        /// </summary>
        /// <returns></returns>
        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);

            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Test method to check the count of the employee list
        /// </summary>
        [TestMethod]
        public void OnCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
           
            Assert.AreEqual(13, dataResponse.Count);
            
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.id + "Name: " + item.name + "Salary: " + item.Salary);
            }
        }

        /// <summary>
        /// Add employee to json server
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Clark");
            jObjectbody.Add("Salary", "15000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual(15000, dataResponse.Salary);
        }

        /// <summary>
        /// Add multiple employees to json server and check count
        /// </summary>
        [TestMethod]
        public void GivenMultipleEmployee_OnPost_ShouldReturn_TotalCount()
        {
            List<Employee> empList = new List<Employee>();
            empList.Add(new Employee { name = "Sarah" ,Salary = 16000});
            empList.Add(new Employee { name = "Peter", Salary = 40000 });

            foreach(Employee employee in empList)
            {
                RestRequest request = new RestRequest("/employees", Method.POST);
                JObject jObjectbody = new JObject();
                jObjectbody.Add("name", employee.name);
                jObjectbody.Add("Salary",employee.Salary);
                request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            }

            IRestResponse emp = getEmployeeList();
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(emp.Content);

            Assert.AreEqual(13, dataResponse.Count);
        }

        /// <summary>
        /// Update employee salary
        /// </summary>
        [TestMethod]
        public void Given_NewSalary_ShouldReturn_UpdateSalaryEmployee()
        {
            RestRequest request = new RestRequest("/employees/3", Method.PUT);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Shiv");
            jObjectBody.Add("Salary", "40000");
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual(3, dataResponse.id);
            Assert.AreEqual(40000, dataResponse.Salary);
        }

        /// <summary>
        /// Delete employee details
        /// </summary>
        [TestMethod]
        public void Given_EmployeeId_WhenDeleted_ShouldReturn_StatusOK()
        {
            RestRequest request = new RestRequest("/employees/16", Method.DELETE);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }
    }
}
