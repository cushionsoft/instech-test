using AutoMapper;
using Claims.Core.Entities;
using Claims.Core.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Claims.Infrastructure.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly Container _container;
        private readonly IMapper _mapper;

        public ClaimRepository(CosmosClient dbClient, IMapper mapper, IOptions<CosmosDbOptions> cosmosDbOptions)
        {
            if (dbClient == null) throw new ArgumentNullException(nameof(dbClient));
            _container = dbClient.GetContainer(cosmosDbOptions.Value.DatabaseName, cosmosDbOptions.Value.ClaimContainerName);
            _mapper = mapper;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            var query = _container.GetItemQueryIterator<Entities.Claim>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Entities.Claim>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return _mapper.Map<IEnumerable<Claim>>(results);
        }

        public async Task<Claim?> GetClaimAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Entities.Claim>(id, new PartitionKey(id));
                return _mapper.Map<Claim>(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddItemAsync(Claim item)
        {
            var claim = _mapper.Map<Entities.Claim>(item);
            await _container.CreateItemAsync(claim, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<Entities.Claim>(id, new PartitionKey(id));
        }
    }
}
