using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MillionRealEstatecompany.API.Middleware;
using Moq;
using System.Net;
using System.Text.Json;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas unitarias para GlobalExceptionHandlingMiddleware
    /// Testing del middleware de manejo global de excepciones
    /// </summary>
    [TestFixture]
    public class GlobalExceptionHandlingMiddlewareTests
    {
        private Mock<ILogger<GlobalExceptionHandlingMiddleware>> _mockLogger;
        private Mock<RequestDelegate> _mockNext;
        private GlobalExceptionHandlingMiddleware _middleware;
        private DefaultHttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
            _mockNext = new Mock<RequestDelegate>();
            _middleware = new GlobalExceptionHandlingMiddleware(_mockNext.Object, _mockLogger.Object);
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }

        #region Constructor Tests

        [Test]
        public void Constructor_ShouldInitializeMiddleware()
        {
            // Act & Assert
            _middleware.Should().NotBeNull();
        }

        #endregion

        #region InvokeAsync Success Tests

        [Test]
        public async Task InvokeAsync_ShouldCallNext_WhenNoExceptionOccurs()
        {
            // Arrange
            _mockNext.Setup(x => x(_httpContext)).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockNext.Verify(x => x(_httpContext), Times.Once);
        }

        #endregion

        #region Exception Handling Tests

        [Test]
        public async Task InvokeAsync_ShouldHandleArgumentException_WithBadRequestStatus()
        {
            // Arrange
            var exceptionMessage = "Invalid argument provided";
            _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Position = 0;
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Be(exceptionMessage);
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task InvokeAsync_ShouldHandleKeyNotFoundException_WithNotFoundStatus()
        {
            // Arrange
            var exceptionMessage = "Resource not found";
            _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(new KeyNotFoundException(exceptionMessage));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _httpContext.Response.Body.Position = 0;
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Be(exceptionMessage);
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task InvokeAsync_ShouldHandleUnauthorizedAccessException_WithUnauthorizedStatus()
        {
            // Arrange
            var exceptionMessage = "Access denied";
            _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(new UnauthorizedAccessException(exceptionMessage));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);

            _httpContext.Response.Body.Position = 0;
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Be("Unauthorized access");
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task InvokeAsync_ShouldHandleInvalidOperationException_WithBadRequestStatus()
        {
            // Arrange
            var exceptionMessage = "Invalid operation";
            _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(new InvalidOperationException(exceptionMessage));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            _httpContext.Response.Body.Position = 0;
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Be(exceptionMessage);
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task InvokeAsync_ShouldHandleGenericException_WithInternalServerErrorStatus()
        {
            // Arrange
            var exceptionMessage = "Unexpected error occurred";
            _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            _httpContext.Response.Body.Position = 0;
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Be("An internal server error occurred");
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        #endregion

        #region Logging Tests

        [Test]
        public async Task InvokeAsync_ShouldLogException_WhenExceptionOccurs()
        {
            // Arrange
            var exception = new ArgumentException("Test exception");
            _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An unhandled exception has occurred")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        #endregion

        #region ErrorResponse Tests

        [Test]
        public void ErrorResponse_ShouldHaveDefaultValues()
        {
            // Act
            var errorResponse = new ErrorResponse();

            // Assert
            errorResponse.StatusCode.Should().Be(0);
            errorResponse.Message.Should().Be(string.Empty);
            errorResponse.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void ErrorResponse_ShouldAllowSettingProperties()
        {
            // Arrange
            var expectedStatusCode = 404;
            var expectedMessage = "Test error message";
            var expectedTimestamp = DateTime.UtcNow.AddMinutes(-5);

            // Act
            var errorResponse = new ErrorResponse
            {
                StatusCode = expectedStatusCode,
                Message = expectedMessage,
                Timestamp = expectedTimestamp
            };

            // Assert
            errorResponse.StatusCode.Should().Be(expectedStatusCode);
            errorResponse.Message.Should().Be(expectedMessage);
            errorResponse.Timestamp.Should().Be(expectedTimestamp);
        }

        #endregion

        #region JSON Serialization Tests

        [Test]
        public async Task InvokeAsync_ShouldSerializeErrorResponse_WithCamelCaseNaming()
        {
            // Arrange
            _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(new ArgumentException("Test message"));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Body.Position = 0;
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            // Verificar que usa camelCase
            responseBody.Should().Contain("statusCode");
            responseBody.Should().Contain("message");
            responseBody.Should().Contain("timestamp");
            
            // No debería contener PascalCase
            responseBody.Should().NotContain("StatusCode");
            responseBody.Should().NotContain("Message");
            responseBody.Should().NotContain("Timestamp");
        }

        #endregion

        #region Integration Tests

        [Test]
        public async Task InvokeAsync_ShouldMaintainResponseIntegrity_ForMultipleExceptionTypes()
        {
            // Arrange
            var exceptions = new Exception[]
            {
                new ArgumentException("Argument error"),
                new KeyNotFoundException("Not found error"),
                new UnauthorizedAccessException("Unauthorized error"),
                new InvalidOperationException("Invalid operation error"),
                new Exception("Generic error")
            };

            var expectedStatusCodes = new[]
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.BadRequest,
                HttpStatusCode.InternalServerError
            };

            for (int i = 0; i < exceptions.Length; i++)
            {
                // Reset context for each test
                _httpContext = new DefaultHttpContext();
                _httpContext.Response.Body = new MemoryStream();
                
                _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(exceptions[i]);

                // Act
                await _middleware.InvokeAsync(_httpContext);

                // Assert
                _httpContext.Response.StatusCode.Should().Be((int)expectedStatusCodes[i]);
                _httpContext.Response.ContentType.Should().Be("application/json");
            }
        }

        #endregion
    }
}