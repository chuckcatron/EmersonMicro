using System.Collections.Generic;
using centralProcessing.Models;

namespace centralProcessing.Interfaces
{
    public interface IChannelRepository
    {
        List<Channel> GetChannels();
        bool AddChannel(string name);
        void DeleteChannel(string name);
    }
}