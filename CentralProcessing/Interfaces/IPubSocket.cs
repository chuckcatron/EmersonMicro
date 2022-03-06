using System;
using centralProcessing.Models;
using NetMQ.Sockets;

namespace centralProcessing.Interfaces
{
    public interface IPubSocket
    {
        PublisherSocket OpenConnection();
        void CloseConnection();

        void Send(Channel channel, String message);
    }
}