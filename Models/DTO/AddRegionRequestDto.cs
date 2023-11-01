namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? regionImageUrl { get; set; } // img url can have null value

    }
}
