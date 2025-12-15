using AutoFixture;
using FluentAssertions;
using Fundo.Applications.Application.Features.Loans.Commands.CreateLoan;
using Fundo.Applications.Application.Features.Loans.Commands.MakePayment;
using Fundo.Services.Tests.TestHelpers;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Fundo.Services.Tests.Integration
{
    public class LoanManagementControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        private readonly Fixture _fixture;

        private readonly ITestOutputHelper _output;

        private readonly CustomWebApplicationFactory _factory;

        public LoanManagementControllerTests(
            CustomWebApplicationFactory factory,
            ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _fixture = new Fixture();

            _client = factory.CreateClient();

            ConfigureAutoFixture();
        }

        private void ConfigureAutoFixture()
        {
            _fixture.Customize<CreateLoanCommand>(c => c
                .With(x => x.Amount, 10000m)
                .With(x => x.ApplicantName, "Test Applicant"));

            _fixture.Customize<MakePaymentCommand>(c => c
                .With(x => x.Payment, 1000m)
                .With(x => x.Id, "valid-id"));
        }

        [Fact]
        public async Task GetBalances_ShouldReturnExpectedResult()
        {
            // Act
            var response = await _client.GetAsync("/loan");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetLoanById_ShouldReturnNotFound_WhenLoanDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/loan/non-existing-id");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateLoan_ShouldReturnCreated()
        {
            // Arrange
            var loanCommand = _fixture.Create<CreateLoanCommand>();

            // Act
            var response = await _client.PostAsJsonAsync("/loan", loanCommand);

            // Debug
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _output.WriteLine($"Error: {error}");
            }

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task MakePayment_ShouldReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            var loanCommand = _fixture.Create<CreateLoanCommand>();

            var createResponse = await _client.PostAsJsonAsync("/loan", loanCommand);

            if (!createResponse.IsSuccessStatusCode)
            {
                var error = await createResponse.Content.ReadAsStringAsync();
                _output.WriteLine($"Create Error: {error}");
            }

            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var location = createResponse.Headers.Location;
            location.Should().NotBeNull();

            var loanId = location!.ToString().Split('/')[^1];

            var paymentCommand = _fixture.Build<MakePaymentCommand>()
                .With(x => x.Id, loanId)
                .Create();

            // Act
            var response = await _client.PostAsJsonAsync(
                $"/loan/{loanId}/payment",
                paymentCommand);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _output.WriteLine($"Payment Error: {error}");
            }

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task MakePayment_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var paymentCommand = _fixture.Create<MakePaymentCommand>();

            // Act
            var response = await _client.PostAsJsonAsync(
                "/loan/expected-id/payment",
                paymentCommand);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}