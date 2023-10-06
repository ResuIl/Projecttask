namespace Projecttask.Models;

public class Orders
{
	public int Id { get; set; }
	public string EmployerId { get; set; }
	public string WorkerId { get; set; }
	public ApplicationUser Employer { get; set; }
	public ApplicationUser Worker { get; set; }
	public string Message { get; set; }
	public decimal OfferPrice { get; set; }
	public bool isJobAccepted { get; set; } = false;
	public bool isJobFinished { get; set; } = false;
	public bool isJobRated { get; set; } = false;
	public int rating { get; set; } = 0;
}