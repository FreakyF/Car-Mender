using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IEngineRepository
{
    Task<Engine> GetEngineByIdAsync(Guid id);
    Task<IEnumerable<Engine>> GetAllEnginesAsync();
    Task AddEngineAsync(Engine engine);
    Task UpdateEngineAsync(Engine engine);
    Task DeleteEngineAsync(Guid id);
}