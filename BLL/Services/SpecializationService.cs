using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services;

public class SpecializationService : ISpecializationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SpecializationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<SpecializationDto>> GetSpecializationTypes()
    {
        var result = await _unitOfWork.Repository<Specialization>().ListAllAsync();
        
        return _mapper.Map<IReadOnlyList<SpecializationDto>>(result);
    }
}