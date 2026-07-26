using Application.Interfaces;
using System;

namespace Application.UnitTests.Common
{
    public class DummyDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
