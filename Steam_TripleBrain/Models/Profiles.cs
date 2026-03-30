namespace Steam_TripleBrain.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Profiles
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Унікальний ідентифікатор для кожного профілю користувача
        public string? Username { get; set; } // Ім'я користувача, яке буде використовуватися для входу в систему
        public string? PasswordHash { get; set; } // Хеш пароля, який буде зберігатися в базі даних (не зберігаємо пароль у відкритому вигляді)
        [NotMapped]
        public string? Password { get; set; }

        public string Role { get; set; } = "user"; // Роль користувача (наприклад, "Admin", "User" тощо), за замовчуванням - "user"
        public string? Email { get; set; } // Електронна адреса користувача, яка може використовуватися для зв'язку або відновлення пароля
        public string? FullName { get; set; } // Повне ім'я користувача, яке може використовуватися для відображення в інтерфейсі або інших цілях
    }
}
