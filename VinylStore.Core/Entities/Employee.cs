using System;
using System.Collections.Generic;

namespace VinylStore.Core.Entities;

public partial class Employee : BaseEntity
{
    // public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Inventorymovement> Inventorymovements { get; set; } = new List<Inventorymovement>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
