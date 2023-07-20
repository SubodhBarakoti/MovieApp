using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum RatingType
    {
        [Display(Name = "One Star")]
        onestar,
        [Display(Name = "Two Star")]
        twostar,
        [Display(Name = "Three Star")]
        threestar,
        [Display(Name = "Four Star")]
        fourstar,
        [Display(Name = "Five Star")]
        fivestar
    }
}
