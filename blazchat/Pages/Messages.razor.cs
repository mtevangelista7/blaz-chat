﻿using Microsoft.AspNetCore.Components;

namespace blazchat.Pages;

public class MessagesBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }
}