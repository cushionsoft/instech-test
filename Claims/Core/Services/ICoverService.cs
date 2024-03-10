using Claims.Core.Entities;
using Claims.Core.Enums;

namespace Claims.Core.Services
{
    public interface ICoverService
    {
        Task<Cover> CreateAsync(Cover cover);
        Task DeleteAsync(string id);
        Task<IEnumerable<Cover>> GetAsync();
        Task<Cover?> GetAsync(string id);
        decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType);
    }
}