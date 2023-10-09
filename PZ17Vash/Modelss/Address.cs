using System;

namespace PZ17Vash.Modelss;

public class Address
{
    private int ID;
    private string NameCity;
    private string StreetID;

    public Address(int ID, string nameCity, string streetId)
    {
        this.ID = ID;
        NameCity = nameCity;
        StreetID = streetId;
    }

    public int Id => ID;

    public string NameCity1 => NameCity;

    public string StreetId => StreetID;
}