using System;
using System.IO;
using System.Linq;
using GetSearchResultsApp.Model;
using GetSearchResultsApp.Model.Interfaces;
using GetSearchResultsApp.ViewModels;
using Moq;
using NUnit.Framework;

namespace GetSearchResultsApp.Tests
{
    [TestFixture]
    public class XmlResponseMapperTests
    {
        private Mock<IZillowTypeMapper> _mockZillowTypeMapper;

        [SetUp]
        public void Setup()
        {
            _mockZillowTypeMapper = new Mock<IZillowTypeMapper>();
        }

        [Test]
        public void MapResponse_Throws_IfNoXmlPassed()
        {
            //Arrange
            var sut = new XmlResponseMapper(_mockZillowTypeMapper.Object);

            //Act
            //Assert
            Assert.Throws<ArgumentException>(() => sut.MapResponse(string.Empty));
        }

        [Test]
        public void MapResponse_SuccessfullyParses_ValidXml()
        {
            //Arrange
            var rawResponse = File.ReadAllText("XmlResponseFake.txt");
            var mappedRespone = new SearchResponse();
            _mockZillowTypeMapper.Setup(m => m.MapSearchResponse(It.IsAny<searchresults>())).Returns(mappedRespone);

            var sut = new XmlResponseMapper(_mockZillowTypeMapper.Object);

            //Act
            var respone = sut.MapResponse(rawResponse);

            //Assert
            Assert.AreEqual(respone, mappedRespone);

            //This is set in the XmlResponseFake.txt
            Func<searchresults, bool> firstStreetIsConstString =
                (s) => s?.response?.results?.FirstOrDefault()?.address?.street == "Arish Labroo In XML";

            _mockZillowTypeMapper.Verify(
                m => m.MapSearchResponse(It.Is<searchresults>(s => firstStreetIsConstString(s))), Times.Once);

        }
    }
}