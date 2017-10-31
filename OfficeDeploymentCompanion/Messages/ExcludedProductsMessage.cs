using System.Collections.Generic;
using System.Linq;

namespace OfficeDeploymentCompanion.Messages
{
    public class ExcludedProductsMessage
    {
        public ExcludedProductsMessage(IEnumerable<string> productsIds)
        {
            this.ProductsIds = productsIds?.ToArray() ?? new string[0];
        }

        public string[] ProductsIds { get; }
    }
}
