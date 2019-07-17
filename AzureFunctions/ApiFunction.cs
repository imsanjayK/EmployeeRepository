using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AzureFunctions
{
    public static class ApiFunction
    {
        [FunctionName("EmployeesInformation")]
        public static async Task<HttpResponseMessage> ViewEmployees([HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("View Employees : method started");
            var stream = Service.ViewEmployees();
            return Result(stream); ;
        }

        [FunctionName("EmployeeInformationId")]
        public static async Task<HttpResponseMessage> ViewEmployeeId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees/{id}")]HttpRequestMessage req, TraceWriter log, string id)
        {
            log.Info("Employee Information by Id : method started");
            var stream = Service.ViewEmployeeByID(id);
            return Result(stream);
        }

        [FunctionName("RegistorEmployee")]
        public static async Task<HttpResponseMessage> RegistorEmployee([HttpTrigger(AuthorizationLevel.Function,"post", Route = "create")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Registor Employee : method started");
            var request = await req.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(request);
            var responce =  Service.CreateEmployee(employee);
            return Result(responce);
        }

        [FunctionName("EmployeeUpadate")]
        public static async Task<HttpResponseMessage> EmployeeUpadate([HttpTrigger(AuthorizationLevel.Function, "put", Route = "update/{id}")]HttpRequestMessage req, TraceWriter log, string id)
        {
            log.Info("Employee Upadate : method started");
            var request = await req.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(request);
            var stream = Service.UpdateEmployee(employee, id);
            return Result(stream);
        }

        [FunctionName("EmployeeDelete")]
        public static async Task<HttpResponseMessage> EmployeeDelete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "delete/{id}")]HttpRequestMessage req, TraceWriter log, string id)
        {
            log.Info("Employee Delete : method started");
            //var request = await req.Content.ReadAsStringAsync();
            //var employee = JsonConvert.DeserializeObject<Employee>(request);
            var stream = Service.DeleteEmployee(id);
            return Result(stream);
        }

        [FunctionName("EmployeePopulation")]
        public static async Task<HttpResponseMessage> EmployeePopulation([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Total")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Employee Population : method started");
            
            var stream = Service.ViewEmployees();
            var employees = JsonConvert.DeserializeObject<List<Employee>>(stream);
            //Count c = new Count()
            //{
            //    total = employees.Count
            //};
            //return Result(JsonConvert.SerializeObject(c, Formatting.Indented));
           
            var j = new JObject();
            j.Add("Total", employees.Count);
            return Result(j.ToString());
        }

        public static HttpResponseMessage Result(string responce) => new HttpResponseMessage(HttpStatusCode.OK)
            { Content = new StringContent(responce, Encoding.UTF8, "application/json")};
        
    }
}
