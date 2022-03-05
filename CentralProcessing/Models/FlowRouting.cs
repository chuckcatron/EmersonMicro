using System;
using System.Collections.Generic;
using System.Linq;
using centralProcessing.Helpers;
using centralProcessing.Models;
using centralProcessing.Interfaces;
using NetMQ;
using NetMQ.Sockets;

namespace centralProcessing.Models
{
    class FlowRouting : IFlowRouting
    {
        private readonly IScreenHelper _screenHelper;
        private readonly IChannelRepository _channelRepository;
        private PublisherSocket _pubSocket;
        public FlowRouting(IScreenHelper screenHelper, IChannelRepository channelRepository)
        {
            _screenHelper = screenHelper;
            _channelRepository = channelRepository;
        }

        public UserFlow GetRolling(PublisherSocket pubSocket)
        {
            _pubSocket = pubSocket;
            return StartFlow();
        }

        protected UserFlow StartFlow()
        {
            var flow = new UserFlow();
            var next = GetNext();

            if (next.Equals("bye", StringComparison.OrdinalIgnoreCase))
            {
                flow.NextStep = "bye";
                return flow;
            }

            DoMainMenu(flow);
            flow = StartFlow();
            if (flow.NextStep == "bye") return flow;

            return flow;
        }

        private string GetNext()
        {
            _screenHelper.Clear();
            _screenHelper.Print("Welcome to Emerson Central Processing");
            _screenHelper.Print("Press <Enter> to continue or \"bye\" to quit ");
            var next = Console.ReadLine();
            return next;
        }
        
        public string NextStep(string nextStep)
        {
            var input = "";

            Console.WriteLine($"{nextStep} ");
            input = Console.ReadLine();
            return input;
        }

        private string DoMainMenu(UserFlow flow)
        {
            var input = "";
            while (input != "3")
            {
                _screenHelper.Clear();
                _screenHelper.Print("Would you like to do next? ");
                _screenHelper.Print("1 = Manage Channels");
                _screenHelper.Print("2 = Publish");
                _screenHelper.Print("3 = Back");
                input = Console.ReadLine();


                if (input == "1")
                {
                    DoManageChannels();
                }
                else if (input == "2")
                {
                    DoPublish();
                }
            }

            return input;
        }

        private void DoPublish()
        {
            var input = "";
            while (input != "2")
            {
                _screenHelper.Clear();
                _screenHelper.Print("Would you like to do next? ");
                _screenHelper.Print("1 = Publish a message");
                _screenHelper.Print("2 = Back");
                input = Console.ReadLine();


                if (input == "1")
                {
                    DoPublishMessage();
                }
            }
        }

        private void DoPublishMessage()
        {
            _screenHelper.Clear();
            _screenHelper.Print("Channel list");
            _screenHelper.Print("------------");
            var channels = _channelRepository.GetChannels();
            foreach (var channel in channels)
            {
                _screenHelper.Print(channel.Name);
            }
            _screenHelper.Print("------------");
            _screenHelper.Print("");
            _screenHelper.Print("Enter a channel to publish a message to or <Enter> to send to All clients");
            var name = _screenHelper.GetResponse();
            name = name == "" ? "All" : name;
            _screenHelper.Print("Enter a message to publish");
            var message = _screenHelper.GetResponse();

            if (name == "All")
            {
                foreach (var channel in channels)
                {
                    _pubSocket.SendMoreFrame(channel.Name).SendFrame(message);
                }
            }
            else
            {
                foreach (var channel in channels)
                {
                    if (name.Equals(channel.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _pubSocket.SendMoreFrame(channel.Name).SendFrame(message);
                        break;
                    }
                }
            }
        }

        private void DoManageChannels()
        {
            var input = "";
            while (input != "4")
            {
                _screenHelper.Clear();
                _screenHelper.Print("Would you like to do next? ");
                _screenHelper.Print("1 = List Channels");
                _screenHelper.Print("2 = Add Channel");
                _screenHelper.Print("3 = Delete Channel");
                _screenHelper.Print("4 = Back");
                input = Console.ReadLine();


                if (input == "1")
                {
                    DoListChannels();
                }
                else if (input == "2")
                {
                    DoAddChannel();
                }
                else if (input == "3")
                {
                    DoDeleteChannel();
                }
            }
        }

        private void DoListChannels()
        {
            _screenHelper.Clear();
            _screenHelper.Print("Channel list");
            _screenHelper.Print("------------");
            var channels = _channelRepository.GetChannels();
            if (channels.Count > 0)
            {
                foreach (var channel in channels)
                {
                    _screenHelper.Print(channel.Name);
                }
            }
            else
            {
                _screenHelper.Print("No channels found");
                _screenHelper.Print("Press <Enter> to go back so you can add one...");
            }

            _screenHelper.Print("------------");
            _screenHelper.Print("");
            _screenHelper.Print("Press <Enter> to continue");
            _screenHelper.GetResponse();
        }

        private void DoAddChannel()
        {
            _screenHelper.Clear();
            _screenHelper.Print("Channel name? ");
            _screenHelper.Print("Enter channel name or <Enter> to go back");
            var name = Console.ReadLine();
            _channelRepository.AddChannel(name);
        }

        private void DoDeleteChannel()
        {
            _screenHelper.Clear();
            _screenHelper.Print("Channel name to delete? ");
            _screenHelper.Print("Enter channel name or <Enter> to go back");
            var name = Console.ReadLine();
            _channelRepository.DeleteChannel(name);
        }

        public void HandleInvalidResponse()
        {
            Console.WriteLine("---Try Again---");
            Console.WriteLine("<Return to continue>");
            Console.ReadLine();
        }
    }

    public class UserFlow
    {
        public string NextStep { get; set; }
    }
}
