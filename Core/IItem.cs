using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinths.Core
{
    public interface IItem
    {
        string Name { get; set; }
        int Value { get; set; }
        void Use(Entity entitiy);
    }
}
