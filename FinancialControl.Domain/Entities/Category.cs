namespace FinancialControl.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Name { get; private set; }  

        private Category() { }

        public Category(Guid userId, string name)
        {
            Validate(userId, name);

            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
        }

        public void Update(string name)
        {
            Validate(UserId, name);
            Name = name;
        }

        private void Validate(Guid userId, string name)
        {
            if(userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty");
        }
    }
}
