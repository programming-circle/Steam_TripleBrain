namespace Steam_TripleBrain.Models
{
    public class TokenLogs
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Унікальний ідентифікатор для кожного запису в журналі токенів
        public string? Username { get; set; } // Ім'я користувача, для якого видано токен
        public string? Token { get; set; } // Сам токен, який був виданий користувачу
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow; // Час, коли токен був виданий (за замовчуванням - поточний час у форматі UTC)
        public DateTime? ExpiredAt { get; set; } // Час, коли токен стане недійсним (може бути null, якщо час закінчення не встановлено)
        public bool IsRevoked { get; set; } = false; // Прапорець, який вказує, чи був токен відкликаний (за замовчуванням - false)
    }
}
