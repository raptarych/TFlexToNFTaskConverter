using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbsShit
{
    public class DbsShit
    {
        public static string ReadDbs(BinaryReader stream)
        {
            var record = ReadRecord(stream);
            return record.ToString();
        }

        private static DbsRecord ReadRecord(BinaryReader stream)
        {
            var record = new DbsRecord();
            record.Size = stream.ReadInt16();
            stream.Read(new byte[5], 0, 4);
            stream.ReadInt16();
            stream.ReadInt16();
            stream.ReadInt16();
            var typeId = stream.ReadInt16();
            record.Type = (DbsRecordType) typeId;
            stream.ReadInt16();
            return record;
        }
    }
}
