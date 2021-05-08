using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzEmidsFunction.Data {
    public class TableDBRepository<T> : ITableDBRepository<T> where T : class, new() {
        public async Task<List<PatientItem>> GetAllPatientInformation(CloudTable table, String invocationName)
        { 
            //TableResult x = await table.ExecuteAsync(TableOperation.Retrieve(invocationName, "1"));    
            var query = new TableQuery<PatientItem>();
            var segment = await table.ExecuteQuerySegmentedAsync<PatientItem>(query, null);
            return (List<PatientItem>)segment.Results;
        }

        public async Task<PatientItem> GetAPatientInformation(CloudTable table, String invocationName, string rowKey)
        { 
            string filterCondition = TableQuery.GenerateFilterCondition("EmailId", QueryComparisons.Equal, rowKey.ToLowerInvariant());
            var query = new TableQuery<PatientItem>().Where(filterCondition);
            var segment = await table.ExecuteQuerySegmentedAsync<PatientItem>(query, null);
            return (PatientItem)segment.Results?.FindLast(x => x.EmailId == rowKey);
        }

    }
}