using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Generator
{
    interface ILevelGenerator
    {
        Models.Level CreateLevel(int seed);
    }
}
