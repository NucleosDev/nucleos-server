using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Infrastructure.Services.DateTime;

public class DateTimeService : IDateTime
{
    public System.DateTime Now => global::System.DateTime.Now;
    public System.DateTime UtcNow => global::System.DateTime.UtcNow;
}