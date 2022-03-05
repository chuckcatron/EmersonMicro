using centralProcessing.Models;
using NetMQ.Sockets;

namespace centralProcessing.Interfaces
{
    public interface IFlowRouting
    {
        string NextStep(string nextStep);
        UserFlow GetRolling(PublisherSocket pubSocket);
    }
}