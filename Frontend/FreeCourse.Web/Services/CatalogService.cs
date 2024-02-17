using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Catalogs;
using FreeCourse.Web.Services.Interface;

namespace FreeCourse.Web.Services {
    public class CatalogService : ICatalogService {
        private readonly HttpClient _client;


        public CatalogService(HttpClient client) {
            _client = client;
          
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput) {
            var response = await _client.PostAsJsonAsync<CourseCreateInput>("courses", courseCreateInput);

            if (!response.IsSuccessStatusCode) {
                // Log the details of the response
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"HTTP status code: {response.StatusCode}");
                Console.WriteLine($"Response content: {content}");

                // Throw an exception or handle the error accordingly
                // throw new Exception($"Failed to create course. Status code: {response.StatusCode}. Content: {content}");
            }

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> DeleteCourseAsync(string courseId) {
            var response = await _client.DeleteAsync($"courses/{courseId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync() {
            var response = await _client.GetAsync("categories");

            if (!response.IsSuccessStatusCode) {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync() {
            //http:localhost:5000/services/catalog/courses
            var response = await _client.GetAsync("courses");

            if (!response.IsSuccessStatusCode) {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
         
            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId) {
            //[controller]/GetAllByUserId/{userId}

            var response = await _client.GetAsync($"courses/GetAllByUserId/{userId}");

            if (!response.IsSuccessStatusCode) {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            return responseSuccess.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId) {
            var response = await _client.GetAsync($"courses/{courseId}");

            if (!response.IsSuccessStatusCode) {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            return responseSuccess.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput) {

            var response = await _client.PutAsJsonAsync<CourseUpdateInput>("courses", courseUpdateInput);
            if (!response.IsSuccessStatusCode) {
                // Log the details of the response
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"HTTP status code: {response.StatusCode}");
                Console.WriteLine($"Response content: {content}");

                // Throw an exception or handle the error accordingly
                // throw new Exception($"Failed to create course. Status code: {response.StatusCode}. Content: {content}");
            }


            return response.IsSuccessStatusCode;
        }
    }
}
