namespace ModelsLibrary.Models;

public record UserEvent(
    string EventType,
    int UserId,
    string Name,
    DateTime Timestamp
)
{
    // Constructor vacío para deserialización
    public UserEvent() : this("", 0, "", DateTime.MinValue)
    {
    }

    // Constructor con User
    public UserEvent(User user) : this("UserCreated", user.Id, user.Name, DateTime.UtcNow)
    {
    }
}