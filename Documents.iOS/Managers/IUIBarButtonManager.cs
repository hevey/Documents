using System.Collections.Generic;
using Documents.iOS.Buttons;
namespace Documents.iOS.Managers
{
    public interface IUIBarButtonManager
    {
        List<IUIBarButtonItem> GetButtons();
    }
}