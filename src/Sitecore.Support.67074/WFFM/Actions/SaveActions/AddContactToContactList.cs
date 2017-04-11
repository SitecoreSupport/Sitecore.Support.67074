using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Eventing;
using Sitecore.ListManagement;
using Sitecore.ListManagement.ContentSearch.Model;
using Sitecore.SecurityModel;
using Sitecore.Support.Form.Core;
using Sitecore.WFFM.Abstractions;
using Sitecore.WFFM.Abstractions.Actions;
using Sitecore.WFFM.Abstractions.Analytics;
using Sitecore.WFFM.Abstractions.Shared;
using Sitecore.WFFM.Actions.Base;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Support.WFFM.Actions.SaveActions
{
  [Required("IsXdbEnabled", true), Required("IsXdbTrackerEnabled", true)]
  public class AddContactToContactList : WffmSaveAction
  {
    private readonly IAnalyticsTracker analyticsTracker;
    private readonly IContactRepository contactRepository;

    public AddContactToContactList(IAnalyticsTracker analyticsTracker, IContactRepository contactRepository)
    {
      Assert.IsNotNull(analyticsTracker, "analyticsTracker");
      Assert.IsNotNull(contactRepository, "contactRepository");
      this.analyticsTracker = analyticsTracker;
      this.contactRepository = contactRepository;
    }

    public override void Execute(ID formId, AdaptedResultList adaptedFields, ActionCallContext actionCallContext = null, params object[] data)
    {
      Assert.ArgumentNotNull(adaptedFields, "fields");
      Assert.IsNotNullOrEmpty(this.ContactsLists, "Empty contact list.");
      Assert.IsNotNull(this.analyticsTracker.CurrentContact, "Contact cannot be null");
      if (adaptedFields.IsTrueStatement(this.ExecuteWhen))
      {
        List<string> list = (from x in this.ContactsLists.Split(new char[] { ',' })
                             select ID.Parse(x).ToString()).ToList<string>();
        using (new SecurityDisabler())
        {
          Contact contact = this.analyticsTracker.Current.Contact;
          if (Sitecore.Form.Core.Configuration.Settings.IsRemoteActions)
          {
            AddToListEvent event1 = new AddToListEvent
            {
              ContactId = contact.ContactId,
              Lists = list
            };
            EventManager.QueueEvent<AddToListEvent>(event1);
          }
          else
          {
            ListManager<ContactList, ContactData> manager = Factory.CreateObject("contactListManager", true) as ListManager<ContactList, ContactData>;
            foreach (string str in list)
            {
              ContactList list2 = manager.FindById(str);
              manager.SubscribeContact(list2, contact);
            }
          }
        }
      }
    }

    public string ContactsLists { get; set; }

    public string ExecuteWhen { get; set; }

  }
}
