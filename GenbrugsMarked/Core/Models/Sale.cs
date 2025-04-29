using System;

namespace Core.Models;

public class Sale // embedded i Users
{
    public int SalesId { get; set; }
    public string UserId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public int RoomId { get; set; }
}