using ReceiptProcessor.Models;

namespace ReceiptProcessor.Services;

public class PointsService
{
    public int CalculatePoints(Receipt receipt)
    {
        var points = 0;
        // One point for every alphanumeric character in the retailer name.
        points += receipt.Retailer.Count(char.IsLetterOrDigit);

        if (!decimal.TryParse(receipt.Total, out var total))
        {
            throw new Exception("Invalid total amount.");
        }

        // 50 points if the total is a round dollar amount with no cents.
        if (decimal.IsInteger(total))
        {
            points += 50;
        }

        // 25 points if the total is a multiple of 0.25.
        if (total % 0.25m == 0)
        {
            points += 25;
        }

        // 5 points for every two items on the receipt.
        points += (receipt.Items.Count / 2) * 5;

        // If the trimmed length of the item description is a multiple of 3, multiply the price by 0.2 and round up to the nearest integer. The result is the number of points earned.
        foreach (var item in receipt.Items)
        {
            if (!decimal.TryParse(item.Price, out var itemPrice))
            {
                throw new Exception("Invalid item price.");
            }
    
            var trimmedLength = item.ShortDescription.Trim().Length;
            if (trimmedLength % 3 == 0)
            {
                points += Decimal.ToInt32(Math.Ceiling(itemPrice * 0.2m));
            }
        }

        // 6 points if the day in the purchase date is odd.
        if (!DateTime.TryParse(receipt.PurchaseDate, out var date))
        {
            throw new Exception("Invalid date.");
        }

        if (int.IsOddInteger(date.Day))
        {
            points += 6;
        }

        // 10 points if the time of purchase is after 2:00pm and before 4:00pm.
        if (!TimeSpan.TryParse(receipt.PurchaseTime, out var purchaseTime))
        {
            throw new Exception("Invalid date.");
        }

        // Define the start and end times
        var startTime = new TimeSpan(14, 0, 0);  // 2:00 PM
        var endTime = new TimeSpan(16, 0, 0);    // 4:00 PM
        if (purchaseTime >= startTime && purchaseTime < endTime)
        {
            points += 10;
        }

        return points;
    }
}