using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IWorkerRepository
{
    Task<Worker> GetWorkerByIdAsync(Guid id);
}