using AutoMapper;
using Claims.Core.Services;
using Claims.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<ClaimsController> _logger;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;

        public ClaimsController(ILogger<ClaimsController> logger, IClaimService claimService, IMapper mapper)
        {
            _logger = logger;
            _claimService = claimService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Claim>> GetAsync()
        {
            var claims = await _claimService.GetAsync();
            return _mapper.Map<IEnumerable<Claim>>(claims);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Claim claim)
        {
            var newClaim = await _claimService.CreateAsync(_mapper.Map<Core.Entities.Claim>(claim));
            return Ok(_mapper.Map<Claim>(newClaim));
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _claimService.DeleteAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetAsync(string id)
        {
            var claim = await _claimService.GetAsync(id);
            if (claim == null)
                return NotFound();
            else
                return Ok(_mapper.Map<Claim>(claim));
        }
    }
}