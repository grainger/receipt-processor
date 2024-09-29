using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;
using ReceiptProcessor.Models;
using ReceiptProcessor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var receipts = new Dictionary<string, Receipt>();

app.MapGet("receipts/{id}/points", Results<Ok<int>, NotFound> (string id) =>
{
    receipts.TryGetValue(id, out var receipt);
    if (receipt == null)
    {
        return TypedResults.NotFound();
    }

    var pointsService = new PointsService();
    
    return TypedResults.Ok(pointsService.CalculatePoints(receipt));
});

app.MapPost("receipts/process", (Receipt receipt) =>
{
    //Since using minimal API pattern, need this to correctly validate DataAnnotations.
    // Create a new ValidationContext
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(receipt, null, null);

    // Validate the receipt object
    bool isValid = Validator.TryValidateObject(receipt, validationContext, validationResults, true);

    // If validation fails, return a 400 BadRequest with validation errors
    if (!isValid)
    {
        var errors = validationResults.ToDictionary(vr => vr.MemberNames.FirstOrDefault() ?? "General", vr => vr.ErrorMessage);
        return Results.BadRequest(errors);
    }
    
    var receiptId = Guid.NewGuid().ToString();
    receipts.Add(receiptId, receipt);
    return TypedResults.Ok(receiptId);
});

app.Run();