using System;
using centralProcessing.Interfaces;
using centralProcessing.Models;
using NetMQ;
using NetMQ.Sockets;

namespace centralProcessing.MessageCenter
{
    public class PubSocket: IPubSocket
    {
        private PublisherSocket _publisherSocket;
        public PubSocket( ){}
        public PublisherSocket OpenConnection()
        {
            _publisherSocket = new PublisherSocket();
            _publisherSocket.Options.SendHighWatermark = 1000;
            _publisherSocket.Bind("tcp://*:3000");
            return _publisherSocket;
        }

        public void CloseConnection()
        {
            _publisherSocket.Close();
        }

        public void Send(Channel channel, String message)
        {
            _publisherSocket.SendMoreFrame(channel.Name).SendFrame(message);
        }
    }
}