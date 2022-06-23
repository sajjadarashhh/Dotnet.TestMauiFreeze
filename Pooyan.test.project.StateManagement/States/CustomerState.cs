using System;
using System.ComponentModel.DataAnnotations;

namespace Pooyan.test.project.StateManagement.States
{
    public class CustomerState
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "فیلد نام اجباری است")]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string NationalCode { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Telephone { get; set; }
        public bool IsMarketer { get; set; }
    }
}
