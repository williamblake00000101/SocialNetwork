using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class RatingDto
{
    [Range(1,5)]
    public int Rate { get; set; }
    public int PhotoId { get; set; }
}