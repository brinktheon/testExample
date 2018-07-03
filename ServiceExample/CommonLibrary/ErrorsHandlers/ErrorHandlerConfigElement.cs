using ServiceExample;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ErrorsHandlers
{
    public class ErrorHandlerConfigElement : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(ErrorHandlerBehavior);

        protected override object CreateBehavior()
        {
            return new ErrorHandlerBehavior(Type.GetType(this["handlerType"].ToString()));
        }

        [ConfigurationProperty("handlerType", IsKey = true, IsRequired = true)]
        public string HandlerType
        {
            get
            {
                return (string)this["handlerType"];
            }

            set
            {
                this["handlerType"] = value;
            }
        }
    }
}
