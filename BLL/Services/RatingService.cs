using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services;

public class RatingService : IRatingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task Vote(RatingDto ratingDto, int appUserId)
    {
        var rating = _mapper.Map<Rating>(ratingDto);
        await _unitOfWork.RatingRepository.Vote(rating, appUserId);
    }
}