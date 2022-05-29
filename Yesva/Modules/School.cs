using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Yesva.Modules
{
    public class School : ModuleBase<SocketCommandContext>
    {
        [Command("suplStart")]
        public async Task StartChecking()
        {
            await Task.Run(async () =>
            {
                //do stuff

                await ReplyAsync("eyo");

                await Task.Delay(TimeSpan.FromSeconds(15));
            });
        }
    }
}
