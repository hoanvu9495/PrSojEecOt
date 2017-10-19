using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Common;
using System.Collections.Generic;
using Web.Custom;
using Model.eAita;

using Business.Business;

namespace UnitTestWeb
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSalaryConvert()
        {
            TblDataKetXuatBusiness bs = new TblDataKetXuatBusiness(new Entities());
            decimal? dcm= 1000;
            string rs = "1,000";
            string rs1 = bs.ConvertSalary(dcm.ToString());
            Assert.AreEqual(rs, rs1);
        }
        [TestMethod]
        public void TestDemo()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
