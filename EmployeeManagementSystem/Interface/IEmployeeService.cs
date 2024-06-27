using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.ServiceFilters;

namespace EmployeeManagementSystem.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeBasicDetailsDTO>> GetAllEmployeeBasicDetailsAsync(FilterCriteria filterCriteria);
        Task<EmployeeAdditionalDetailsDTO> GetEmployeeAdditionalDetailsByBasicDetailsUIdAsync(string employeeBasicDetailsUId);
        Task CreateEmployeeBasicDetailsAsync(EmployeeBasicDetailsDTO employeeBasicDetails);
        Task CreateEmployeeAdditionalDetailsAsync(EmployeeAdditionalDetailsDTO employeeAdditionalDetails);
        Task ExportEmployeesToExcelAsync(string filePath);
        Task ImportEmployeesFromExcelAsync(string filePath);
    }
}
