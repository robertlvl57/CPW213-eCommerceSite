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
        [DateOfBirth]
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

    public class DateOfBirthAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Member m = validationContext.ObjectInstance as Member;

            // Get the value of DateOfBirth for the model
            DateTime dob = Convert.ToDateTime(value);

            DateTime oldestAge = DateTime.Today.AddYears(-120);

            if(dob > DateTime.Today || dob < oldestAge)
            {
                string errMsg = "You cannot be born in the future or more than 120 in the past";
                return new ValidationResult(errMsg);
            }

            return ValidationResult.Success;
        }
    }
}
