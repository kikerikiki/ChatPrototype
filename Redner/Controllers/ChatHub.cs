using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using LAPTemplateMVC.Models.dboSchema;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using LAPTemplateMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LAPTemplateMVC.Hubs
{
    public class ChatHub : Hub
    {
        private readonly chatlerContext _context;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(chatlerContext context, ILogger<ChatHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _logger.LogInformation("Client connected: " + Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            _logger.LogInformation("Client disconnected: " + Context.ConnectionId);
        }

        public async Task SendMessage(string sender, string receiver, string encryptedMessage)
        {
            try
            {
                _logger.LogInformation($"SendMessage called with sender: {sender}, receiver: {receiver}, encryptedMessage: {encryptedMessage}");

                var senderUser = await _context.Chatuser.SingleOrDefaultAsync(u => u.Username == sender && u.Valid == 1);
                var receiverUser = await _context.Chatuser.SingleOrDefaultAsync(u => u.Username == receiver && u.Valid == 1);

                if (senderUser != null && receiverUser != null)
                {
                    await _context.Procedures.MessageInsertAsync(
                        senderUser.Chatuserid,
                        receiverUser.Chatuserid,
                        encryptedMessage,
                        "dummyKey", // Dummy Key, in einer realen App sollte dies der echte Schlüssel sein
                        DateTime.Now,
                        1 // Valid
                    );

                    await Clients.User(receiver).SendAsync("ReceiveMessage", sender, encryptedMessage);
                }
                else
                {
                    if (senderUser == null)
                    {
                        _logger.LogWarning($"Sender not found: {sender}");
                        await Clients.Caller.SendAsync("ReceiveMessage", "System", "Sender not found.");
                    }
                    if (receiverUser == null)
                    {
                        _logger.LogWarning($"Receiver not found: {receiver}");
                        await Clients.Caller.SendAsync("ReceiveMessage", "System", "Receiver not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendMessage");
                await Clients.Caller.SendAsync("ReceiveMessage", "System", $"Error: {ex.Message}");
            }
        }
    }
}
