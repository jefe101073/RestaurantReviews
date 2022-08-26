using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsUserBlocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int DeletedByUserId { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
