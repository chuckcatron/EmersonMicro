using centralProcessing.Models;

namespace centralProcessing.Interfaces
{
    public interface IFlowRouting
    {
        string NextStep(string nextStep);
        UserFlow GetRolling();
    }
}