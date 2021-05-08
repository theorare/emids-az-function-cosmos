using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace AzEmidsFunction.Data {
    public interface ITableDBRepository<T> where T : class {
        Task<List<PatientItem>> GetAllMessages(CloudTable table, String invocationName);
    }
}