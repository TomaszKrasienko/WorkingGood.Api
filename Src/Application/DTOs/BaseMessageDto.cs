namespace Application.DTOs;

public class BaseMessageDto
{
    public string? Message { get; set; }
    public object? Object { get; set; }
    public object? Errors { get; set; }

    public bool IsSuccess()
    {
        return Errors == null;
    }
}
