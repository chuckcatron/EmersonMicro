using System;
using System.Collections.Generic;
using centralProcessing.Helpers;
using centralProcessing.Interfaces;
using centralProcessing.Models;
using Microsoft.Extensions.Logging;
using NetMQ.Sockets;

namespace centralProcessing
{
    public class CentralProcessing
    {
        private readonly ILogger _logger;
        private readonly IScreenHelper _screenHelper;
        private readonly IFlowRouting _flowRouting;
        public CentralProcessing(ILogger<CentralProcessing> logger, IScreenHelper screenHelper, IFlowRouting flowRouting)
        {
            _logger = logger;
            _screenHelper = screenHelper;
            _flowRouting = flowRouting;
        }
        internal void Run()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);
            _screenHelper.Clear();

            var flow = new UserFlow();

            while (flow.NextStep != "bye")
            {
                using var pubSocket = new PublisherSocket();
                pubSocket.Options.SendHighWatermark = 1000;
                pubSocket.Bind("tcp://*:3000");

                flow = _flowRouting.GetRolling(pubSocket);
            }

            _logger.LogInformation("Application Ended at {dateTime}", DateTime.UtcNow);
        }
    }
}