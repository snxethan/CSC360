using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTracker.States
{
    public interface IPlayerXPState
    {
        void ModifyXP(Player player, int amount);
        string Name { get; }
    }

}
