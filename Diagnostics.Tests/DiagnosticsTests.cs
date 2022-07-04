using System;
using System.Linq;
using System.Threading;
using Diagnostics.Interfaces;
using NUnit.Framework;

namespace Diagnostics.Tests
{
    [TestFixture]
    class DiagnosticsTests
    {
        private IDiagnostics Diagnostics { get; set; }
        private IDiagnosticsSettings DiagnosticsSettings { get; } = new DiagnosticsSettings();

        [OneTimeSetUp]
        public void SetUp()
        {
            Diagnostics = new Instances.Diagnostics(DiagnosticsSettings);
        }

        [Test]
        public void ProcessesInfoProviderGetAll()
        {
            var gotFirstEventFlag = false;
            var gotSecondEventFlag = false;

            var firstEvent = new ManualResetEvent(false);
            var secondEvent = new ManualResetEvent(false);

            var methodLatency = TimeSpan.FromSeconds(1);

            Diagnostics.ProcessesInfoProvider.Actual += (s, e) =>
            {
                Assert.IsNotNull(e);
                Assert.Greater(e.Count(), 0);
                if (gotFirstEventFlag)
                {
                    gotSecondEventFlag = true;
                    secondEvent.Set();
                }
                else
                {
                    gotFirstEventFlag = true;
                    firstEvent.Set();
                }
            };

            Assert.IsTrue(firstEvent.WaitOne(DiagnosticsSettings.ProcessesInfoPeriod + methodLatency));
            Assert.IsTrue(gotFirstEventFlag);
            Assert.IsFalse(gotSecondEventFlag);
            Assert.IsTrue(secondEvent.WaitOne(DiagnosticsSettings.ProcessesInfoPeriod + methodLatency));
            Assert.IsTrue(gotSecondEventFlag);
        }
    }

    class DiagnosticsSettings : IDiagnosticsSettings
    {
        public TimeSpan ProcessesInfoPeriod => TimeSpan.FromSeconds(2);
    }
}
