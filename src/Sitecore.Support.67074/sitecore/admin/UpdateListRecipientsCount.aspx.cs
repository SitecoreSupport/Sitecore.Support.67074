using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.ListManagement;
using Sitecore.ListManagement.Configuration;
using Sitecore.ListManagement.ContentSearch.Model;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Support.sitecore.admin
{
    public partial class UpdateListRecipientsCount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ListManager<ContactList, ContactData> listManager = Factory.CreateObject("contactListManager", false) as ListManager<ContactList, Sitecore.ListManagement.ContentSearch.Model.ContactData>;
            using (new SecurityDisabler())
            {
                foreach (ContactList list in listManager.GetAll(null, true))
                {
                    Item item = Factory.GetDatabase(ListManagementSettings.Database).GetItem(list.Id);
                    Response.Write("List: " + list.DisplayName + ", list id: (" + list.Id + ")" + ", contacts in index: " + listManager.GetContacts(list).Count<ContactData>() + ", contacts in field: " + item["Recipients"]);
                    if (item != null && listManager.GetContacts(list).Count<ContactData>() > int.Parse(item["Recipients"]))
                    {
                        item.Editing.BeginEdit();
                        item["Recipients"] = listManager.GetContacts(list).Count<ContactData>().ToString();
                        item.Editing.EndEdit();
                        Response.Write("</br>List field was updated");
                    }
                    Response.Write("</br></br>");
                }
            }
        }
    }
}