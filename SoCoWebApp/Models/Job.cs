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

    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            this.Application = new HashSet<Application>();
        }
    
        public int Id { get; set; }
        [Display(Name = "Responsibilities")]
        [Required(ErrorMessage = "Responsibilities are required")]
        public string Responsibility { get; set; }
        [Display(Name = "Requirments")]
        [Required(ErrorMessage = "Requirments are required")]
        public string Requirment { get; set; }
        [Display(Name = "Skills")]
        [Required(ErrorMessage = "Skills are required")]
        public string Skill { get; set; }
        [Display(Name = "We offer")]
        [Required(ErrorMessage = "We offer is required")]
        public string Offer { get; set; }
        [Display(Name = "Additional comment")]
        public string AdditComment { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Position")]
        [Required(ErrorMessage = "Position is required")]
        public int PositionId { get; set; }
        [Display(Name = "Office")]
        [Required(ErrorMessage = "Office is required")]
        public int OfficeId { get; set; }
        [Display(Name = "Seniority Level")]
        [Required(ErrorMessage = "Seniority Level is required")]
        public int SeniorityLevelId { get; set; }
        [Display(Name = "Created At")]
        public System.DateTime CreatedAt { get; set; }
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        [Display(Name = "Updated At")]
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        [Display(Name = "Updated By")]
        public Nullable<int> UpdatedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Application { get; set; }
        public virtual Office Office { get; set; }
        public virtual Position Position { get; set; }
        public virtual SeniorityLevel SeniorityLevel { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}