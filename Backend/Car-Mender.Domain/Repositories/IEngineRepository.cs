using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IEngineRepository
{
    Task<Engine> GetEngineByIdAsync(Guid id);
}