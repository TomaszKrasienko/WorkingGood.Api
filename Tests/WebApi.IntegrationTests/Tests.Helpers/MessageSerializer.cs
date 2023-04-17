using Application.DTOs;
using Newtonsoft.Json;

namespace WebApi.IntegrationTests.Tests.Helpers;

public static class MessageSerializer
{
    public static BaseMessageDto GetBaseMessageDto(this string content)
    {
        BaseMessageDto baseMessageDto = JsonConvert.DeserializeObject<BaseMessageDto>(content)
            ?? throw new ArgumentException("Can not deserialize message");
        return baseMessageDto;
    }
}