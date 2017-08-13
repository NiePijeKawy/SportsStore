using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineColection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineColection
                .Where(p => p.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (line==null)
            {
                lineColection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            lineColection.RemoveAll(p => p.Product.ProductId == product.ProductId);
        }

        public decimal ComputeTotalValue() 
        {
            return lineColection.Sum(e=>e.Product.Price * e.Quantity);
        }

        public void Clear()
        {
            lineColection.Clear();
        }

        public IEnumerable<CartLine> Lines { get { return lineColection; } }
    }
}
