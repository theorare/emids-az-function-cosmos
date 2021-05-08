using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzEmidsFunction.Data {
    public class TableDBRepository<T> : ITableDBRepository<T> where T : class {
        public async Task<List<PatientItem>> GetAllMessages(CloudTable table, String invocationName)
        { 
            //TableResult x = await table.ExecuteAsync(TableOperation.Retrieve(invocationName, "1"));    
            var query = new TableQuery<PatientItem>();
            var segment = await table.ExecuteQuerySegmentedAsync<PatientItem>(query, null);
            return (List<PatientItem>)segment.Results;
        }

    }
}