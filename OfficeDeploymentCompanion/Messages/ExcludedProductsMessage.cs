using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.Messages
{
    public class ExcludedProductsMessage
    {
        public ExcludedProductsMessage(IEnumerable<string> productsIds)
        {
            this.ProductsIds = productsIds != null 
                ? productsIds.ToArray()
                : new string[0];
        }

        public string[] ProductsIds { get; }
    }
}
