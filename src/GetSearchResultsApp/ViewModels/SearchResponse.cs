using System.Collections.Generic;

namespace GetSearchResultsApp.ViewModels
{
    /// <summary>
    /// Viewmodel to decouple service entities from the view.
    /// </summary>
    public class SearchResponse
    {
        public string[] ErrorMessages { get; set; }
        public string ErrorCode { get; set; }
        public bool Success { get; set; }
        public bool LimitWarning { get; set; }
        public List<SearchResult> SearchResults { get; set; }
    }

    public class SearchResult
    {
        public uint ZpId { get; set; }
        public SearchResultLinks Links { get; set; }
        public SearchResultAddress Address { get; set; }
        public SearchResultZestimate Zestimate { get; set; }
        public SearchResultZestimate RentZestimate { get; set; }
        public List<SearchResultLocalRegion> LocalRegions { get; set; }
    }

    public class SearchResultLinks
    {
        public string HomeDetailsLink { get; set; }
        public string GraphsAndDataLink { get; set; }
        public string MapThisHomeLink { get; set; }
        public string ComparablesLinks { get; set; }
    }

    public class SearchResultAddress
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class SearchResultZestimate
    {
        public string Currency { get; set; }

        public decimal? Amount { get; set; }
        public string AmountString { get; set; }

        public decimal? LowValuation { get; set; }
        public string LowValuationString { get; set; }

        public decimal? HighValuation { get; set; }
        public string HighValuationString { get; set; }

        public decimal? ValueChange { get; set; }
        public string ValueChangeString { get; set; }

        public string ValueChangeDuration { get; set; }
        public bool? ValueDown { get; set; }
    }

    public class SearchResultLocalRegion
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public uint Id { get; set; }
        public decimal? ZindexValue { get; set; }
        public decimal? ZindexOneYearChange { get; set; }

        public string OverViewLink { get; set; }
        public string ForSaleByOwnerLink { get; set; }
        public string ForSaleLink { get; set; }
    }
}