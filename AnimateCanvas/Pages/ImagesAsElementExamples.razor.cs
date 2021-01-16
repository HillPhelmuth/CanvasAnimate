using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AnimateCanvas.Pages
{
    public partial class ImagesAsElementExamples
    {
        
        private List<string> actionLog = new();
        private string inputText;
        private bool inputChecked;
        private string cursorCss = "";
        private List<string> radioButtons = new() {"Knight", "Mage", "Ranger"};
        
        private Dictionary<string, string> smallSquare = new() {{"blue", "blue"}, {"beige", "beige"}, {"brown", "brown"}, {"grey", "grey"}};
        private Dictionary<string, string> largeColor = new() { { "blue", "blue" }, { "beige", "beige" }, { "brown", "brown" }, { "grey", "grey" } };
        private Dictionary<string, string> smallRound = new() { { "blue", "blue" }, { "beige", "beige" }, { "brown", "brown" }, { "grey", "grey" } };
        private Dictionary<int, string> smallRoundIndex = new()
            {{1, "beige"}, {2, "blue"}, {3, "green"}, {4, "grey"}, {5, "pink"}, {6, "yellow"}, {7, "brown"}, { 8, "red" }, { 9, "purple" }, { 10, "black" } };
        private string selectedRadio = "";
        


        private void HandleAction(string action)
        {
            actionLog.Add(action);
            if (actionLog.Count > 30)
                actionLog.RemoveAt(0);
            StateHasChanged();
        }

        private void ChangeColor(string color)
        {
            var random = new Random();
            smallRound[color] = smallRoundIndex[random.Next(1, 11)];
        }
        private void HandleMouseDownLg(string color) => largeColor[color] = $"{color}_pressed";
        private void HandleMouseUpLg(string color) => largeColor[color] = color;
        private void HandleMouseDownSm(string color) => smallSquare[color] = $"{color}_pressed";
        private void HandleMouseUpSm(string color) => smallSquare[color] = color;
        private void ChangeCursor(string cssClass) => cursorCss = cssClass;
        private void RadioButtonSelect(ChangeEventArgs args)
        {
            selectedRadio = args.Value?.ToString() ?? "well, shit. Radio buttons fucked up";
            HandleAction($"Radio button {selectedRadio} selected");
        }
    }

}
