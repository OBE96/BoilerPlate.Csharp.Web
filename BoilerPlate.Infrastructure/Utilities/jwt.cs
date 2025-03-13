using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Infrastructure.Utilities
{
    public class Jwt
    {
        public string? SecretKey { get; set; }

        public int ExpireInMinute { get; set; }
    }
}
