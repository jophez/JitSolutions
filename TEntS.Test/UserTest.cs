using NUnit.Framework;
using TEntS.BLL.Interfaces;
using TEntS.BLL.Session;
using TEntS.BLL.User;
using TEntS.Types;

namespace TEntS.Test
{
    [TestFixture]
    public class UserTest
    {
        [SetUp]
        public void Init()
        { }

        [Test]
        public void Test_RetrieveAdminPassword()
        {
            string userName = "Admin";
            ulong sessionId = 0;
            IUser iUser = new User(new UserBase());
            string password = iUser.RetrieveUserPassword(sessionId, "Admin");
            Assert.AreEqual("12345a$", password);
        }

        [Test]
        public void Test_AddUser()
        {
            string userName = "Admin";
            ulong sessionId = 0;
            int userId = 0;
            UserDetails userObj = new UserDetails() { FirsName = "Jose", LastName = "Rizal", MiddleName = "Mercado", Password = "rizal!@", UserName = "jrizal" };
            IUser iUser = new User(new UserBase());
            ISession session = new Session(new SessionBase());
            session.Login(userName, "12345a$", "127.0.0.1", false, ref sessionId);
            iUser.Add(sessionId, userObj, 2, ref userId);

        }
    }
}