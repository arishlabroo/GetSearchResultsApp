using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GetSearchResultsApp.ViewModels
{
    public partial class SearchRequest : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return CityStateGoHandInHand();
            yield return ZipCodeRequiredIfCityStateNotPresent();
        }

        private ValidationResult ZipCodeRequiredIfCityStateNotPresent()
        {
            if (!string.IsNullOrWhiteSpace(City) || !string.IsNullOrWhiteSpace(State))
            {
                //Zip code is only required when city state not present
                return ValidationResult.Success;
            }

            if (string.IsNullOrWhiteSpace(ZipCode))
            {
                return new ValidationResult("Zip code is required if city state is not provided", new[] {"ZipCode"});
            }

            return ValidationResult.Success;
        }

        private ValidationResult CityStateGoHandInHand()
        {
            if (string.IsNullOrWhiteSpace(City) && string.IsNullOrWhiteSpace(State))
            {
                //Both are empty
                return ValidationResult.Success;
            }

            if ((!string.IsNullOrWhiteSpace(City)) && (!string.IsNullOrWhiteSpace(State)))
            {
                //Both are non empty
                return ValidationResult.Success;
            }

            return new ValidationResult("City and State go hand in hand");
        }
    }
}