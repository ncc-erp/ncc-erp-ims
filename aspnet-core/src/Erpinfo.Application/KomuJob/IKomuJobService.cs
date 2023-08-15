using Abp.Application.Services;
using Abp.Dependency;
using Erpinfo.KomuJob.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erpinfo.KomuJob
{
    public interface IKomuJobService : IApplicationService, ITransientDependency
    {
        Task<object> CreateJob(CreateJobDTO createJobDto);
        Task<IEnumerable<JobDTO>> GetJobs(string mail);
        Task<object> UpdateJobStatus(UpdateJobStatusDto updateJobStatusDto);
    }
}
