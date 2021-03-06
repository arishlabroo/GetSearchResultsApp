using System;
using System.IO;
using System.Xml.Serialization;
using GetSearchResultsApp.Model.Interfaces;
using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model
{
    /// <summary>
    /// Maps the raw XML reponse to the viewmodel.
    /// </summary>
    /// <seealso cref="GetSearchResultsApp.Model.Interfaces.ISearchResponseMapper" />
    public class XmlResponseMapper : ISearchResponseMapper
    {
        private readonly IZillowTypeMapper _zillowTypeMapper;

        public XmlResponseMapper(IZillowTypeMapper zillowTypeMapper)
        {
            _zillowTypeMapper = zillowTypeMapper;
        }

        public SearchResponse MapResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                throw new ArgumentException("Null or whitespace response passed to mapper", nameof(response));
            }

            //NOTE: The weird casing for class 'searchresults' is from the xsd.exe autogenerated file

            var serialzer = new XmlSerializer(typeof (searchresults));

            searchresults searchresults;

            using (var reader = new StringReader(response))
            {
                searchresults = (searchresults) serialzer.Deserialize(reader);
            }

            if (searchresults == null) return null;

            return _zillowTypeMapper.MapSearchResponse(searchresults);
        }
    }
}