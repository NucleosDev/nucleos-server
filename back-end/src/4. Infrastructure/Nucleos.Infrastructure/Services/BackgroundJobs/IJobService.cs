namespace Nucleos.Infrastructure.Services.BackgroundJobs;
public interface IJobService { void Enqueue(string job, object? args = null); void Schedule(string job, TimeSpan delay, object? args = null); }
