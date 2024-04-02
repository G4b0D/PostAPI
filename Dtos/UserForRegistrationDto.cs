namespace DotNetAPI_EF.Dtos
{
    public partial class UserForRegistrationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }  
        public string PasswordConfirm { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Gender { get; set; } = "";

        public UserForRegistrationDto() { 
            
            if (Email == null)
            {
                Email = string.Empty;
            }
            if (Password == null)
            {
                Password = string.Empty;
            }
            if (PasswordConfirm == null)
            {
                PasswordConfirm = string.Empty;
            }
        }
    }
}
