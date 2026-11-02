namespace Core
{
    public class ClothingItem
    {
        public string Id { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string OwnerId { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
    }
}