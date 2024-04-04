﻿using GenerateTicket.Common;
using GenerateTicket.Models;

namespace GenerateTicket.Services
{
    public class TicketInfoService : ITicketInfoService
    {
        private readonly AppDbContext _dbContext;

        public TicketInfoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TicketInfo> AddTicketInfo(TicketInfo ticketInfo, int attempt)
        {
            if (ticketInfo is not null)
            {
                ticketInfo.TicketNumber = StringGenerator.Generate();

                if (attempt < 3) //Force excption before 3 retries
                    ticketInfo = null;

                await _dbContext.TicketInfo.AddAsync(ticketInfo);
                await _dbContext.SaveChangesAsync();
            }

            return ticketInfo;
        }

        public bool RemoveTicketInfo(string TicketId)
        {
            var ticketInfoObj = _dbContext.TicketInfo.FirstOrDefault(t => t.TicketId == TicketId);
            if (ticketInfoObj is not null)
            {
                _dbContext.TicketInfo.Remove(ticketInfoObj);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
