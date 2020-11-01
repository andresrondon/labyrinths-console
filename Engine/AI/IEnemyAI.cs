using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinths.Engine.AI
{
    public interface IEnemyAI
    {
        int IQ { get; set; }
        
        event BeingAttackedEvent BeingAttacked;

        void DecideAction(Hero heroToAttack);
    }
}
