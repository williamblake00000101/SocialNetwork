using DAL.Entities;

namespace DAL.Specifications;

public class AppUserWithFiltersForCountSpecification : BaseSpecification<AppUser>
{
    public AppUserWithFiltersForCountSpecification(UserParams userParams) : base(a =>
        (string.IsNullOrEmpty(userParams.Search) || a.LastName.ToLower().Contains(userParams.Search)) &&
        (!userParams.SpecializationId.HasValue || a.SpecializationId == userParams.SpecializationId))
    {
    }
}