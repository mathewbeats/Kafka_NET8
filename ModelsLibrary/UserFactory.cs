using ModelsLibrary.Models;

namespace ModelsLibrary;

public static class UserFactory
{
    public static List<User> CreateUsers()
    {
        var user1 = User.CreateUser(
            id: 1,
            name: "Juan",
            lastName: "Pérez",
            address: "Madrid, España",
            phoneNumber: 123456789,
            birthDateTime: new DateTime(1990, 1, 1),
            loggDate: DateTime.UtcNow,
            idNumber: "12345678A",
            premiumUser: false
        );

        var user2 = User.CreateUser(
            id: 2,
            name: "María",
            lastName: "Gómez",
            address: "Barcelona, España",
            phoneNumber: 987654321,
            birthDateTime: new DateTime(1985, 5, 10),
            loggDate: DateTime.UtcNow,
            idNumber: "87654321B",
            premiumUser: true
        );

        return new List<User> { user1, user2 };
    }
}