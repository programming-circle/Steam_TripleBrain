namespace Steam_TripleBrain
{
    public class AuthResponse
    {
        public string Accesstoken { get; set; }
        public DateTime AccessExpiresAtUtc { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpiresAtUtc { get; set; }

    }
}
