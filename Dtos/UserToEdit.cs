namespace DotnetAPI.Dtos
{
    public class UserToEdit
    {
        public int UserId { get; set; }
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool? Active { get; set; }
    }
}
