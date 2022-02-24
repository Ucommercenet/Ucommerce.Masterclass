namespace Ucommerce.Masterclass.Umbraco.Models
{
public class AddressViewModel
{
    public AddressViewModel()
    {
    }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public string PhoneNumber { get; set; }

    public string MobilePhoneNumber { get; set; }

    public string Line1 { get; set; }

    public string Line2 { get; set; }

    public string PostalCode { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Attention { get; set; }

    public string CompanyName { get; set; }

    public CountryViewModel Country { get; set; }
    public string ShippingMethodId { get; set; }
}
}