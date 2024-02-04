using FreeCourse.Services.Catolog.Dtos;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Catolog.Services {
    public interface ICategoryService {
        Task<Response<List<CategoryDto>>> GetAllAsync();

        Task<Response<CategoryDto>> CreateAsync(CategoryDto category);

        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
