using CRD.Enums;

namespace CRD.Models
{
    public class Loan
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public Currency CurrencyCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public LoanStatusCode LoanStatusCode { get; set; }

        public bool IsDeleted { get; set; }

    }

    public class UpdateLoanRequest
    {
        public int ID { get; set; }
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public Currency CurrencyCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class AddLoan
    {
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public Currency CurrencyCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }


}
