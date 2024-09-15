using Hangfire;
using JobStreamline.Entity.Enum;
using JobStreamline.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JobStreamline.Api;
public static class Jobs
{
    static IServiceProvider _serviceProvider = null;
    static IConfiguration _configuration = null;
    public static void StartJobs(this IApplicationBuilder app, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        app.StartRecurringJobs();
    }

    private static void StartRecurringJobs(this IApplicationBuilder app)
    {
        if (Convert.ToBoolean(_configuration["Jobs:DeactivatedExpiredJobs:Active"]))
        {
            RecurringJob.AddOrUpdate("DeactivatedExpiredJobs", () => DeactivatedExpiredJobs(), $"*/{Convert.ToInt64(_configuration["Jobs:DeactivatedExpiredJobs:CycleMinute"])} * * * *");
        }
    }

    /// <summary>
    /// Bitiş tarihi geçmiş kapatmaların durumunu pasife çeker.
    /// </summary>
    public static void DeactivatedExpiredJobs()
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            using (IJobService jobService = scope.ServiceProvider.GetRequiredService<IJobService>())
            {
                var expiredJobs = jobService.GetAll(s => s.Status == JobStatus.Active && s.ExpiryDate < DateTime.Now).Include(w => w.Company).ToList();
                expiredJobs?.ForEach(x =>
                {
                    x.Status = JobStatus.Closed;
                    x.Company.JobPostingLimit++;
                });
                jobService.Update(expiredJobs);
            }
        }
    }

}