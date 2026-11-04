using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class ClothingItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        public int OwnerId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = String.Empty; // String.Empty: En tom string "" som default

        [Required(ErrorMessage = "Size is required")]
        public string Size { get; set; } = String.Empty;

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; } = String.Empty;

        public bool IsAvailable { get; set; }
        [Required(ErrorMessage = "A link to picture of clothing item is required")]

        public string ImgLink { get; set; } = String.Empty; // vi kan vise billede med Blazor fx: <img src="@clothing.ImgLink" alt="@clothing.Type" />

        public int? BorrowerId { get; set; }
    }
}