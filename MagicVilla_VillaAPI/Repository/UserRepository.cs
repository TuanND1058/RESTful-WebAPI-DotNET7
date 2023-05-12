using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private string secretKey;

        public UserRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _context.LocalUsers.FirstOrDefault(x => x.UserName == username);
            return user == null;
        }

        public async Task<LoginRequestDTO> Login(LoginRequestDTO request)
        {
            var user = _context.LocalUsers.FirstOrDefault(x => x.UserName == request.UserName && x.Password == request.Password);

            if (user == null)
            {
                return null;
            }

            return null;
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO request)
        {
            LocalUser user = new()
            {
                Name = request.Name,
                UserName = request.UserName,
                Password = request.Password,
                Role = request.Role
            };

            _context.LocalUsers.Add(user);
            await _context.SaveChangesAsync();
            user.Password = "";

            return user;
        }
    }
}
