//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoCoWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Application
    {
        public int Id { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }        
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required")]
        public string FName { get; set; }
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LName { get; set; }
        public string Comment { get; set; }
        [Display(Name = "CV")]
        public string CVPath { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Job")]
        public int JobId { get; set; }
        [Display(Name = "Applicant state")]
        public int ApplicantStateId { get; set; }
        [Display(Name = "Application Date")]
        public System.DateTime ApplicationDate { get; set; }
        [Display(Name = "Observer")]
        public Nullable<int> ObserverId { get; set; }
        [Display(Name = "Observer's comment")]
        public string ObserverComment { get; set; }
        [Display(Name = "Updated At")]
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        [Display(Name = "Updated By")]
        public Nullable<int> UpdatedBy { get; set; }
    
        public virtual ApplicantState ApplicantState { get; set; }
        public virtual Job Job { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
