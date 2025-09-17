namespace DSED.REC.Common;

public class MessageEnveloppe
{
    
    public string HpptVerb { get; set; }
    public byte[] DatasEntityEncoded { get; set; }
    public DateTime TimeCreation { get; set; } = DateTime.Now;
    public string EnityType { get; set; } = "Lead";
    public string Application {get; set;} = "LeadCRM";
}