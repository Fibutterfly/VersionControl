using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factory_example.Entites;

namespace Factory_example.Abstraction
{
    public interface IToyFactory

    { 
        Toy CreateNew();
    }
}
