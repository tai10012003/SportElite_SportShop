namespace WebService.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public int Id { get; set; }
        public string MaNguoiDung { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Quyen { get; set; } = string.Empty;
    }
}