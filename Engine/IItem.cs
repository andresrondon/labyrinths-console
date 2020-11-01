using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinths.Engine
{
    public interface IItem
    {
        string Name { get; set; }
        int Value { get; set; }
        void Use(Entity entitiy);
    }
}
