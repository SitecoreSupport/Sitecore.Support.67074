using Sitecore.Analytics.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.ListManagement;
using Sitecore.ListManagement.ContentSearch.Model;

namespace Sitecore.Support.Form.Core
{
  public class WffmListActionHandler
  {
    public static void Run(AddToListEvent remoteEvent)
    {
      ListManager<ContactList, ContactData> manager = Factory.CreateObject("contactListManager", true) as ListManager<ContactList, ContactData>;
      ContactRepository repository = Factory.CreateObject("contactRepository", true) as ContactRepository;
      Assert.IsNotNull(repository, "ContactRepository cannot be null");
      Assert.IsNotNull(manager, "ListManager cannot be null");
      foreach (string str in remoteEvent.Lists)
      {
        ContactList list = manager.FindById(str);
        Assert.IsNotNull(list, $"Could not get contact list {str}");
        Contact contact = repository.LoadContactReadOnly(remoteEvent.ContactId);
        Assert.IsNotNull(contact, $"Could not load contact {remoteEvent.ContactId}");
        manager.SubscribeContact(list, contact);
      }
    }
  }
}
