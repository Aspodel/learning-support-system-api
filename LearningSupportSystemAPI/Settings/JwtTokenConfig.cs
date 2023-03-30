namespace LearningSupportSystemAPI.Settings
{
    public class JwtTokenConfig
    {
        public string Key { get; set; } = string.Empty;
        public TimeSpan ExpiresIn { get; set; }
        public string TokenType { get; set; } = string.Empty;
    }
}
