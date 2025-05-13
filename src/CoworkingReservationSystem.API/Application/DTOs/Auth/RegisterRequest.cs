﻿using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, MinLength(6)]
    public string Name { get; set; }
    [Required, MaxLength(100)]
    public string Password { get; set; }
}