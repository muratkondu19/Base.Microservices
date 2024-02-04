using Microsoft.Extensions.Options;

namespace FreeCourse.Services.Catolog.Settings {

    //Program.cs tarafında kullanılacak 
    //herhangi bir class ctor'ında IOptions<DatabaseSettings> ile elde edebiliriz. Options pattern
    //Bu kullanım için DI olarak program.cs üzerinde tanımlanmalıdır. 
    //builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings")); -> bu şekilde datalar doldurularak kullanıma hazır hale getirilir.
    //Bu işlem sonrasında da singleton olarak tanımlaması yapılır ve ctor'da IDatabaseSettings üzerinden dolu bir db settings verisi appsettingsten okunarak erişilebilir. 

    //program.cs üzerinde tanımı
    //    builder.Services.AddSingleton<IDatabaseSettings>(sp => {
    //    //GetRequiredService -> ilgili servisi bulamazsa hata fırlatır
    //    return sp.GetRequiredService<IOptions<IDatabaseSettings>>().Value;
    //    //herhangi bir class ctor'ında IDatabaseSettings geçtipi anda dolu bir databasesetting verisi gelecektir.
    //});

    public class DatabaseSettings : IDatabaseSettings
    {
        public string CourseCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
