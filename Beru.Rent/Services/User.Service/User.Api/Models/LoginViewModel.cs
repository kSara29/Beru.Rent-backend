﻿namespace User.Api.Models;

public class LoginViewModel
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public string? ReturnUrl { get; init; }
}