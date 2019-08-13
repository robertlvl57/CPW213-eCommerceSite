using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    /// <summary>
    /// Represents an individual website user
    /// </summary>
    public class Member
    {
        public int MemberId { get; set; }

        /// <summary>
        /// The first and last name of the Member.
        /// es. J. Doe
        /// </summary>
        [StringLength(60)]
        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "That doesn't look like an email")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[\d\w]+$",
            ErrorMessage = "Usernames can only contain A-Z, 0-9, and underscores")]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// The date of birth for the member. Time is ignored.
        /// </summary>
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        // Required because DateTime is a structure (it's a value type)
        public DateTime DateOfBirth { get; set; }
    }

    /// <summary>
    /// ViewModel for the login page
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName or Email")]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
