using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace AzEmidsFunction.Data {
    public interface ITableDBRepository<T> where T : class, new() {
        Task<List<PatientItem>> GetAllPatientInformation(CloudTable table, String invocationName);
        Task<PatientItem> GetAPatientInformation(CloudTable table, String invocationName, string rowKey);
    }
}