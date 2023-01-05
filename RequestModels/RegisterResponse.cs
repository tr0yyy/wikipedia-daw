using WikipediaDAW.Models;

namespace WikipediaDAW.RequestModels
{
    public class RegisterResponse
    {
        public String UserName {get;set;}
        public String Email { get; set; }
        public String Role { get; set; }

        public RegisterResponse(User user, string role)
        {
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.Role = role;
        }
    }
}
