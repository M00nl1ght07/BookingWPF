using System;

public class User
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int BonusPoints { get; set; }
}