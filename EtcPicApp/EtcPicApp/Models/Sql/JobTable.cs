using System;
using EtcPicApp.Models.Clients;

namespace EtcPicApp.Models.Sql
{
    public class JobsTable
    {
        public int JobId { get; set; }
        public string Client { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public int ServiceId { get; set; }

        public static Jobs.Jobs ToModel(JobsTable jobsTable)
        {
            return new Jobs.Jobs
            {
                Client = new Client { Id = 0, Name = jobsTable.Client },
                FacilityAddress = jobsTable.FacilityAddress,
                FacilityName = jobsTable.FacilityName,
                JobId = jobsTable.JobId,
                ServiceId = jobsTable.ServiceId
            };
        }
    }
}