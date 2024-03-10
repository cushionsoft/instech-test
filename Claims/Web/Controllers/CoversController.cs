using AutoMapper;
using Claims.Core.Enums;
using Claims.Core.Services;
using Claims.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ILogger<CoversController> _logger;
    private readonly ICoverService _coverService;
    private readonly IPremiumCalculatorService _premiumCalculatorService;
    private readonly IMapper _mapper;

    public CoversController(ICoverService coverService, IPremiumCalculatorService premiumCalculatorService, IMapper mapper, ILogger<CoversController> logger)
    {
        _logger = logger;
        _coverService = coverService;
        _premiumCalculatorService = premiumCalculatorService;
        _mapper = mapper;
    }

    [HttpGet("premium")] // change of the endpoint name and type breaks contract, but it fixes swagger and makes it compliant with REST
    public ActionResult ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return Ok(_premiumCalculatorService.ComputePremium(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
    {
        var results = await _coverService.GetAsync();

        return Ok(_mapper.Map<IEnumerable<Cover>>(results));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var result = await _coverService.GetAsync(id);
        if (result == null)
            return NotFound();
        else
            return Ok(_mapper.Map<Cover>(result));
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        var result = await _coverService.CreateAsync(_mapper.Map<Core.Entities.Cover>(cover));
        return Ok(_mapper.Map<Cover>(result));
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(string id)
    {
        await _coverService.DeleteAsync(id);
    }
}