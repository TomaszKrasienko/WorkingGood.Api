namespace Application.DTOs;

public record BaseParameters
{
    private const int _maxPageSize = 20;
    public int PageNumber { get; init; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }
}