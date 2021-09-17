using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net
{
    public interface ISerializable
    {
        abstract int Serialize(MemoryStream stream);
        abstract int DeSerialize(MemoryStream stream);
    }


}
