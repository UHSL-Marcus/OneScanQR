using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLControls.Tests
{
    [TestClass()]
    public class SetTests
    {
        [TestMethod()]
        public void doInsertTest()
        {
            Dictionary<string, object> insert = new Dictionary<string, object>();
            insert.Add("nameo", "this name");
            insert.Add("Anumber", 22);
            
            FieldInfo[] fields = Set.doInsert("new name", insert).GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                string s = fields[i].Name;
            }

            Assert.Fail();
        }
    }
}