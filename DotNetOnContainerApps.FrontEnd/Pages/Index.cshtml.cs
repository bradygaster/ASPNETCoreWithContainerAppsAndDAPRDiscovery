using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetOnContainerApps.FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public string CatalogApiStatus;
        public string OrderApiStatus;
        public string CatalogApiUrl;
        public string OrderApiUrl;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            using (var catalogClient = _httpClientFactory.CreateClient("catalog"))
            {
                try
                {
                    CatalogApiStatus = await catalogClient.GetStringAsync("/");
                    CatalogApiUrl = catalogClient.BaseAddress.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    CatalogApiStatus = ex.Message;
                }
            }
            using (var ordersClient = _httpClientFactory.CreateClient("orders"))
            {
                try
                {
                    OrderApiStatus = await ordersClient.GetStringAsync("/");
                    OrderApiUrl = ordersClient.BaseAddress.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    OrderApiStatus = ex.Message;
                }
            }
        }
    }
}