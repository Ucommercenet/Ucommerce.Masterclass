using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class BillingController : RenderMvcController
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var addressDetails = new AddressDetailsViewModel();
            
            var shippingInformation = TransactionLibrary.GetShippingInformation();
            var billingInformation = TransactionLibrary.GetBillingInformation();

            addressDetails.BillingAddress.FirstName = billingInformation.FirstName;
            addressDetails.BillingAddress.LastName = billingInformation.LastName;
            addressDetails.BillingAddress.EmailAddress = billingInformation.EmailAddress;
            addressDetails.BillingAddress.PhoneNumber = billingInformation.PhoneNumber;
            addressDetails.BillingAddress.MobilePhoneNumber = billingInformation.MobilePhoneNumber;
            addressDetails.BillingAddress.Line1 = billingInformation.Line1;
            addressDetails.BillingAddress.Line2 = billingInformation.Line2;
            addressDetails.BillingAddress.PostalCode = billingInformation.PostalCode;
            addressDetails.BillingAddress.City = billingInformation.City;
            addressDetails.BillingAddress.State = billingInformation.State;
            addressDetails.BillingAddress.Attention = billingInformation.Attention;
            addressDetails.BillingAddress.CompanyName = billingInformation.CompanyName;
            addressDetails.BillingAddress.CountryId = billingInformation.Country != null ? billingInformation.Country.CountryId : -1;

            addressDetails.ShippingAddress.FirstName = shippingInformation.FirstName;
            addressDetails.ShippingAddress.LastName = shippingInformation.LastName;
            addressDetails.ShippingAddress.EmailAddress = shippingInformation.EmailAddress;
            addressDetails.ShippingAddress.PhoneNumber = shippingInformation.PhoneNumber;
            addressDetails.ShippingAddress.MobilePhoneNumber = shippingInformation.MobilePhoneNumber;
            addressDetails.ShippingAddress.Line1 = shippingInformation.Line1;
            addressDetails.ShippingAddress.Line2 = shippingInformation.Line2;
            addressDetails.ShippingAddress.PostalCode = shippingInformation.PostalCode;
            addressDetails.ShippingAddress.City = shippingInformation.City;
            addressDetails.ShippingAddress.State = shippingInformation.State;
            addressDetails.ShippingAddress.Attention = shippingInformation.Attention;
            addressDetails.ShippingAddress.CompanyName = shippingInformation.CompanyName;
            addressDetails.ShippingAddress.CountryId = shippingInformation.Country != null ? shippingInformation.Country.CountryId : -1;

            addressDetails.AvailableCountries = Country.All().ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.CountryId.ToString() }).ToList();

            addressDetails.UseAlternativeAddress = 
                !string.IsNullOrWhiteSpace(addressDetails.BillingAddress.Line1 ) && 
                (addressDetails.BillingAddress.Line1 != addressDetails.ShippingAddress.Line1);
            
            return View(addressDetails);
        }

        [HttpPost]
        public ActionResult Index(AddressDetailsViewModel addressDetails)
        {
            TransactionLibrary.EditBillingInformation(
                addressDetails.BillingAddress.FirstName,
                addressDetails.BillingAddress.LastName,
                addressDetails.BillingAddress.EmailAddress,
                addressDetails.BillingAddress.PhoneNumber,
                addressDetails.BillingAddress.MobilePhoneNumber,
                addressDetails.BillingAddress.CompanyName,
                addressDetails.BillingAddress.Line1,
                addressDetails.BillingAddress.Line2,
                addressDetails.BillingAddress.PostalCode,
                addressDetails.BillingAddress.City,
                addressDetails.BillingAddress.State,
                addressDetails.BillingAddress.Attention,
                addressDetails.BillingAddress.CountryId);


            var shippingAddress = addressDetails.BillingAddress;

            if (addressDetails.UseAlternativeAddress)
                shippingAddress = addressDetails.ShippingAddress;
            
            TransactionLibrary.EditShippingInformation(
                shippingAddress.LastName,
                shippingAddress.EmailAddress,
                shippingAddress.PhoneNumber,
                shippingAddress.FirstName,
                shippingAddress.MobilePhoneNumber,
                shippingAddress.CompanyName,
                shippingAddress.Line1,
                shippingAddress.Line2,
                shippingAddress.PostalCode,
                shippingAddress.City,
                shippingAddress.State,
                shippingAddress.Attention,
                shippingAddress.CountryId);

            TransactionLibrary.ExecuteBasketPipeline();

            return Redirect("/shipping");
        }
    }
}