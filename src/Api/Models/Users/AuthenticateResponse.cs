using System.Text.Json.Serialization;
using WebApi.Entities;

namespace WebApi.Models.Users
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(int id, string firstName, string lastName, string username, string jwtToken, string refreshToken) 
        {
            this.Id = id;
                this.FirstName = firstName;
                this.LastName = lastName;
                this.Username = username;
                this.JwtToken = jwtToken;
                this.RefreshToken = refreshToken;
               
        }
                public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}