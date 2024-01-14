using GscToolTest.GoogleSearchConsole;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GscToolTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GscController : ControllerBase
    {
        private readonly ILogger<GscController> _logger;
        private readonly IGoogleService _googleService;
        //private readonly ICarrierServiceFactory _carrierService;
        //private readonly ShopifySettings _shopifySettings;

        public GscController(ILogger<GscController> logger, IGoogleService googleService)
        {
            _logger = logger;
            _googleService = googleService;
            //_shopifySettings = settings.Value;
        }

        [HttpGet(Name = "GetSomething")]
        public async Task<IActionResult> GetAsync()
        {

            //var sites = await _googleService.ListSitesAsync();

            //var site = _googleService.GetSite("https://www.mobelstudion.se");

            var result = _googleService.IndexResponse();

            //var list = new List<ShopifySharp.Carrier>();

            //var credentials = new ShopifyApiCredentials(_shopifySettings.ShopUrl, _shopifySettings.AccessToken);
            //var service = _carrierService.Create(credentials);

            //var serviceResult = await service.ListAsync();

            //list = serviceResult.ToList();
            //_logger.LogInformation("Listed {0} carriers.", list.Count);

            return Ok(result);
        }
    }
}
