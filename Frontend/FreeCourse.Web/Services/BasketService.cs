using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Services.Interface;

namespace FreeCourse.Web.Services {
    public class BasketService : IBasketService {
        public Task AddBasketItem(BasketItemViewModel basketItemViewModel) {
            throw new NotImplementedException();
        }

        public Task<bool> ApplyDiscount(string discountCode) {
            throw new NotImplementedException();
        }

        public Task<bool> CancelApplyDiscount() {
            throw new NotImplementedException();
        }

        public Task<bool> Delete() {
            throw new NotImplementedException();
        }

        public Task<BasketViewModel> Get() {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveBasketItem(string courseId) {
            throw new NotImplementedException();
        }

        public Task<bool> SaveOrUpdate(BasketViewModel basketViewModel) {
            throw new NotImplementedException();
        }
    }
}
