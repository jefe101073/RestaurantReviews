using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public bool IsUserBlocked { get; set; } = false;
        [Required]
        public bool IsDeleted { get; set; } = false;
        public int DeletedByUserId { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
