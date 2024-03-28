namespace Loan_API.Domain
{
    public class Loan
    {
        public Loan()
        {
            Status = LoanStatus.Processing;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int Period { get; set; }
        public User User { get; set; }

        
    }

}
