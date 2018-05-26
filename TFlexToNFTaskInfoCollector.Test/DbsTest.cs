using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbsShit;

namespace TFlexToNFTaskInfoCollector.Test
{
    [TestClass]
    public class DbsTest
    {
        [TestMethod]
        public void LoadDbs()
        {
            var file = File.OpenRead("nested.dbs");
            using (var writer = new BinaryReader(File.OpenRead("nested.dbs")))
                Debug.WriteLine(DbsShit.DbsShit.ReadDbs(writer));
        }
    }
}
