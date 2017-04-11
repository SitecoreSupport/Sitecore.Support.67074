using Sitecore.Eventing;
using Sitecore.Events.Hooks;
using System;

namespace Sitecore.Support.Form.Core
{
  public class WffmListActionHook : IHook
  {
    public void Initialize()
    {
      EventManager.Subscribe<AddToListEvent>(new Action<AddToListEvent>(WffmListActionHandler.Run));
    }
  }
}
