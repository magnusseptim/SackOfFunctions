using FluentlyCheckingFunction.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentlyCheckingFunction.Model
{
    [FluentValidation.Attributes.Validator(typeof(OrderModelValidator))]
    public class OrderModel
    {
        public Guid ID { get; set; }
        public string Warename { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
