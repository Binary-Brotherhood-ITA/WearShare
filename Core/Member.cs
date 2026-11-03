using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Member
    {
        [Required(ErrorMessage = "Please select a member")]
        public int? Id { get; set; } // Id gøres nullable. Så kan blazor repræsentere en tom værdi (fx hvis bruger trykker delete uden at have valgt member)

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = String.Empty; // String.Empty: En tom string "" som default

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; } = String.Empty;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = String.Empty;
    }
}