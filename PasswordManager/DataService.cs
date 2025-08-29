using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class DataService
    {
        string ConnectionString { get; set; }

        public DataService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task GetListOfPasswords(string group)
        {

        }

        public async Task GetPasswordById(int id, int? subid)
        {

        }

        public async Task SavePassword(int? id)
        {

        }

        public async Task DiactivatePassword(int id, int subid)
        {

        }
    }
}
