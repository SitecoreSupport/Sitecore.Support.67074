using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sitecore.Support.Form.Core
{
  [DataContract]
  public class AddToListEvent
  {
    [DataMember]
    public List<string> ContactLists { get; set; }
  }
}
