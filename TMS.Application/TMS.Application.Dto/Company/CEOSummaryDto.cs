namespace TMS.Application.Dto.Company;

public class CEOSummaryDto
{
    public CEOSummaryBoardDto[] Boards { get; set; }
    public int TotalTasks { get; set; }
    public int TotalDone { get; set; }
    public int TotalInProgress { get; set; }
    public string LeadBoard { get; set; }
    public string MostActiveUser { get; set; }
}