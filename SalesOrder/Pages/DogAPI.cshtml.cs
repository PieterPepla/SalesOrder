using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SharedLibrary.ViewModels;

namespace SalesOrder.Pages
{
    public class DogAPIModel : PageModel
    {
        public DogAPI dogApi;

        public async Task OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync("https://dog.ceo/api/breeds/image/random"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dogApi = JsonConvert.DeserializeObject<DogAPI>(apiResponse);
                }
            }
        }
    }
}
