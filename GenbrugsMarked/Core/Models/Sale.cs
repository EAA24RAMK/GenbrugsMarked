using System;

namespace Core.Models;

public class Sale
{
    public int SaleId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public int RoomId { get; set; }
}