using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Text;
using static Mango.Web.Utility.Miscelenous;

namespace Mango.Web.Service;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenProvider _tokenProvider;

    public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
    }

    public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
    {
        try
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("MangoAPI");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Headers.Add("Accept", "application/json");

            if (withBearer)
            {
                string token = _tokenProvider.GetToken();
                httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");
            }

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
