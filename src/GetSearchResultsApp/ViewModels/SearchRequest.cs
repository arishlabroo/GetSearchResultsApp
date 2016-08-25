using System.ComponentModel.DataAnnotations;

namespace GetSearchResultsApp.ViewModels
{
    public partial class SearchRequest
    {
        [Required]
        [Display(Name = "Address Line")]
        public string AddressLine { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        [Display(Name="Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Zestimate Rent")]
        public bool ZestimateRent { get; set; }
    }
}