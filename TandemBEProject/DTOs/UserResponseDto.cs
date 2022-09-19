namespace TandemBEProject.DTOs
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
    }
}
