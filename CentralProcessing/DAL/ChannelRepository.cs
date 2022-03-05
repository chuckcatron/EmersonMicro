using System;
using centralProcessing.Interfaces;
using centralProcessing.Models;
using System.Collections.Generic;
using System.Linq;

namespace centralProcessing.DAL
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly centraldbContext _context;

        public ChannelRepository(centraldbContext context)
        {
            _context = context;
        }

        public List<Channel> GetChannels()
        {
            return _context.Channels.ToList();
        }
        public bool AddChannel(string name)
        {
            var channel = new Channel() {Name = name};
            _context.Add(channel);
            var added = _context.SaveChanges();
            return added == 1;
        }
        public void DeleteChannel(string name)
        {
            var channel = GetChannels().FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (channel != null)
            {
                _context.Channels.Remove(channel);
                _context.SaveChanges();
            }
        }
    }
}