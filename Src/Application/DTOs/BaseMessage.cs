namespace Application.DTOs;

public class BaseMessage
{
    public string? Message { get; set; }
    public object? Object { get; set; }
    public object? Errors { get; set; }
}