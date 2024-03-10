using AutoMapper;
using Claims.Core.Entities;
using Claims.Core.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Claims.Infrastructure.Repositories
{
    public class CoverRepository : ICoverRepository
    {
        private readonly Container _container;
        private readonly IMapper _mapper;

        public CoverRepository(CosmosClient dbClient, IOptions<CosmosDbOptions> cosmosDbOptions, IMapper mapper)
        {
            if (dbClient == null) throw new ArgumentNullException(nameof(dbClient));
            _container = dbClient.GetContainer(cosmosDbOptions.Value.DatabaseName, cosmosDbOptions.Value.CoverContainerName);
            _mapper = mapper;
        }

        public async Task<IEnumerable<Cover>> GetCoversAsync()
        {
            var query = _container.GetItemQueryIterator<Entities.Cover>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Entities.Cover>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return _mapper.Map<IEnumerable<Cover>>(results);
        }

        public async Task<Cover?> GetCoverAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Cover>(id, new PartitionKey(id));
                return _mapper.Map<Cover>(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public Task AddItemAsync(Cover item)
        {
            var cover = _mapper.Map<Entities.Cover>(item);
            return _container.CreateItemAsync(cover, new PartitionKey(item.Id));
        }

        public Task DeleteItemAsync(string id)
        {
            return _container.DeleteItemAsync<Entities.Cover>(id, new PartitionKey(id));
        }
    }
}
