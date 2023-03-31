using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories;

public class SpecializationRepository : ISpecializationRepository
{
    private readonly SocialNetworkContext _context;

    public SpecializationRepository(SocialNetworkContext context)
    {
        _context = context;
    }
    
    public void AddSpecialization(Specialization specialization)
    {
        _context.Specializations.Add(specialization);
    }
}