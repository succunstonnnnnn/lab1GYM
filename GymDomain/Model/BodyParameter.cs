using System.ComponentModel.DataAnnotations;

namespace GymDomain.Model;
public class BodyParameter : Entity
{
    public int UserId { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public float Weight { get; set; }
    
    public double StartWeight { get; set; } 
    public double PersonWeight { get; set; }  
    
    public float? TargetWeight { get; set; }
    public float? Waist { get; set; }
    public float? Chest { get; set; }
    public float? Thigh { get; set; }
    public float? Biceps { get; set; }
    public float? Calf { get; set; }
    public float? Glutes { get; set; }

    public virtual User User { get; set; } = null!;
}