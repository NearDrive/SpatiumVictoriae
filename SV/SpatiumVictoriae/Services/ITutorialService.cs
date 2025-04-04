using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpatiumVictoriae.Services
{
    public interface ITutorialService
    {
        // Marks a tutorial step as completed for the given user.
        Task MarkStepCompletedAsync(string userId, string stepName);

        // Checks whether a specific tutorial step is completed.
        Task<bool> IsStepCompletedAsync(string userId, string stepName);

        // Retrieves a list of all completed tutorial steps for the given user.
        Task<IEnumerable<string>> GetCompletedStepsAsync(string userId);

        // Resets the tutorial progress for the given user.
        Task ResetTutorialAsync(string userId);
    }
}
