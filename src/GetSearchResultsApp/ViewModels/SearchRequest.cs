using System.ComponentModel.DataAnnotations;

namespace GetSearchResultsApp.ViewModels
{
    public partial class SearchRequest
    {
        [Required]
        public string AddressLine { get; set; }

        public string City { get; set; }
        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        public bool ZestimateRent { get; set; }
    }
}