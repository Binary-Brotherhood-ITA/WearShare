using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class ClothingItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        public string Owner { get; set; } = String.Empty; // String.Empty: En tom string "" som default

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = String.Empty;

        [Required(ErrorMessage = "Size is required")]
        public string Size { get; set; } = String.Empty;

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; } = String.Empty;

        public bool IsAvailable { get; set; }
    }
}