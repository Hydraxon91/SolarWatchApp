﻿namespace WeatherApi.Contracts;

public record RegistrationResponse(string Email, string UserName, string? Role, bool Success);