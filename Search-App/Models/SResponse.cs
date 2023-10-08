
namespace Search_App.Models
{
    public class SResponse
    {
        //public string CustomerType {  get; set; }
        //public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        //public string LastName { get; set; }
        //public string CompanyName { get; set; }
        //public string Name { get; set; }
        //public string DBAName { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string StateCode { get; set; }
        //public string PostalCode { get; set; }
        //public string Country { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public double Score { get; set; }
        public double NSScore { get; set; }
        public double ADScore { get; set; }

    }
}