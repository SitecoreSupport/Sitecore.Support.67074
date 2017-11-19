using Sitecore.Analytics.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
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
      foreach (string contactList in remoteEvent.ContactLists)
      {
        UpdateRecipientCount(contactList);
      }
    }
    private static void UpdateRecipientCount(string contactListID)
    {
      Database masterDB = null;
      try
      {
        masterDB = Database.GetDatabase("master");
        Item contactList = masterDB.GetItem(ID.Parse(contactListID));
        int recipients = int.Parse(contactList.Fields["Recipients"].Value) + 1;
        contactList.Editing.BeginEdit();
        contactList.Fields["Recipients"].Value = recipients.ToString();
        contactList.Editing.AcceptChanges();
        contactList.Editing.EndEdit();
      }
      catch
      {
        Log.Error($"[WFFM] Unable to update the {contactListID} contact list because master database missed", null);
      }
    }
  }
}
