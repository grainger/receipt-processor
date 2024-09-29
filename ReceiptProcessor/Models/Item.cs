using System.ComponentModel.DataAnnotations;

namespace ReceiptProcessor.Models;

public class Item
{
    [Required]
    [RegularExpression(@"^[\w\s\-]+$", ErrorMessage = "Invalid short description.")]
    public string ShortDescription { get; set; }

    [Required]
    [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "Invalid price.")]
    public string Price { get; set; }
}