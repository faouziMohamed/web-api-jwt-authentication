namespace authTests.Api.Models;

static public class UserConstants
{
  static public readonly List<UserModel> Users = new()
  {
    new UserModel
    {
      Username = "jason_admin",
      Password = "password",
      Role = "Admin",
      EmailAddress = "json.admin@email.com",
      Surname = "Bryant",
      GivenName = "Jason"
    },
    new UserModel
    {
      Username = "faouzi_bat",
      Password = "password",
      Role = "User",
      EmailAddress = "faouzi.bat@email.com",
      Surname = "Bat", GivenName = "Faouzi"
    }
  };
}
