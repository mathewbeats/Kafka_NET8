namespace ModelsLibrary.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public int PhoneNumber { get; set; }
    public DateTime BirthDateTime { get; set; }
    public DateTime LoggDateTime { get; set; }
    public string IdNumber { get; set; }
    public bool PremiumUser { get; set; }

    public User(int id, string name, string lastName, string address, int phoneNumber, DateTime birthDateTime,
        DateTime loggDate, string idNumber, bool premiumUser)
    {
        this.Id = id;
        this.Name = name;
        this.LastName = lastName;
        this.Address = address;
        this.PhoneNumber = phoneNumber;
        this.BirthDateTime = birthDateTime;
        this.LoggDateTime = loggDate;
        this.IdNumber = idNumber;
        this.PremiumUser = premiumUser;
    }


    public static User CreateUser(int id, string name, string lastName, string address, int phoneNumber,
        DateTime birthDateTime,
        DateTime loggDate, string idNumber, bool premiumUser)
    {
        if (id <= 0 || string.IsNullOrEmpty(name))
            throw new ArgumentException(nameof(name), "Name or Id cannot be null");

        return new User(id, name, lastName, address, phoneNumber, birthDateTime, loggDate, idNumber, premiumUser);
    }

    public static User[] CreateManyUser(params User[] users)
    {
        if (users.Length == 0 || !users.Any())
            throw new ArgumentException(nameof(users), "Users is not a valid array");

        return users;
    }
}