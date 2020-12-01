using EmployeePayrollC_SharpRestAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollRestApiTest
{
    [TestClass]
    public class EmployeePayrollRestApiTestr
    {
        RestClient restClient = new RestClient("http://localhost:3000");
        public IRestResponse GetEmployeeList()
        {
            //Arrange
            RestRequest restRequest = new RestRequest("/employees", Method.GET);
            //Act
            IRestResponse response = restClient.Execute(restRequest);
            //Returning Json result
            return response;
        }
        [TestMethod]
        public void OnCallingEmployeeRestAPI_RetrivesAllData()
        {
            //Act
            IRestResponse restResponse = GetEmployeeList();
            //Assert
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.OK);
            List<EmployeeModel> dataResponse = JsonConvert.DeserializeObject<List<EmployeeModel>>(restResponse.Content);
            Assert.AreEqual(4, dataResponse.Count);
            foreach (var employee in dataResponse)
            {
                Console.WriteLine($"ID: {employee.id}, Name: {employee.name}, Salary: {employee.salary}");
            }
        }
    }
}
