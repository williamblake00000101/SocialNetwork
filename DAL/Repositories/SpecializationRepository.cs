using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyList<Specialization>> GetSpecializationTypes()
    {
        return await _context.Specializations.ToListAsync();
    }
}