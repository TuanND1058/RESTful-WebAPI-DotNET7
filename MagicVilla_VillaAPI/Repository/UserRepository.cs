using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _context.LocalUsers.FirstOrDefault(x => x.UserName == username);
            return user == null;
        }

        public async Task<LoginRequestDTO> Login(LoginRequestDTO request)
        {
            throw new NotImplementedException();
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
