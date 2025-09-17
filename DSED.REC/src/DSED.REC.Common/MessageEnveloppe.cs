namespace DSED.REC.Common;

public class MessageEnveloppe
{
    
    public string HttpVerb { get; set; }
    public byte[] DatasEntityEncoded { get; set; }
    public DateTime TimeCreation { get; set; } 
    public string EnityType { get; set; }
    public string Application {get; set;}
}