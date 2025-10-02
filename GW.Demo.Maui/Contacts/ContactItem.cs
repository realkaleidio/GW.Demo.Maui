namespace GW.Demo.Maui.Contacts;



public class ContactItem
{
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }


    public ContactItem(string displayName, string phoneNumber)
    {
        DisplayName = displayName;
        PhoneNumber = phoneNumber;
    }
}
