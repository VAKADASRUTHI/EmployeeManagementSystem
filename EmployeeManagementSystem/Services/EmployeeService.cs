using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagementSystem.Common;
using EmployeeManagementSystem.CosmosDB;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Entities;
using EmployeeManagementSystem.ServiceFilters;
using Microsoft.Azure.Cosmos;
using OfficeOpenXml;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly ICosmosDBService _cosmosDbService;
        private readonly IMapper _mapper;

        public EmployeeService(ICosmosDBService cosmosDbService, IMapper mapper)
        {
            _cosmosDbService = cosmosDbService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeBasicDetailsDTO>> GetAllEmployeeBasicDetailsAsync(FilterCriteria filterCriteria)
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var results = await _cosmosDbService.GetItemsAsync<EmployeeBasicDetailsEntity>(query);

            return _mapper.Map<IEnumerable<EmployeeBasicDetailsDTO>>(results);
        }

        public async Task<EmployeeAdditionalDetailsDTO> GetEmployeeAdditionalDetailsByBasicDetailsUIdAsync(string employeeBasicDetailsUId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.employeeBasicDetailsUId = @employeeBasicDetailsUId")
                .WithParameter("@employeeBasicDetailsUId", employeeBasicDetailsUId);
            var results = await _cosmosDbService.GetItemsAsync<EmployeeAdditionalDetailsEntity>(query);

            return _mapper.Map<EmployeeAdditionalDetailsDTO>(results.FirstOrDefault());
        }

        public async Task CreateEmployeeBasicDetailsAsync(EmployeeBasicDetailsDTO employeeBasicDetails)
        {
            var entity = _mapper.Map<EmployeeBasicDetailsEntity>(employeeBasicDetails);
            await _cosmosDbService.AddItemAsync(entity);
        }

        public async Task CreateEmployeeAdditionalDetailsAsync(EmployeeAdditionalDetailsDTO employeeAdditionalDetails)
        {
            var entity = _mapper.Map<EmployeeAdditionalDetailsEntity>(employeeAdditionalDetails);
            await _cosmosDbService.AddItemAsync(entity);
        }

        public async Task ImportEmployeesFromExcelAsync(string filePath)
        {
            var employees = new List<EmployeeBasicDetailsDTO>();
            var additionalDetails = new List<EmployeeAdditionalDetailsDTO>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var employee = new EmployeeBasicDetailsDTO
                    {
                        FirstName = worksheet.Cells[row, 1].Text,
                        LastName = worksheet.Cells[row, 2].Text,
                        Email = worksheet.Cells[row, 3].Text,
                        Mobile = worksheet.Cells[row, 4].Text,
                        ReportingManagerName = worksheet.Cells[row, 5].Text
                    };
                    var additionalDetail = new EmployeeAdditionalDetailsDTO
                    {
                        PersonalDetails = new PersonalDetailsEntity
                        {
                            DateOfBirth = DateTime.Parse(worksheet.Cells[row, 6].Text)
                        },
                        WorkInformation = new Entities.WorkInfoEntity
                        {
                            DateOfJoining = DateTime.Parse(worksheet.Cells[row, 7].Text)
                        }
                    };
                    employees.Add(employee);
                    additionalDetails.Add(additionalDetail);
                }
            }

            foreach (var employee in employees)
            {
                await CreateEmployeeBasicDetailsAsync(employee);
            }

            foreach (var detail in additionalDetails)
            {
                await CreateEmployeeAdditionalDetailsAsync(detail);
            }
        }

        public async Task ExportEmployeesToExcelAsync(string filePath)
        {
            var basicDetails = await GetAllEmployeeBasicDetailsAsync(new FilterCriteria());
            var additionalDetails = new List<EmployeeAdditionalDetailsDTO>();

            foreach (var basicDetail in basicDetails)
            {
                var additionalDetail = await GetEmployeeAdditionalDetailsByBasicDetailsUIdAsync(basicDetail.EmployeeID);
                if (additionalDetail != null)
                {
                    additionalDetails.Add(additionalDetail);
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employees");
                worksheet.Cells[1, 1].Value = "First Name";
                worksheet.Cells[1, 2].Value = "Last Name";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Mobile";
                worksheet.Cells[1, 5].Value = "Reporting Manager Name";
                worksheet.Cells[1, 6].Value = "Date Of Birth";
                worksheet.Cells[1, 7].Value = "Date of Joining";

                for (int i = 0; i < basicDetails.Count(); i++)
                {
                    var basic = basicDetails.ElementAt(i);
                    var additional = additionalDetails.ElementAtOrDefault(i);

                    worksheet.Cells[i + 2, 1].Value = basic.FirstName;
                    worksheet.Cells[i + 2, 2].Value = basic.LastName;
                    worksheet.Cells[i + 2, 3].Value = basic.Email;
                    worksheet.Cells[i + 2, 4].Value = basic.Mobile;
                    worksheet.Cells[i + 2, 5].Value = basic.ReportingManagerName;
                    worksheet.Cells[i + 2, 6].Value = additional?.PersonalDetails?.DateOfBirth.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 7].Value = additional?.WorkInformation?.DateOfJoining.ToString("yyyy-MM-dd");
                }

                var file = new FileInfo(filePath);
                package.SaveAs(file);
            }
        }

    }
}
    

