using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interface;

namespace FreeCourse.Web.Services {
    public class OrderService : IOrderService {
        public Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput) {
            throw new NotImplementedException();
        }

        public Task<List<OrderViewModel>> GetOrder() {
            throw new NotImplementedException();
        }

        public Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput) {
            throw new NotImplementedException();
        }
    }
}
