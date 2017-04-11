using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sitecore.Support.Form.Core
{
  [DataContract]
  public class AddToListEvent
  {
    [DataMember]
    public Guid ContactId { get; set; }

    [DataMember]
    public List<string> Lists { get; set; }
  }
}
