using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IWorkerRepository
{
    Task<Worker> GetWorkerByIdAsync(Guid id);
    Task<IEnumerable<Worker>> GetAllWorkersAsync();
    Task AddWorkerAsync(Worker worker);
    Task UpdateWorkerAsync(Worker worker);
    Task DeleteWorkerAsync(Guid id);
}