using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimateCanvas.Pages
{
    public partial class ImagesAsElementExamples
    {
        private List<string> actionLog = new();
        private void HandleAction(string action)
        {
            actionLog.Add(action);
            if (actionLog.Count > 30)
                actionLog.RemoveAt(0);
            StateHasChanged();
        }
    }
}
