using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace CommonLibrary.Model
{ 
    public class CarTypeModel
    {
        public virtual int Id { get; set; }
        public virtual string TypeName { get; set; }
    }
}
