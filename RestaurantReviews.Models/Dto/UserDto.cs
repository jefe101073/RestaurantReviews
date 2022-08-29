namespace RestaurantReviews.Models.Dto
{
    /// <summary>
    /// The smaller AddUserDto does not include Id, or database nullable fields, simplifies the Add method
    /// </summary>
    public class AddUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsUserBlocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class UserDto : AddUserDto
    {
        public int Id { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
