using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Ken
{
    [LuisModel("829ee634-ba62-4240-baf7-70a66163ab01", "d018c3a25fcc49889b863595ceef2ffd")]
    [Serializable]
    public class KenAi : LuisDialog<object>
    {
        public const string EntityStockSymbol = "StockSymbol";

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("StockPrice")]
        public async Task GetStockPrice(IDialogContext context, LuisResult result)
        {
            EntityRecommendation symbol;
            if (result.TryFindEntity(EntityStockSymbol, out symbol))
            {
                await context.PostAsync($"found symbol {symbol.Entity}");
            }
            else
            {
                await context.PostAsync("did not find symbol");
            }
            context.Wait(MessageReceived);
        }
    }
}