using System;

namespace WakeyWakey.Models;

public class UserModel
{
    public int Id { get; set; }
    public String Username { get; set; }
    public String Password { get; set; }
    public String Email { get; set; }
    public String Salt { get; set; }
}
