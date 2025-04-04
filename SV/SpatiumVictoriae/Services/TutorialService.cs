using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpatiumVictoriae.Services
{
    public class TutorialService : ITutorialService
    {
        private readonly ILogger<TutorialService> _logger;

        // In-memory storage mapping a user ID to their completed tutorial steps.
        private readonly ConcurrentDictionary<string, HashSet<string>> _tutorialProgress = new ConcurrentDictionary<string, HashSet<string>>();

        public TutorialService(ILogger<TutorialService> logger)
        {
            _logger = logger;
        }

        public Task MarkStepCompletedAsync(string userId, string stepName)
        {
            var steps = _tutorialProgress.GetOrAdd(userId, _ => new HashSet<string>());
            steps.Add(stepName);
            _logger.LogInformation("User {UserId} completed tutorial step: {StepName}", userId, stepName);
            return Task.CompletedTask;
        }

        public Task<bool> IsStepCompletedAsync(string userId, string stepName)
        {
            if (_tutorialProgress.TryGetValue(userId, out var steps))
            {
                bool isCompleted = steps.Contains(stepName);
                _logger.LogInformation("Tutorial step check for user {UserId}: Step {StepName} completed? {IsCompleted}", userId, stepName, isCompleted);
                return Task.FromResult(isCompleted);
            }
            _logger.LogInformation("No tutorial progress found for user {UserId}", userId);
            return Task.FromResult(false);
        }

        public Task<IEnumerable<string>> GetCompletedStepsAsync(string userId)
        {
            if (_tutorialProgress.TryGetValue(userId, out var steps))
            {
                _logger.LogInformation("Returning completed steps for user {UserId}", userId);
                return Task.FromResult(steps.AsEnumerable());
            }
            _logger.LogInformation("No completed steps found for user {UserId}", userId);
            return Task.FromResult(Enumerable.Empty<string>());
        }

        public Task ResetTutorialAsync(string userId)
        {
            _tutorialProgress[userId] = new HashSet<string>();
            _logger.LogInformation("Tutorial progress reset for user {UserId}", userId);
            return Task.CompletedTask;
        }
    }
}
