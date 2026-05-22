using FinancialControl.Domain.Enums;

namespace FinancialControl.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public TransactionType Type { get; private set; }
        public DateTime Date { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        private Transaction() { }

        public Transaction(
            Guid userId, 
            Guid categoryId,
            decimal amount, 
            string description, 
            TransactionType type, 
            DateTime date
            )
        {
            Validate(userId, categoryId, amount, description);
            Id = Guid.NewGuid();
            UserId = userId;
            CategoryId = categoryId;
            Amount = amount;
            Description = description;
            Type = type;
            Date = date;
        }

        public void Validate(Guid userId, Guid categoryId, decimal amount, string description)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.");
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Category cannot be empty.");
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty.");
        }

        public void Update(Guid categoryId, decimal amount, string description, TransactionType type)
        {
            Validate(UserId, categoryId, amount, description);
            CategoryId = categoryId;
            Amount = amount;
            Description = description;
            Type = type;
        }
    }
}
