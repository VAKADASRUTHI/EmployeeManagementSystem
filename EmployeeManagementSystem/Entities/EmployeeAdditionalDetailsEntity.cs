﻿using Newtonsoft.Json;

namespace EmployeeManagementSystem.Entities
{
    public class EmployeeAdditionalDetailsEntity
    {
        [JsonProperty(PropertyName = "employeeBasicDetailsUId", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeBasicDetailsUId { get; set; }

        [JsonProperty(PropertyName = "alternateEmail", NullValueHandling = NullValueHandling.Ignore)]
        public string AlternateEmail { get; set; }

        [JsonProperty(PropertyName = "alternateMobile", NullValueHandling = NullValueHandling.Ignore)]
        public string AlternateMobile { get; set; }

        [JsonProperty(PropertyName = "workInformation", NullValueHandling = NullValueHandling.Ignore)]
        public WorkInfoEntity WorkInformation { get; set; }

        [JsonProperty(PropertyName = "personalDetails", NullValueHandling = NullValueHandling.Ignore)]
        public PersonalDetails_ PersonalDetails { get; set; }

        [JsonProperty(PropertyName = "identityInformation", NullValueHandling = NullValueHandling.Ignore)]
        public IdentityInfoEntity IdentityInformation { get; set; }
    }
}