using Domain.Enums;

namespace Domain.Interfaces.Communication;

public interface IBrokerSender
{
    void Send<T>(MessageDestinations messageDestination, T obj);
}