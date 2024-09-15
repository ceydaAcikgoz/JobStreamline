

using System;
using JobStreamline.Entity;

namespace JobStreamline.Service;

public interface IJobService : IService<Job>
{
    OutputJobDto? Get(Guid Id);
    OutputJobDto Create(InputJobDTO InputJobDTO);
    OutputJobDto Update(InputJobDTO InputJobDTO);
}
