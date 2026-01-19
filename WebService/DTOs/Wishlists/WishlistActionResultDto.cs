namespace WebService.DTOs.Wishlists
{
    public class WishlistActionResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsInWishlist { get; set; }
    }
}