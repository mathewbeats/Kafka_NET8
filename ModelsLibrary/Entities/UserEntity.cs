using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary.Entities;

public class UserEntity
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public int PhoneNumber { get; set; }
    public DateTime BirthDateTime { get; set; }
    public DateTime LoggDateTime { get; set; }
    public string IdNumber { get; set; }
    public bool PremiumUser { get; set; }
}