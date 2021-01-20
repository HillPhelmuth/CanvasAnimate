using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RpgComponents
{
    public partial class StyleTester
    {
        [Inject]
        private IJSRuntime jsRuntime { get; set; }
        private GuiInterop gui => new(jsRuntime);
        private List<string> guiJsObjects = new();
        private string guiJsObject;
        private string className;
        private string buttonInput;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await gui.InitGui();
            }
        }

        private async Task GetJsObjectString()
        {
           var guiJsObject = await gui.GetJavascriptObjectString();
           var splitByComma = guiJsObject.Split(',');
           guiJsObjects = splitByComma.ToList();
           await InvokeAsync(StateHasChanged);
        }

        private async Task ReLoadList()
        {
            var listId = "background-select2";
            await gui.CreateList(listId);
            await InvokeAsync(StateHasChanged);
        }
    }
}
