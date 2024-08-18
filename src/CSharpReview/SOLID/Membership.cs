using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID
{
    public class Membership
    {
        public void CreateAccount(string username, string email, string password)
        {
            string encryptedPassword = EncryptPassword(password);

            // more code
        }

        private string EncryptPassword(string password)
        {
            throw new NotImplementedException();
        }
    }
}
