using Microsoft.Extensions.Configuration;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.Infrastructure.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MatchingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _httpClient.BaseAddress = new Uri(
                _configuration["MLService:BaseUrl"]!);
        }

        public async Task TriggerApplicantMatchAsync(int applicantId)
        {
            var response = await _httpClient.PostAsync(
                $"/match/applicant/{applicantId}",
                null);
            response.EnsureSuccessStatusCode();
        }

        public async Task TriggerJobMatchAsync(int jobId)
        {
            var response = await _httpClient.PostAsync(
                $"/match/job/{jobId}",
                null
            );

            response.EnsureSuccessStatusCode();
        }
    }
}