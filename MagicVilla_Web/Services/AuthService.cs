using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string villaUrl;

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = ApiType.POST,
                Data = loginRequestDTO,
                Url = villaUrl + "/api/UsersAuth/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDTO registerationRequestDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = ApiType.POST,
                Data = registerationRequestDTO,
                Url = villaUrl + "/api/UsersAuth/register"
            });
        }
    }
}
