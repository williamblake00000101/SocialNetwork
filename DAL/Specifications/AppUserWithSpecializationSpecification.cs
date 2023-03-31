using DAL.Entities;

namespace DAL.Specifications;

public class AppUserWithSpecializationSpecification: BaseSpecification<AppUser>
{
    public AppUserWithSpecializationSpecification(UserParams userParams) : base(a =>
        (string.IsNullOrEmpty(userParams.Search) || a.LastName.ToLower().Contains(userParams.Search)) &&
        (!userParams.SpecializationId.HasValue || a.SpecializationId == userParams.SpecializationId))
    {
        AddInclude(a => a.Specialization);
        AddOrderBy(a => a.LastName);
        ApplyPaging(userParams.PageSize * (userParams.PageNumber - 1), userParams.PageSize);

        if (!string.IsNullOrEmpty(userParams.Sort))
        {
            switch (userParams.Sort)
            {
                case "yearDateOfBirthAsc":
                    AddOrderBy(a => a.DateOfBirth);
                    break;
                case "yearDateOfBirthDesc":
                    AddOrderByDescending(a => a.DateOfBirth);
                    break;
                default:
                    AddOrderBy(a => a.LastName);
                    break;
            }
        }
    }

    public AppUserWithSpecializationSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(a => a.Specialization);
    }
}