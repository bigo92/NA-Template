using NUnit.Framework;

namespace NUnitTest.Services
{
    public class UTTempService
    {
        public UTTempService()
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(2)]
        [TestCase(0)]
        [TestCase(1)]
        public void Test1(int a)
        {
            if (a>=0)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
            
        }
    }
}
