using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Deadfile.Model.Interfaces
{
    public interface IModelEntityMapper
    {
        IMapper Mapper { get; }
    }
}
