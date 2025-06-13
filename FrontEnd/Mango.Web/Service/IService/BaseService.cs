using Mango.Web.Models;
using Newtonsoft.Json;
using System.Text;
using static Mango.Web.Utility.Miscelenous;

namespace Mango.Web.Service.IService;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public BaseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
    {
        try
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("MangoAPI");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Headers.Add("Accept", "application/json");
            //token

            httpRequestMessage.RequestUri = new Uri(requestDto.Url);

            if (requestDto.Data is not null)
            {
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage apiResponseMessage = null;

            switch (requestDto.ApiType)
            {
                case ApiType.GET:
                    httpRequestMessage.Method = HttpMethod.Get;
                    break;
                case ApiType.POST:
                    httpRequestMessage.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    httpRequestMessage.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    httpRequestMessage.Method = HttpMethod.Delete;
                    break;
            }

            apiResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            switch (apiResponseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case System.Net.HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case System.Net.HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };
                case System.Net.HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    string apiContent = await apiResponseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            }
        }
        catch (Exception ex)
        {
            ResponseDto responseDto = new()
            {
                IsSuccess = false,
                Message = ex.Message,
            };

            return responseDto;
        }
    }
}
