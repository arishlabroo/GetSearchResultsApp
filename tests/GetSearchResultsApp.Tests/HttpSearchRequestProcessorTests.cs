﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GetSearchResultsApp.Model;
using GetSearchResultsApp.Model.Interfaces;
using GetSearchResultsApp.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace GetSearchResultsApp.Tests
{
    //NOTE: THIS IS NOT A COMPREHENSIVE TEST SUITE. 
    //Just added a couple of tests for example.

    [TestFixture]
    public class HttpSearchRequestProcessorTests
    {
        private const string CityStateZipParamName = "citystatezip";
        private Mock<IHttpClientWrapper> _mockHttpClient;
        private Mock<IOptions<ZillowServiceSettings>> _mockOptions;
        private Mock<ILogger<HttpSearchRequestProcessor>> _mockLogger;

        private static readonly ZillowServiceSettings Settings = new ZillowServiceSettings
        {
            SearchServiceUrl = "http://happybirthdaytoyou.com",
            ZwsId = "IMeMyself"
        };


        [SetUp]
        public void Setup()
        {
            _mockHttpClient = new Mock<IHttpClientWrapper>();
            _mockLogger = new Mock<ILogger<HttpSearchRequestProcessor>>();
            _mockOptions = new Mock<IOptions<ZillowServiceSettings>>();

            _mockHttpClient.Setup(m => m.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent("Hello")});

            _mockOptions.Setup(o => o.Value).Returns(Settings);
        }

        [Test]
        public async Task SearchAsync_UrlEncodes_AllParameters()
        {
            //Arrange
            var sut = new HttpSearchRequestProcessor(_mockOptions.Object, _mockHttpClient.Object, _mockLogger.Object);
            
            var request = new SearchRequest
            {
                AddressLine = "apple",
                ZipCode = "92+: 618",
                ZestimateRent = false
            };

            var query =
                $"?zws-id={Settings.ZwsId}&address={request.AddressLine}&citystatezip=92%2B%3A%20618&rentzestimate=False";

            //Act
            var stringResponse = await sut.SearchAsync(request);

            //Assert
            _mockHttpClient.Verify(h => h.GetAsync(It.Is<Uri>(u => u.Query == query)), Times.Once);
        }


        [Test]
        public async Task SearchAsync_CalculatesValid_CityStateZipParameter()
        {
            //Arrange
            var sut = new HttpSearchRequestProcessor(_mockOptions.Object, _mockHttpClient.Object, _mockLogger.Object);

            var request = new SearchRequest
            {
                AddressLine = "Line1",
                City = "Irvine",
                State = "CA",
                ZipCode = "92618"
            };


            //Act
            var stringResponse = await sut.SearchAsync(request);

            //Assert
            const string expectedParamValue = "Irvine CA, 92618";
            Assert.AreEqual("Hello", stringResponse);
            _mockHttpClient.Verify(h => h.GetAsync(It.Is<Uri>(u => CityStateZipParamIs(u, expectedParamValue))),
                Times.Once);
        }

        [Test]
        public async Task SearchAsync_PassesOnlyZipAs_CityStateZipParameter()
        {
            //Arrange
            var sut = new HttpSearchRequestProcessor(_mockOptions.Object, _mockHttpClient.Object, _mockLogger.Object);

            var request = new SearchRequest
            {
                AddressLine = "Line1",
                ZipCode = "92618"
            };


            //Act
            var stringResponse = await sut.SearchAsync(request);

            //Assert
            const string expectedParamValue = "92618";
            _mockHttpClient.Verify(h => h.GetAsync(It.Is<Uri>(u => CityStateZipParamIs(u, expectedParamValue))),
                Times.Once);
        }

        [Test]
        public async Task SearchAsync_PassesStateZipAs_CityStateZipParameter()
        {
            //Arrange
            var sut = new HttpSearchRequestProcessor(_mockOptions.Object, _mockHttpClient.Object, _mockLogger.Object);

            var request = new SearchRequest
            {
                AddressLine = "Line1",
                State = "CA",
                ZipCode = "92618"
            };


            //Act
            var stringResponse = await sut.SearchAsync(request);

            //Assert
            const string expectedParamValue = "CA, 92618";
            _mockHttpClient.Verify(h => h.GetAsync(It.Is<Uri>(u => CityStateZipParamIs(u, expectedParamValue))),
                Times.Once);
        }



        private static bool CityStateZipParamIs(Uri uri, string cityStateZipParamExpected)
        {
            var queryParams = QueryHelpers.ParseQuery(uri.Query);
            string cityStateZipParamActual = queryParams[CityStateZipParamName];
            return string.Equals(cityStateZipParamExpected, cityStateZipParamActual);
        }
    }
}