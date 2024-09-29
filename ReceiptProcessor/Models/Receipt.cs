using System.ComponentModel.DataAnnotations;

namespace ReceiptProcessor.Models;

public class Receipt
{
    [Required]
    [RegularExpression(@"^[\w\s\-]+$", ErrorMessage = "Invalid retailer value.")]
    public string Retailer { get; set; }
    
    [Required]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$", ErrorMessage = "Invalid date format.")]
    public string PurchaseDate { get; set; }
    
    [Required]
    [DataType(DataType.Time, ErrorMessage = "Invalid time format.")]
    [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Invalid time format.")]
    public string PurchaseTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<Item> Items { get; set; } = [];

    [Required]
    [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "Invalid total.")]
    public string Total { get; set; }
}