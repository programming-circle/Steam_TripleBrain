namespace Steam_TripleBrain.Profiles.Tokens
{
    /// <summary>Ответ login/register (как JwtTokenProfile в amazon-backend + срок действия).</summary>
    public class JwtTokenProfile
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
