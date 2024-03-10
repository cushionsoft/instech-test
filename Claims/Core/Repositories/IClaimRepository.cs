using Claims.Core.Entities;

namespace Claims.Core.Repositories
{
    public interface IClaimRepository
    {
        Task AddItemAsync(Claim item);
        Task DeleteItemAsync(string id);
        Task<Claim?> GetClaimAsync(string id);
        Task<IEnumerable<Claim>> GetClaimsAsync();
    }
}