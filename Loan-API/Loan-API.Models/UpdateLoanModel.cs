namespace Loan_API.Models
{
    public class UpdateLoanModel
    {
        public int LoanId { get; set; }
        public string LoanType { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public int LoanPeriod { get; set; }

    }
}
