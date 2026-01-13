using AutoMapper;
using WebService.DTOs.Users;
using WebService.Interfaces.Users;
using WebService.Models;

namespace WebService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetUserDTO>>(users);
        }

        public async Task<GetUserDTO> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task<GetUserDTO> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task<UserResponseDTO> UpdateProfileAsync(int userId, UpdateUserDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
            user.HoTen = dto.HoTen;
            user.Email = dto.Email;
            user.SoDienThoai = dto.SoDienThoai;
            user.DiaChi = dto.DiaChi;
            user.NgayCapNhat = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> UpdateAsync(int id, UpdateUserDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
            user.HoTen = dto.HoTen;
            user.Email = dto.Email;
            user.SoDienThoai = dto.SoDienThoai;
            user.DiaChi = dto.DiaChi;
            if (dto.TrangThai.HasValue) user.TrangThai = dto.TrangThai.Value;
            if (!string.IsNullOrWhiteSpace(dto.Quyen)) user.Quyen = dto.Quyen;
            user.NgayCapNhat = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> CreateAsync(CreateUserDTO dto)
        {
            var entity = _mapper.Map<User>(dto);
            entity.NgayTao = entity.NgayCapNhat = DateTime.Now;
            var createdUser = await _userRepository.CreateAsync(entity);
            return _mapper.Map<UserResponseDTO>(createdUser);
        }

        public async Task<UserResponseDTO> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
            await _userRepository.DeleteAsync(id);
            return _mapper.Map<UserResponseDTO>(user);
        }
    }
}