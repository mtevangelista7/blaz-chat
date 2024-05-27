using blazchat.Entities;
using blazchat.Services;
using Microsoft.AspNetCore.Mvc;

namespace blazchat.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
    {
        var user1Id = request.User1Id;
        var user2Id = request.User2Id;

        var chatId = await _chatService.CreateNewChat(user1Id, user2Id);
        return Ok(chatId);
    }

    [HttpGet("validate/chatId={chatId:guid}/userId={userId:guid}")]
    public async Task<IActionResult> ValidateChat(Guid chatId, Guid userId)
    {
        var isValid = await _chatService.ValidateChat(chatId, userId);

        if (!isValid)
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpGet("getActiveChats/userId={userId:guid}")]
    public async Task<IActionResult> GetActiveChats(Guid userId)
    {
        var activeChats = await _chatService.GetActiveChats(userId);
        return Ok(activeChats);
    }
}