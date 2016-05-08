using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartEnergyMeter.Models
{
    public class RegisterCustomerViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "AddressLine1")]
        public string AddressLine1 { get; set; }

        [Required]
        [Display(Name = "AddressLine2")]
        public string AddressLine2 { get; set; }

        [Required]
        [Display(Name = "Pincode")]
        public int PinCode { get; set; }
    }

    public class LoginViewModel
    {
        public int UserType { get; set; }
        

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }

    public class ConsumptionLogViewModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string SmartEnergyMeterId { get; set; }

        [Required]
        [Display(Name = "Unit")]
        public decimal Unit { get; set; }

        [Required]
        [Display(Name = "Logged Date Time")]
        public DateTime CreatedDateTime { get; set; }
    }
}