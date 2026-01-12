using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.DTOs.Users;
using WebService.Interfaces.Users;
using WebService.Models;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetUserDTO>> GetAllAsync()
    {
        var users = await _context.Users.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<GetUserDTO>>(users);
    }

    public async Task<GetUserDTO> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
        return _mapper.Map<GetUserDTO>(user);
    }

    public async Task<GetUserDTO> GetProfileAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
        return _mapper.Map<GetUserDTO>(user);
    }

    public async Task<UserResponseDTO> UpdateProfileAsync(int userId, UpdateUserDTO dto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
        user.HoTen = dto.HoTen;
        user.Email = dto.Email;
        user.SoDienThoai = dto.SoDienThoai;
        user.DiaChi = dto.DiaChi;
        user.NgayCapNhat = DateTime.Now;
        await _context.SaveChangesAsync();
        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<UserResponseDTO> UpdateAsync(int id, UpdateUserDTO dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
        user.HoTen = dto.HoTen;
        user.Email = dto.Email;
        user.SoDienThoai = dto.SoDienThoai;
        user.DiaChi = dto.DiaChi;
        if (dto.TrangThai.HasValue) user.TrangThai = dto.TrangThai.Value;
        if (!string.IsNullOrWhiteSpace(dto.Quyen)) user.Quyen = dto.Quyen;
        user.NgayCapNhat = DateTime.Now;
        await _context.SaveChangesAsync();
        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<UserResponseDTO> CreateAsync(CreateUserDTO dto)
    {
        var entity = _mapper.Map<User>(dto);
        entity.NgayTao = entity.NgayCapNhat = DateTime.Now;
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserResponseDTO>(entity);
    }

    public async Task<UserResponseDTO> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new KeyNotFoundException("Không tìm thấy người dùng");
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserResponseDTO>(user);
    }
}
