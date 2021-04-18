using Amazon.DynamoDBv2.Model;
using BevCapital.Logon.Application.Errors;
using BevCapital.Logon.Data.Context.Interfaces;
using BevCapital.Logon.Domain.Constants;
using BevCapital.Logon.Domain.Entities;
using BevCapital.Logon.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Repositories
{
    public class AppUserRepositoryAsync : IAppUserRepositoryAsync
    {
        private readonly IDatabaseClient _client;

        public AppUserRepositoryAsync(IDatabaseClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken cancellationToken)
        {
            var request = new ScanRequest
            {
                TableName = DynamoTables.LOGON_APPUSERS,
                ProjectionExpression = "Email, #Name, CreatedAtUtc, UpdatedAtUtc",
                ExpressionAttributeNames = new Dictionary<string, string> {
                    { "#Name", "Name" }
                }
            };

            var itemsRequested = await _client.ScanTableAsync(request, cancellationToken);

            if (itemsRequested.HttpStatusCode != HttpStatusCode.OK)
                throw new AppException(itemsRequested.HttpStatusCode);

            var appUsers = new List<AppUser>();
            foreach (var itemRequested in itemsRequested.Items)
            {
                var epochCreatedAtUtc = long.Parse(itemRequested["CreatedAtUtc"].N);
                var epochUpdatedAtUtc = long.Parse(itemRequested["UpdatedAtUtc"].N);

                appUsers.Add(AppUser.CreateResponse(email: itemRequested["Email"].S,
                                                    name: itemRequested["Name"].S,
                                                    createdAtUtc: DateTimeOffset.FromUnixTimeSeconds(epochCreatedAtUtc).DateTime,
                                                    updatedAtUtc: DateTimeOffset.FromUnixTimeSeconds(epochUpdatedAtUtc).DateTime));
            }

            return appUsers;
        }

        public async Task<AppUser> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var getItemRequest = new GetItemRequest
            {
                TableName = DynamoTables.LOGON_APPUSERS,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Email", new AttributeValue { S = email } }
                },
                ProjectionExpression = "Email, Password, #Name, CreatedAtUtc, UpdatedAtUtc",
                ExpressionAttributeNames = new Dictionary<string, string> {
                    { "#Name", "Name" }
                }
            };

            var itemRequested = await _client.GetAsync(getItemRequest, cancellationToken);

            if (itemRequested.HttpStatusCode != HttpStatusCode.OK)
                throw new AppException(itemRequested.HttpStatusCode);

            if (itemRequested.Item.Count <= 0)
                return null;

            var epochCreatedAtUtc = long.Parse(itemRequested.Item["CreatedAtUtc"].N);
            var epochUpdatedAtUtc = long.Parse(itemRequested.Item["UpdatedAtUtc"].N);

            var appUser = AppUser.CreateResponse(email: itemRequested.Item["Email"].S,
                                                 name: itemRequested.Item["Name"].S,
                                                 createdAtUtc: DateTimeOffset.FromUnixTimeSeconds(epochCreatedAtUtc).DateTime,
                                                 updatedAtUtc: DateTimeOffset.FromUnixTimeSeconds(epochUpdatedAtUtc).DateTime);
            appUser.SetPassword(itemRequested.Item["Password"].S);

            return appUser;
        }

        public async Task CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            var createdAtUtc = new DateTimeOffset(user.CreatedAtUtc).ToUnixTimeSeconds();
            var updatedAtUtc = new DateTimeOffset(user.UpdatedAtUtc).ToUnixTimeSeconds();

            var request = new PutItemRequest
            {
                TableName = DynamoTables.LOGON_APPUSERS,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Email", new AttributeValue { S = user.Email } },
                    { "Name", new AttributeValue { S = user.Name } },
                    { "Password", new AttributeValue { S = user.Password } },
                    { "CreatedAtUtc", new AttributeValue { N = createdAtUtc.ToString() } },
                    { "UpdatedAtUtc", new AttributeValue { N = updatedAtUtc.ToString() } },
                }
            };

            await _client.CreateAsync(request, cancellationToken);
        }

        public async Task UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            var updatedAtUtc = new DateTimeOffset(user.UpdatedAtUtc).ToUnixTimeSeconds();

            var request = new UpdateItemRequest
            {
                TableName = DynamoTables.LOGON_APPUSERS,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Email", new AttributeValue { S = user.Email } }
                },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    { "Name", new AttributeValueUpdate { Value = new AttributeValue(user.Name) } },
                    { "UpdatedAtUtc", new AttributeValueUpdate { Value = new AttributeValue{ N = updatedAtUtc.ToString() } } },
                }
            };

            await _client.UpdateAsync(request, cancellationToken);
        }

        public async Task RemoveAsync(AppUser user, CancellationToken cancellationToken)
        {
            var request = new DeleteItemRequest
            {
                TableName = DynamoTables.LOGON_APPUSERS,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Email", new AttributeValue { S = user.Email } }
                }
            };

            await _client.RemoveAsync(request, cancellationToken);
        }

    }
}
