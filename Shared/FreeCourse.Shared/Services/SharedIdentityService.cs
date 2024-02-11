using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Services {
    public class SharedIdentityService : ISharedIdentityService {
        private IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        //Token içerisinde yer alan sub / user id değerini alacak olan fonk, her seferinde yazmak yerine shared üzerinde tanımlayıp implemente ettiğimiz her yerde kullanabileceğiz 
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
