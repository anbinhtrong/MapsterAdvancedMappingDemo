using System.Collections.Generic;

namespace MapsterDemo.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string DisplayName { get; set; }
    }

    public class RequestDetailViewViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public EmployeeViewModel Employee { get; set; }
    }

    public class RequestViewModel
    {
        public int Id { get; set; }
        public string RequestCode { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public List<RequestDetailViewViewModel> RequestDetailModels { get; set; }

        public RequestViewModel()
        {
            RequestDetailModels = new List<RequestDetailViewViewModel>();
        }
    }
}
