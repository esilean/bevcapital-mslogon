using Amazon.DynamoDBv2.Model;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Context.Interfaces
{
    public interface IDatabaseClient
    {
        Task<QueryResponse> QueryTableAsync(QueryRequest queryRequest, CancellationToken cancellationToken);

        Task<ScanResponse> ScanTableAsync(ScanRequest scanRequest, CancellationToken cancellationToken);

        Task<GetItemResponse> GetAsync(GetItemRequest getItemRequest, CancellationToken cancellationToken);

        Task<PutItemResponse> CreateAsync(PutItemRequest document, CancellationToken cancellationToken);

        Task<UpdateItemResponse> UpdateAsync(UpdateItemRequest document, CancellationToken cancellationToken);

        Task<DeleteItemResponse> RemoveAsync(DeleteItemRequest document, CancellationToken cancellationToken);
    }
}
