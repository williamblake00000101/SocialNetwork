using BLL.DTOs;

namespace BLL.Interfaces;

public interface ISpecializationService
{
    Task<IReadOnlyList<SpecializationDto>> GetSpecializationTypes();
}