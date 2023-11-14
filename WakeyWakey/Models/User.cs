using System;

namespace WakeyWakey.Models;

public class User
{
    public int Id { get; set; }
    public required String Username { get; set; }
    public required String Password { get; set; }
    public String? Email { get; set; }

}
