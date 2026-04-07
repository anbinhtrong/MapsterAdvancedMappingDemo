using System.Collections.Generic;

namespace MapsterDemo.Models
{
    public class RequestDetail : EntityBase
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Employee Employee { get; set; }

        // Computed property - getter only
        public decimal TotalPrice
        {
            get { return Quantity * UnitPrice; }
        }
    }

    public class Request : EntityBase
    {
        public string RequestCode { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public List<RequestDetail> RequestDetails { get; set; }

        public Request()
        {
            RequestDetails = new List<RequestDetail>();
        }
    }
}
