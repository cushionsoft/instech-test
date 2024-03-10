using Claims.Core.Entities;

namespace Claims.Core.Services
{
    public interface IClaimService
    {
        Task<Claim> CreateAsync(Claim claim);
        Task DeleteAsync(string id);
        Task<IEnumerable<Claim>> GetAsync();
        Task<Claim?> GetAsync(string id);
    }
}