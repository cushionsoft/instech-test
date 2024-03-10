using Claims.Core.Entities;

namespace Claims.Core.Repositories
{
    public interface ICoverRepository
    {
        Task AddItemAsync(Cover item);
        Task DeleteItemAsync(string id);
        Task<Cover?> GetCoverAsync(string id);
        Task<IEnumerable<Cover>> GetCoversAsync();
    }
}