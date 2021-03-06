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

        public async Task<PatientItem> CreateAPatientInformation(CloudTable table, String invocationName, Patient patient)
        { 
            var patientInformation = new Patient(patient);
            var query = new PatientItem(patient);
            var operation = TableOperation.InsertOrMerge(query);
            var tableResult = await table.ExecuteAsync(operation);
            return (PatientItem)tableResult.Result;
        }

        public async Task<PatientItem> DeleteAPatientInformation(CloudTable table, String invocationName, Patient patient)
        { 
            var patientInformation = new Patient(patient);
            var query = new PatientItem(patient);
            var operation = TableOperation.Retrieve<PatientItem>(query.PartitionKey, query.RowKey);
            var readResult = table.ExecuteAsync(operation).GetAwaiter().GetResult().Result as PatientItem;
            operation = TableOperation.Delete(readResult);
            var deleteResult = await table.ExecuteAsync(operation);
            return (PatientItem)deleteResult.Result;
        }

    }
}