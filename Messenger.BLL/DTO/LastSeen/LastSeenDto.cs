namespace Mesagger.BLL.DTO.LastSeen;

public class LastSeenDto
{
    public int ProfileId { get; set; }
    public bool IsOnline { get; set; }
    public DateTime? LastSeen { get; set; }
}