using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeCourse.Shared.Dtos {
    public class Response<T> {
        public T Data { get; set; }

        //Response bir status code değerine sahiptir bunu json ile response nesnesinde dönmeye gerek yoktur fakat yazılımda bu kullanılacaktır
        [JsonIgnore]
        public int StatusCode { get; set; }

        //Yazılım tarafında kullanılacaktır response ile görünümüne gerek yoktur
        [JsonIgnore]
        public bool IsSuccessful { get; set; }

        public List<string> Errors { get; set; }

        // Static Factory Method -> class içerisinde tanımlanarak nesne oluşturmaya yarar
        public static Response<T> Success(T data, int statusCode) {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        //Başarılı olabilir fakat data almayabilir, update delete işlemi örnek
        public static Response<T> Success(int statusCode) {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        //Birden fazla hata dönme durumu
        public static Response<T> Fail(List<string> errors, int statusCode) {
            return new Response<T> {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        //Tekil hata olma durumu
        public static Response<T> Fail(string error, int statusCode) {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
