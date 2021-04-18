using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using BevCapital.Logon.Data.Context.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Context
{
    public class DatabaseClient : IDatabaseClient
    {
        private readonly IAmazonDynamoDB _client;

        public DatabaseClient(IAmazonDynamoDB client)
        {
            _client = client;
        }

        /// <summary>
        /// Get all the records from the given table
        /// </summary>
        /// <param name="scanRequest">ItemRequest value</param>
        /// <returns></returns>
        public async Task<QueryResponse> QueryTableAsync(QueryRequest scanRequest, CancellationToken cancellationToken)
        {
            return await _client.QueryAsync(scanRequest, cancellationToken);
        }

        /// <summary>
        /// Get all the records from the given table
        /// </summary>
        /// <param name="scanRequest">ItemRequest value</param>
        /// <returns></returns>
        public async Task<ScanResponse> ScanTableAsync(ScanRequest scanRequest, CancellationToken cancellationToken)
        {
            return await _client.ScanAsync(scanRequest, cancellationToken);
        }

        /// <summary>
        /// Gets a record by itemRequset
        /// </summary>
        /// <param name="getItemRequest">ItemRequest value</param>
        /// <returns></returns>
        public async Task<GetItemResponse> GetAsync(GetItemRequest getItemRequest, CancellationToken cancellationToken)
        {
            return await _client.GetItemAsync(getItemRequest, cancellationToken);
        }

        /// <summary>
        /// Creates the given record in the table
        /// </summary>
        /// <param name="document">Record to save in the table</param>
        /// <returns></returns>
        public async Task<PutItemResponse> CreateAsync(PutItemRequest document, CancellationToken cancellationToken)
        {
            return await _client.PutItemAsync(document, cancellationToken);
        }

        /// <summary>
        /// Updates the given record in the table
        /// </summary>
        /// <param name="document">Record to save in the table</param>
        /// <returns></returns>
        public async Task<UpdateItemResponse> UpdateAsync(UpdateItemRequest document, CancellationToken cancellationToken)
        {
            return await _client.UpdateItemAsync(document, cancellationToken);
        }

        /// <summary>
        /// Deletes the given record in the table
        /// </summary>
        /// <param name="document">Record to be removed from the table</param>
        /// <returns></returns>
        public async Task<DeleteItemResponse> RemoveAsync(DeleteItemRequest document, CancellationToken cancellationToken)
        {
            return await _client.DeleteItemAsync(document, cancellationToken);
        }

    }
}
