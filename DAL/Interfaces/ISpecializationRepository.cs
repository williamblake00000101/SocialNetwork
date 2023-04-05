using DAL.Entities;

namespace DAL.Interfaces;

public interface ISpecializationRepository
{
    void AddSpecialization(Specialization specialization);
    Task<IReadOnlyList<Specialization>> GetSpecializationTypes();
}