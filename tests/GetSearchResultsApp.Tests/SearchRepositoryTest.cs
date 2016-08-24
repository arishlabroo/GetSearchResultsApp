using System.Threading.Tasks;
using GetSearchResultsApp.Model;
using GetSearchResultsApp.Model.Interfaces;
using GetSearchResultsApp.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace GetSearchResultsApp.Tests
{
    //NOTE: THIS IS NOT A COMPREHENSIVE TEST SUITE. 
    //Just added a couple of tests for example.

    [TestFixture]
    public class SearchRepositoryTests
    {
        private Mock<ISearchRequestProcessor> _mockProcessor;
        private Mock<ISearchResponseMapper> _mockResponseMapper;
        private Mock<ILogger<SearchRepository>> _mockLogger;


        [SetUp]
        public void Setup()
        {
            _mockProcessor = new Mock<ISearchRequestProcessor>();
            _mockResponseMapper = new Mock<ISearchResponseMapper>();
            _mockLogger = new Mock<ILogger<SearchRepository>>(MockBehavior.Loose);
        }


        [Test]
        public async Task SearchAsync_ReturnsFailedResponse_ForNullRequest()
        {
            //Arrange
            var sut = new SearchRepository(_mockProcessor.Object, _mockResponseMapper.Object, _mockLogger.Object);

            //Act
            var response = await sut.SearchAsync(null);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.IsNotEmpty(response.ErrorMessages);
        }

        [Test]
        public async Task SearchAsync_ReturnsFailedResponse_ForEmptyRawResponse()
        {
            //Arrange
            var request = new SearchRequest();
            _mockProcessor.Setup(x => x.SearchAsync(request)).ReturnsAsync(string.Empty);
            var sut = new SearchRepository(_mockProcessor.Object, _mockResponseMapper.Object, _mockLogger.Object);

            //Act
            var response = await sut.SearchAsync(request);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.IsNotEmpty(response.ErrorMessages);
            _mockProcessor.Verify(x => x.SearchAsync(It.IsAny<SearchRequest>()), Times.Once);
        }

        [Test]
        public async Task SearchAsync_InvokesProcessorAndMapper_ForValidRequest()
        {
            //Arrange
            var request = new SearchRequest {AddressLine = "line", ZipCode = "92406"};
            var mappedResponse = new SearchResponse();
            _mockProcessor.Setup(x => x.SearchAsync(request)).ReturnsAsync("Valid XML Response");
            _mockResponseMapper.Setup(x => x.MapResponse("Valid XML Response")).Returns(mappedResponse);

            var sut = new SearchRepository(_mockProcessor.Object, _mockResponseMapper.Object, _mockLogger.Object);

            //Act
            var response = await sut.SearchAsync(request);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response,mappedResponse);
            _mockProcessor.Verify(m => m.SearchAsync(request), Times.Once);
            _mockResponseMapper.Verify(m => m.MapResponse("Valid XML Response"), Times.Once);
        }
    }
    }