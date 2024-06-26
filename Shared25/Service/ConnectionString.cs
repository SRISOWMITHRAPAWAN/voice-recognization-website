using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared25.Service
{
    public class ConnectionString
    {


        public string MstConnection()
        {
            return "Data Source=SOPADYWK000407;Initial Catalog=Akira-Task1;Integrated Security=True";

            //Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InfoDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        }
    }
}
