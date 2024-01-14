using Google.Apis.SearchConsole.v1;
using Google.Apis.SearchConsole.v1.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using static Google.Apis.SearchConsole.v1.UrlInspectionResource.IndexResource;

namespace GscToolTest.GoogleSearchConsole
{
    public interface IGoogleService
    {
        Task<SitesListResponse> ListSitesAsync();
        SitesListResponse ListSites();
        WmxSite GetSite(string siteUrl);
        //Task<InspectUrlIndexResponse> IndexResponseAsync();
        InspectUrlIndexResponse IndexResponse();
        //List<Product> ListFeedProducts(string feedLabel);
        //bool InsertProduct(Product product);
        //bool DeleteProduct(Product product);
    }

    public class GoogleService : IGoogleService
    {
        private readonly ILogger<GoogleService> _logger;
        private readonly GoogleSettings _googleSettings;
        private readonly IGoogleUtil _googleUtil;
        private readonly SearchConsoleService _searchConsoleService;
        IEnumerable<HttpStatusCode> _retryCodes = new HttpStatusCode[] { HttpStatusCode.NotFound };

        public GoogleService(ILogger<GoogleService> logger, IOptions<GoogleSettings> googleSettings, IGoogleUtil googleUtil, ISearchConsoleServiceFactory searchConsoleServiceFactory) //, IShoppingUtil shoppingUtil, IShoppingContentServiceFactory shoppingContentServiceFactory
        {
            _logger = logger;

            _googleSettings = googleSettings.Value;
            _googleUtil = googleUtil;
            _searchConsoleService = searchConsoleServiceFactory.Create();
        }

        public SitesListResponse ListSites()
        {
            var request = _searchConsoleService.Sites.List();
            var sitesList = _googleUtil.ExecuteWithRetries(request, _retryCodes);
            FileExtensions.CreateJsonFile("Sites", JsonConvert.SerializeObject(sitesList));

            return sitesList;
        }

        public async Task<SitesListResponse> ListSitesAsync()
        {
            var request = _searchConsoleService.Sites.List();
            var sitesList = await _googleUtil.ExecuteWithRetriesAsync(request, _retryCodes);
            FileExtensions.CreateJsonFile("Sites", JsonConvert.SerializeObject(sitesList));

            return sitesList;
        }

        public WmxSite GetSite(string siteUrl)
        {
            var request = _searchConsoleService.Sites.Get(siteUrl);
            var wmxSite = _googleUtil.ExecuteWithRetries(request, _retryCodes);
            FileExtensions.CreateJsonFile("Site", JsonConvert.SerializeObject(wmxSite));

            return wmxSite;
        }

        public InspectUrlIndexResponse IndexResponse()
        {
            var inspectUrlIndexRequest = new InspectUrlIndexRequest();
            inspectUrlIndexRequest.SiteUrl = "https://www.mobelstudion.se/";
            inspectUrlIndexRequest.InspectionUrl = "https://www.mobelstudion.se/pages/tapetserarverkstad";
            //inspectUrlIndexRequest.LanguageCode = "sv-SE";

            var inspectRequest = new InspectRequest(_searchConsoleService, inspectUrlIndexRequest);

            var request = _searchConsoleService.UrlInspection.Index.Inspect(inspectUrlIndexRequest);
            var inspectUrlIndexResponse = _googleUtil.ExecuteWithRetries(request, _retryCodes);
            FileExtensions.CreateJsonFile("IndexResponse", JsonConvert.SerializeObject(inspectUrlIndexResponse));

            return inspectUrlIndexResponse;
        }

        //public void GetSiteX(string siteUrl)
        //{
        //    var request = _searchConsoleService.Searchanalytics..Sites.Get(siteUrl);
        //    var wmxSite = _googleUtil.ExecuteWithRetries(request, _retryCodes);
        //    FileExtensions.CreateJsonFile("Site", JsonConvert.SerializeObject(wmxSite));

        //    return wmxSite;
        //}

        //    public List<Product> ListFeedProducts(string feedLabel)
        //    {
        //        var products = new List<Product>();

        //        var listRequest = _shoppingContentService.Products.List(_googleSettings.MerchantId);
        //        listRequest.MaxResults = 250;

        //        var response = listRequest.Execute();

        //        while (response.NextPageToken != null)
        //        {
        //            var list = response.Resources;
        //            foreach (var resource in list)
        //            {
        //                if (resource.Id.StartsWith($"online:sv:{feedLabel.ToUpper()}:"))
        //                {
        //                    products.Add(LoadProduct(resource.Id));
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(response.NextPageToken))
        //            {
        //                listRequest = _shoppingContentService.Products.List(_googleSettings.MerchantId);
        //                listRequest.MaxResults = 250; //max 250
        //                listRequest.PageToken = response.NextPageToken;
        //                response = listRequest.Execute();
        //            }
        //        }
        //        return products;
        //    }

        //    public bool InsertProduct(Product product)
        //    {
        //        Product response = _shoppingContentService.Products.Insert(product, _googleSettings.MerchantId).Execute();
        //        if (response == null)
        //        {
        //            _logger.LogWarning("Insert product failed. ID:{0}, Title:{1}", product.Id, product.Title);
        //            return false;
        //        }
        //        _logger.LogInformation("Insert product, ID:{0}, Title:{1}", response.Id, response.Title);
        //        return true;
        //    }

        //    public bool DeleteProduct(Product product)
        //    {
        //        var request = _shoppingContentService.Products.Delete(_googleSettings.MerchantId, product.Id);
        //        var deleteResult = _shoppingUtil.ExecuteWithRetries(request, _retryCodes);
        //        if (deleteResult == null)
        //        {
        //            _logger.LogWarning("Delete product failed. ID:{0}, Title:{1}", product.Id, product.Title);
        //            return false;
        //        }
        //        _logger.LogInformation("Google product deleted. ID:{0}, Title:{1}", product.Id, product.Title);
        //        return true;
        //    }
    }
}
