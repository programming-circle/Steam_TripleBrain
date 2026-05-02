namespace Steam_TripleBrain.Profiles.Tokens
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool StaySignedIn { get; set; }
    }
}
