using BoilerPlate.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;


namespace BoilerPlate.Domain.Entities
{
    //one-to-many relationship:
    // One User can have many Transaction: This is the main relationship between the two entities.
    // A user can own multiple Transaction, but each Transaction is owned by exactly one user.

    //one-to-one relationship
    //Each transaction corresponds to a single subscription
    //(although there could be multiple transactions for a subscription
    //over time if it's a recurring subscription, this design implies onetransaction per subscription).
    public class Transaction : EntityBase
    {
        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public Guid? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public Guid? SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public Subscription? Subscription { get; set; }

        public TransactionType Type { get; set; }

        public TransactionStatus Status { get; set; }

        public TransactionIntegrationPartners Partners { get; set; }

        public decimal Amount { get; set; }

        public string? Reference { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
