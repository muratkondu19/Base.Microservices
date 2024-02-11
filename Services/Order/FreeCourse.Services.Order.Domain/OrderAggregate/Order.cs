using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate {
    //EF Core features
    // -- Owned Types
    // -- Shadow Property
    // -- Backing Field
    public class Order : Entity, IAggregateRoot {
        public DateTime CreatedDate { get; private set; }

        //ef core bu adress alanı için order tabloya alanlar da ekleyebilir ya da işaretlersek ayrı bir tablo oluşturabilir
        public Address Address { get; private set; }

        public string BuyerId { get; private set; }

        //ef core içerisinde fiel üzerinden okuma yazma gerçekleşiyorsa bu Backing Field'dır
        //private olarak order order item alabilir ama set edemez 
        private readonly List<OrderItem> _orderItems;

        //kapsülleme işlemi
        //bu aggregate order item kullanıyorsa başka bir aggregate order item kullanamaz 
        //başka bir entity adres value object kullanabilir, bir agregate root bir entity kullanıyorsa başkasının bu entityi kullanmaması gerekiyor 
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order() {
        }

        public Order(string buyerId, Address address) {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl) {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);

            if (!existProduct) {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);

                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
    }
}
