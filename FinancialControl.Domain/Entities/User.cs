namespace FinancialControl.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        private User() { }

        public User(string name, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }

        public void Validate(string name, string email, string passwordHash)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if(string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.");
            if(string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password cannot be empty.");
        }
    }
}
