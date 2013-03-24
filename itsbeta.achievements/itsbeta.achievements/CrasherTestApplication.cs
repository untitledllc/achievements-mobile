using System;
using Android.App;
using Android.Runtime;
using Mono.Android.Crasher;
using Mono.Android.Crasher.Attributes;
using Mono.Android.Crasher.Data.Submit;

namespace itsbeta.achievements
{
    [Application]
    [Crasher(UseCustomData = true, CustomDataProviders = new[] { typeof(TestAppCustomDataReportProvider) })]
    [GoogleFormReporterSettings("dHh2V05Od1ZFdlU1QjRBUDlSM0hDZlE6MQ")]
    public class CrasherTestApplication : Application
    {
        public CrasherTestApplication(IntPtr doNotUse, JniHandleOwnership transfer)
            : base(doNotUse, transfer)
        {
        }

        public CrasherTestApplication()
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrashManager.Initialize(this);
            CrashManager.AttachSender(() => new GoogleFormSender());
        }
    }
}