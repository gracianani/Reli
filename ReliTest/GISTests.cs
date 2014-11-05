using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;

namespace ReliTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class GISTests
    {
        private string GIS_Service_Url = "192.168.57.253";
        private int portNumber = 9001;

        public GISTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CanReadGISLocation()
        {
            UdpClient udpClient = new UdpClient(portNumber);
            try
            {
                udpClient.Connect(GIS_Service_Url, portNumber);

                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes( new char[2] { Convert.ToChar(1), Convert.ToChar(111) });
                
                udpClient.Send(sendBytes, sendBytes.Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                var year = Convert.ToInt32(receiveBytes[0]) + 2000;
                var month = Convert.ToInt32(receiveBytes[1]);
                var day = Convert.ToInt32(receiveBytes[2]);
                var hour = Convert.ToInt32(receiveBytes[3]);
                var minute = Convert.ToInt32(receiveBytes[4]);
                var second = Convert.ToInt32(receiveBytes[5]);
                var latitude = BitConverter.ToDouble(receiveBytes, 6);
                var longitude = BitConverter.ToDouble(receiveBytes, 14);

                udpClient.Close();

                Assert.AreEqual(year, 2014);
                Assert.AreEqual(month, 3);
                Assert.AreEqual(day, 13);
                Assert.AreEqual(hour, 14);
                Assert.AreEqual(minute, 2);
                Assert.AreEqual(second, 49);
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
