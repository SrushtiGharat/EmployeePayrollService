using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
           
            Assert.AreEqual(10, dataResponse.Count);
            
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.id + "Name: " + item.name + "Salary: " + item.Salary);
            }
        }
    }
}
