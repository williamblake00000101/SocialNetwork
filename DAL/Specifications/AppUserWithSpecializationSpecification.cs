using DAL.Entities;

namespace DAL.Specifications;

public class AppUserWithSpecializationSpecification: BaseSpecification<AppUser>
{
    public AppUserWithSpecializationSpecification(UserParams userParams) : base(a =>
        (string.IsNullOrEmpty(userParams.Search) || a.LastName.ToLower().Contains(userParams.Search)) &&
        (!userParams.SpecializationId.HasValue || a.SpecializationId == userParams.SpecializationId))
    {
        AddInclude(a => a.Specialization);
        
        ApplyPaging(userParams.PageSize * (userParams.PageNumber - 1), userParams.PageSize);

        if (!string.IsNullOrEmpty(userParams.OrderBy))
        {
            switch (userParams.OrderBy)
            {
                case "created":
                    AddOrderByDescending(a => a.Created);
                    break;
                default:
                    AddOrderBy(a => a.LastActive);
                    break;
            }
        }
    }

    public AppUserWithSpecializationSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(a => a.Specialization);
    }
}