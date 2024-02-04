using AutoMapper;
using FreeCourse.Services.Catolog.Dtos;
using FreeCourse.Services.Catolog.Models;
using FreeCourse.Services.Catolog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catolog.Services {
    public class CategoryService : ICategoryService {

        //Bağlantı kurulacak connection
        private readonly IMongoCollection<Category> _categoryCollection;

        //dto nesnesini dönüştürme işlemi için
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings) {
            var client = new MongoClient(databaseSettings.ConnectionString); //veritabanına bağlanma
            var database = client.GetDatabase(databaseSettings.DatabaseName); //bağlanılacak veritabanını belirleme

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName); //
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync() {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto) {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id) {
            var category = await _categoryCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null) {
                return Response<CategoryDto>.Fail("Category not found", 404);
            }

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }
}
