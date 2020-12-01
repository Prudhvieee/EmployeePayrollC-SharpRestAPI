using EmployeePayrollC_SharpRestAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        [TestMethod]
        public void OnAddingEmployee_ShouldReturnAddedEmployee()
        {
            //Arrange
            RestRequest restRequest = new RestRequest("employees/", Method.POST);
            JObject jObject = new JObject();
            jObject.Add("name", "joe");
            jObject.Add("Salary", "10000");
            restRequest.AddParameter("application/json", jObject, ParameterType.RequestBody);
            //Act
            IRestResponse restResponse = restClient.Execute(restRequest);
            //Assert
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.Created);
            EmployeeModel dataResponse = JsonConvert.DeserializeObject<EmployeeModel>(restResponse.Content);
            Assert.AreEqual("joe", dataResponse.name);
            Assert.AreEqual("10000", dataResponse.salary);
        }
        [TestMethod]
        public void GivenMultipleData_OnPost_ShouldReturn_TotalCount()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            employeeList.Add(new EmployeeModel { name = "Bieden", salary = "19000" });
            employeeList.Add(new EmployeeModel { name = "Trump", salary = "11500" });
            employeeList.ForEach(employeeData =>
            {
                RestRequest restRequest = new RestRequest("/employees", Method.POST);
                /// Creating reference of json object
                JObject jObject = new JObject();
                /// Adding the data attribute with data elements
                jObject.Add("name", employeeData.name);
                jObject.Add("Salary", employeeData.salary);

                restRequest.AddParameter("application/json", jObject, ParameterType.RequestBody);
                //Act
                IRestResponse restResponse = restClient.Execute(restRequest);
                //Assert
                Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.Created);
                EmployeeModel dataResponse = JsonConvert.DeserializeObject<EmployeeModel>(restResponse.Content);
                Assert.AreEqual(employeeData.name, dataResponse.name);
                Assert.AreEqual(employeeData.salary, dataResponse.salary);
            });
            IRestResponse response = GetEmployeeList();
            List<EmployeeModel> dataResponse = JsonConvert.DeserializeObject<List<EmployeeModel>>(response.Content);
            Assert.AreEqual(7, dataResponse.Count);
        }
        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            //Arrange
            RestRequest restRequest = new RestRequest("/employees/7", Method.PUT);
            JObject jObject = new JObject();
            jObject.Add("name", "Trump");
            jObject.Add("salary", "13500");
            restRequest.AddParameter("application/json", jObject, ParameterType.RequestBody);
            //Act
            IRestResponse restResponse = restClient.Execute(restRequest);
            //Assert
            Assert.AreEqual(restResponse.StatusCode, System.Net.HttpStatusCode.OK);
            EmployeeModel dataResponse = JsonConvert.DeserializeObject<EmployeeModel>(restResponse.Content);
            Assert.AreEqual("Trump", dataResponse.name);
            Assert.AreEqual("13500", dataResponse.salary);
            System.Console.WriteLine(restResponse.Content);
        }
        [TestMethod]
        public void GivenEmployeeId_OnDelete_ShouldReturnSuccessStatus()
        {
            //Arrange
            RestRequest restRequest = new RestRequest("/employees/5", Method.DELETE);
            //Act
            IRestResponse restResponse = restClient.Execute(restRequest);
            //Assert
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.OK);
            System.Console.WriteLine(restResponse.Content);
        }
    }
}