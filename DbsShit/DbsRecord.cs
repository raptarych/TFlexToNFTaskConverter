using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbsShit
{
    public class DbsRecord
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public DbsRecordType Type { get; set; }
    }
}
