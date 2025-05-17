using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTracker.States
{
    internal class DoubleXPState : IPlayerXPState
    {
        public string Name => "Double XP";

        public void ModifyXP(Player player, int amount)
        {
            player.XP += amount * 2;
        }
    }
}
