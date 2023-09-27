// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Moq;
// using SolarWatch;
// using SolarWatch.Controllers;
// using SolarWatch.Service;
// using WeatherApi.Service;
//
// namespace SolarWatchControllerTests;
//
// [TestFixture]
// public class Tests
// {
//     private Mock<ILogger<SolarWatchController>> _loggerMock;
//     private Mock<ISunriseSunsetProvider> _sunriseSunsetProviderMock;
//     private Mock<ILongitudeAndLatitudeProvider> _longitudeAndLatitudeProviderMock;
//     private Mock<IJsonProcessor> _jsonProcessorMock;
//     private SolarWatchController _controller;
//     
//     [SetUp]
//     public void Setup()
//     {
//         _loggerMock = new Mock<ILogger<SolarWatchController>>();
//         _sunriseSunsetProviderMock = new Mock<ISunriseSunsetProvider>();
//         _longitudeAndLatitudeProviderMock = new Mock<ILongitudeAndLatitudeProvider>();
//         _jsonProcessorMock = new Mock<IJsonProcessor>();
//         _controller = new SolarWatchController(_loggerMock.Object, _longitudeAndLatitudeProviderMock.Object,
//             _jsonProcessorMock.Object, _sunriseSunsetProviderMock.Object);
//     }
//
//     // [Test]
//     // public void GetCurrentReturnsNotFoundResultIfLongLatProviderFails()
//     // {
//     //     // Arrange
//     //     _longitudeAndLatitudeProviderMock.Setup(x =>
//     //         x.GetCurrent(It.IsAny<string>())).Throws(new Exception());
//     //     
//     //     // Act
//     //     var result = _controller.GetSunrise("London", DateTime.Now);
//     //     
//     //     Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
//     // }
//     //
//     // [Test]
//     // public void GetCurrentReturnsNotFoundResultIfLongLatDataIsInvalid()
//     // {
//     //     // Arrange
//     //     var longLatData = "{}";
//     //     _longitudeAndLatitudeProviderMock.Setup(x =>
//     //         x.GetCurrent(It.IsAny<string>())).Returns(longLatData);
//     //     _jsonProcessorMock.Setup(x => x.ProcessLongLat(longLatData)).Throws<Exception>();
//     //
//     //     // Act
//     //     var result = _controller.GetSunrise(longLatData, DateTime.Now);
//     //
//     //     // Assert
//     //     Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
//     // }
//     //
//     // [Test]
//     // public void GetCurrentReturnsOkResultIfWeatherDataIsValid()
//     // {
//     //     // Arrange
//     //     var expectedForecast = new SolarWatch.SolarWatch();
//     //     var expectedLongLat = new Tuple<string, string>("51.5073219","-0.1276474");
//     //     var sunriseData = "{}";
//     //     
//     //     
//     //     _sunriseSunsetProviderMock.Setup(x => x.GetOnDate(expectedLongLat, DateTime.Now)).Returns(sunriseData);
//     //     _jsonProcessorMock.Setup(x => x.ProcessSunrise(sunriseData)).Returns(expectedForecast);
//     //     
//     //     // Act
//     //     var result = _controller.GetSunrise("London", DateTime.Now);
//     //
//     //     // Assert
//     //     Assert.That(result.Result, Is.InstanceOf(typeof(OkObjectResult)));
//     //     Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(expectedForecast));
//     // }
// }