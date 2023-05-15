using Application.ViewModels.Base;
using MediatR;

namespace Application.DTOs;

public record BaseMessageDto 
{
    public string? Message { get; init; }
    public object? Object { get; init; }
    public object? Errors { get; init; }
    public MetaDataVm? MetaData { get; init; }

    public bool IsSuccess()
    {
        return Errors == null;
    }
}
