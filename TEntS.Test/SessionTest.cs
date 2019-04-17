using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Moq;
using TEntS.BLL.Interfaces;
using TEntS.BLL.Session;

namespace TEntS.Test
{
    [TestFixture]
    public class SessionTest
    {
        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void Test_Login()
        {
            ISession target = new Session(new SessionBase());
            ulong sessionId = 0;
            target.Login("admin", "", "127.0.0.1", false, ref sessionId);
            //ISession _target = new Session(new SessionBase());
            //Mock<ISession> _handler = new Mock<ISession>();
            //ulong sessionId = ulong.MinValue + 1;
            //_handler.Setup(m => m.Login("admin", "12345a$", "127.0.0.1", false, ref sessionId));
            //_target.Login("admin", "12345a$", "127.0.0.1", false, ref sessionId);
            //_handler.VerifyAll();
            //Assert.AreEqual(sessionId, ulong.MinValue + 1);
        }
    }
}
