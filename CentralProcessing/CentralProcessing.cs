using System;
using centralProcessing.Helpers;
using centralProcessing.Interfaces;
using centralProcessing.Models;
using Microsoft.Extensions.Logging;

namespace centralProcessing
{
    public class CentralProcessing
    {
        private readonly ILogger _logger;
        private readonly IScreenHelper _screenHelper;
        private readonly IFlowRouting _flowRouting;
        private readonly IPubSocket _pubSocket;

        public CentralProcessing(ILogger<CentralProcessing> logger, IScreenHelper screenHelper, IFlowRouting flowRouting, IPubSocket pubSocket)
        {
            _logger = logger;
            _screenHelper = screenHelper;
            _flowRouting = flowRouting;
            _pubSocket = pubSocket;
        }
        internal void Run()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);
            _screenHelper.Clear();

            var flow = new UserFlow();

            while (flow.NextStep != "bye")
            {
                _pubSocket.OpenConnection();
                
                flow = _flowRouting.GetRolling();
            }

            _logger.LogInformation("Application Ended at {dateTime}", DateTime.UtcNow);
        }
    }
}