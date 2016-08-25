using System;
using System.Linq;
using GetSearchResultsApp.Model.Interfaces;
using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model
{
    /// <summary>
    /// Maps the zillow type to the viewmodel type
    /// </summary>
    /// <seealso cref="GetSearchResultsApp.Model.Interfaces.IZillowTypeMapper" />
    public class ZillowTypeMapper : IZillowTypeMapper
    {
        public SearchResponse MapSearchResponse(searchresults searchresults)
        {
            if (searchresults?.message == null) return null;

            var response = new SearchResponse
            {
                Success = string.Equals("0", searchresults.message.code)
            };

            if (!response.Success)
            {
                response.ErrorMessages = new[] {searchresults.message.text};
                response.ErrorCode = searchresults.message.code;
                return response;
            }

            response.LimitWarning =
                searchresults.message.limitwarningSpecified && searchresults.message.limitwarning;

            response.SearchResults =
                searchresults.response?.results?.Select(MapPropertyToSearchResult)?.ToList();

            return response;
        }

        private static SearchResult MapPropertyToSearchResult(SimpleProperty simpleProperty)
        {
            if (simpleProperty == null) return null;

            return new SearchResult
            {
                ZpId = simpleProperty.zpid,
                Address = MapAddress(simpleProperty.address),
                Links = MapLinks(simpleProperty.links),
                Zestimate = MapEstimate(simpleProperty.zestimate),
                RentZestimate = MapEstimate(simpleProperty.rentzestimate),
                LocalRegions = simpleProperty.localRealEstate?.Select(MapRegion)?.ToList()
            };
        }

        private static SearchResultAddress MapAddress(Address address)
        {
            if (address == null) return null;

            return new SearchResultAddress
            {
                Street = address.street,
                City = address.city,
                State = address.state,
                ZipCode = address.zipcode,
                Latitude = address.latitude,
                Longitude = address.longitude,
            };
        }

        private static SearchResultLinks MapLinks(Links links)
        {
            if (links == null) return null;
            return new SearchResultLinks
            {
                ComparablesLinks = links.comparables,
                GraphsAndDataLink = links.graphsanddata,
                HomeDetailsLink = links.homedetails,
                MapThisHomeLink = links.mapthishome,
            };
        }


        private static SearchResultZestimate MapEstimate(Zestimate zestimate)
        {
            if (zestimate == null) return null;

            var estimate = new SearchResultZestimate();

            if (zestimate.amount != null)
            {
                estimate.Amount = SafeConvertToDecimal(zestimate.amount.Value);
                estimate.AmountString = ToCurrencyString(estimate.Amount);
                estimate.Currency = zestimate.amount.currency.ToString();
            }

            if (zestimate.valuationRange != null)
            {
                estimate.HighValuation = SafeConvertToDecimal(zestimate.valuationRange.high?.Value);
                estimate.HighValuationString = ToCurrencyString(estimate.HighValuation);

                estimate.LowValuation = SafeConvertToDecimal(zestimate.valuationRange.low?.Value);
                estimate.LowValuationString = ToCurrencyString(estimate.LowValuation);
            }

            if (zestimate.valueChange != null)
            {
                estimate.ValueChange = SafeConvertToDecimal(zestimate.valueChange.Value);

                estimate.ValueChangeString = estimate.ValueChange.HasValue
                    ? ToCurrencyString(Math.Abs(estimate.ValueChange.Value))
                    : string.Empty;

                estimate.ValueDown = estimate.ValueChange < 0;
                estimate.ValueChangeDuration = zestimate.valueChange.duration;
            }

            return estimate;
        }

        private static SearchResultLocalRegion MapRegion(LocalRealEstateRegion region)
        {
            if (region == null) return null;

            var mappedRegion = new SearchResultLocalRegion
            {
                Name = region.name,
                Id = region.id,
                Type = region.type,
                ZindexValue = SafeConvertToDecimal(region.zindexValue)

            };

            if (region.links != null)
            {
                mappedRegion.ForSaleByOwnerLink = region.links.forSaleByOwner;
                mappedRegion.ForSaleLink = region.links.forSale;
                mappedRegion.OverViewLink = region.links.overview;
            }

            return mappedRegion;

        }

        private static decimal? SafeConvertToDecimal(string input)
        {
            decimal output;
            if (decimal.TryParse(input, out output))
            {
                return output;
            }
            return default(decimal?);
        }

        private static string ToCurrencyString(decimal? number)
        {
            //NOTE: The api docs says everything is in "$". So i am just formatting it to currency and ignoring the"Currency field" 
            return number?.ToString("C0") ?? string.Empty;
        }
    }
}