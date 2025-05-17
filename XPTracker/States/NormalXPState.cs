using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTracker.States
{
    internal class NormalXPState : IPlayerXPState
    {
        public string Name => "Normal XP";

        public void ModifyXP(Player player, int amount)
        {
            player.XP += amount;
        }
    }
}
