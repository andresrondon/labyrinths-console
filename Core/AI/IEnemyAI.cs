using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinths.Core.AI
{
    public interface IEnemyAI
    {
        int IQ { get; set; }
        
        event BeingAttackedEvent OnBeingAttacked;

        void DecideAction(Hero heroToAttack);
    }
}
