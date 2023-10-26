public class ErrorData
{
    public bool DidValidationfail { get; set; }
    public string? ErrorMessage { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? ValidationType { get; set; }
    public string? FilePath { get; set; }
    public List<string>? rowDataFromCSV { get; set; }
    public DateTime? currentTime {get; set;}


    public ErrorData()
    {
        
        DidValidationfail = false;
    }
}
