using System.ComponentModel.DataAnnotations;

namespace SmeKpiDashboard.DTOs;

public class SaleRequest
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}
