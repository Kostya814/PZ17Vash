using System;

namespace PZ17Vash.Modelss;

public class Streets
{
    public int ID { get; }
    private string NameStreet;

    public Streets(int id, string nameStreet)
    {
        ID = id;
        NameStreet = nameStreet;
    }
   
    public int Id
    {
        get => ID;
        
    }
    
    public string NameStreet1
    {
        get => NameStreet;
        set => NameStreet = value;
    }

   
}