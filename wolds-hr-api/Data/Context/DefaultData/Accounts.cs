using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.DefaultData;

public class Accounts
{
    public static List<Account> GetAccountDefaultData()
    {
        return
        [
            new() {
                Id = 1,
                FirstName = "test",
                LastName = "test",
                Email = "Test100@hotmail.com",
                PasswordHash = "$2a$11$H.p94nv0W1/wdlYd4L3/S.q6SUGSh/GKQ88PYGIMW/L1zZh9O2k4e",
                AcceptTerms = false,
                Role = 0,
                VerificationToken = "",
                Verified = new DateTime(),
                ResetToken = "U6hLv0HSty17HSw8YR33MwQTXpWEDIO7ylVZke0TUHW7EtGNltlzNQ44",
                ResetTokenExpires = new DateTime(),
                Created = new DateTime()
            }
        ];
    }
}