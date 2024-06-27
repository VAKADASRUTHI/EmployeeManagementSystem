using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Services;
using EmployeeManagementSystem.Common;
using EmployeeManagementSystem.ServiceFilters;
using System.Text.Json;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly HttpClient _httpClient;

        public EmployeeController(IEmployeeService employeeService, HttpClient httpClient)
        {
            _employeeService = employeeService;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IEnumerable<EmployeeBasicDetailsDTO>> GetAllEmployeeBasicDetails([FromQuery] FilterCriteria filterCriteria)
        {
            return await _employeeService.GetAllEmployeeBasicDetailsAsync(filterCriteria);
        }

        [HttpGet("{employeeBasicDetailsUId}")]
        public async Task<EmployeeAdditionalDetailsDTO> GetEmployeeAdditionalDetailsByBasicDetailsUId(string employeeBasicDetailsUId)
        {
            return await _employeeService.GetEmployeeAdditionalDetailsByBasicDetailsUIdAsync(employeeBasicDetailsUId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeBasicDetails([FromBody] EmployeeBasicDetailsDTO employeeBasicDetails)
        {
            await _employeeService.CreateEmployeeBasicDetailsAsync(employeeBasicDetails);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAdditionalDetails([FromBody] EmployeeAdditionalDetailsDTO employeeAdditionalDetails)
        {
            await _employeeService.CreateEmployeeAdditionalDetailsAsync(employeeAdditionalDetails);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ImportEmployeesFromExcel([FromForm] IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _employeeService.ImportEmployeesFromExcelAsync(filePath);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ExportEmployeesToExcel()
        {
            var filePath = Path.GetTempFileName();
            await _employeeService.ExportEmployeesToExcelAsync(filePath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> MakePostRequestExample([FromBody] object payload)
        {
            var url = "https://localhost:7000/api/Employee";
            var content = JsonSerializer.Serialize(payload);
            var response = await HttpClientHelper.MakePostRequest(_httpClient, url, content);
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpGet]
        public async Task<IActionResult> MakeGetRequestExample(string queryParam)
        {
            var url = $"https://localhost:7000/api/Employee={queryParam}";
            var response = await HttpClientHelper.MakeGetRequest(_httpClient, url);
            return Ok(await response.Content.ReadAsStringAsync());
        }
    }

}

