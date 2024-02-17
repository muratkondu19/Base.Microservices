using FreeCourse.Web.Models.PhotoStocks;
using FreeCourse.Web.Services.Interface;

namespace FreeCourse.Web.Services {
    public class PhotoStockService : IPhotoStockService {
        public Task<bool> DeletePhoto(string photoUrl) {
            throw new NotImplementedException();
        }

        public Task<PhotoViewModel> UploadPhoto(IFormFile photo) {
            throw new NotImplementedException();
        }
    }
}
