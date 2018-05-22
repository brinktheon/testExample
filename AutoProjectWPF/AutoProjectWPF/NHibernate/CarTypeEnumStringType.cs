using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using NHibernate.Type;

namespace AutoProjectWPF.NHibernate
{
    class CarTypeEnumStringType : EnumStringType
    {
        public CarTypeEnumStringType() : base(typeof(CarType), 30)
        {
        }
    }
}
