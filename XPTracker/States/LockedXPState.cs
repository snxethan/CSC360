using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTracker.States
{
    internal class LockedXPState : IPlayerXPState
    {
        public string Name => "Locked";

        public void ModifyXP(Player player, int amount)
        {
            // Do nothing
        }
    }
}
