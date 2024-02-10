﻿using FreeCourse.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.PhotoStock.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController {
        //CancellationToken -> endpointi çağıran yer işlemi sonlandırırsa yükleme işlemini bitirmeyi sağlar
        //hata fırlatarak işlemi sonlandırır 
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken) {
            if (photo != null && photo.Length > 0) {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                var returnPath = "photos/" + photo.FileName;

                PhotoDto photoDto = new() { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
        }
    }
}
