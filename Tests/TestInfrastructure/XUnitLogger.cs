using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestInfrastructure
{
    public interface IXUnitLogger
    {
        void WriteLine(string message);
        string GetAllMessages { get; }
    }

    public class XUnitLogger
    {
        private readonly IMessageSink _messageSink;
        private readonly ITestOutputHelper _outputHelper;
        private readonly List<string> _allMessages = new List<string>();


        public XUnitLogger(IMessageSink messageSink)
        {
            _messageSink = messageSink;
        }

        public XUnitLogger(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public void WriteLine(string message)
        {
            _allMessages.Add(message);
            var enrichedMessage = $"[{DateTime.Now:HH:mm:ss.fff tt}] {message}";

            if (_messageSink != null)
            {
                _messageSink.OnMessage(new DiagnosticMessage(enrichedMessage));
                return;
            }

            if (_outputHelper != null)
            {
                _outputHelper.WriteLine(enrichedMessage);
                return;
            }
            Debug.WriteLine("OOPS -- how did you get here?");
        }
    }
}
