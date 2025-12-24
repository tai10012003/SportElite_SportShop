using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebService.DTOs.Auth;
using WebService.Models;
using WebService.Interfaces.Auth;
using BCrypt.Net;

namespace WebService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var user = await _authRepository.GetUserByEmailAsync(loginDto.Email);
            
            if (user == null)
                throw new Exception("Email không tồn tại");

            var isValidPassword = await _authRepository.ValidatePasswordAsync(user, loginDto.Password);
            if (!isValidPassword)
                throw new Exception("Mật khẩu không đúng");

            if (!user.TrangThai)
                throw new Exception("Tài khoản đã bị khóa");

            var token = GenerateJwtToken(user);

            return new AuthResponseDTO
            {
                Id = user.Id,
                MaNguoiDung = user.MaNguoiDung,
                Email = user.Email,
                HoTen = user.HoTen,
                Quyen = user.Quyen,
                Token = token
            };
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new List<Claim>
            {
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Quyen ?? "user"),
                new Claim(ClaimTypes.Name, user.HoTen ?? user.TenDangNhap),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto)
        {
            var emailExists = await _authRepository.GetUserByEmailAsync(registerDto.Email);
            if (emailExists != null)
                throw new Exception("Email đã được sử dụng");
            var usernameExists = await _authRepository.GetUserByUsernameAsync(registerDto.TenDangNhap);
            if (usernameExists != null)
                throw new Exception("Tên đăng nhập đã tồn tại");
            string maNguoiDung;
            int counter = 1;
            do
            {
                maNguoiDung = $"USER{counter:D3}";
                counter++;
            }
            while (await _authRepository.GetUserByMaNguoiDungAsync(maNguoiDung) != null);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.MatKhau);
            var user = new User
            {
                MaNguoiDung = maNguoiDung,
                TenDangNhap = registerDto.TenDangNhap,
                Email = registerDto.Email,
                HoTen = registerDto.HoTen,
                MatKhau = passwordHash,
                SoDienThoai = registerDto.SoDienThoai,
                DiaChi = registerDto.DiaChi,
                Quyen = "user",
                TrangThai = true,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };
            var createdUser = await _authRepository.CreateUserAsync(user);
            var token = GenerateJwtToken(createdUser);

            return new AuthResponseDTO
            {
                Id = createdUser.Id,
                MaNguoiDung = createdUser.MaNguoiDung,
                Email = createdUser.Email,
                HoTen = createdUser.HoTen,
                Quyen = createdUser.Quyen,
                Token = token
            };
        }
    }
}