namespace ProjectTracker.Infrastructure.Models;

public interface IConcurrencyModel
{
	public uint Version { get; set; }
}