using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Shared.ControllerBases {
    public class CustomBaseController : ControllerBase {
        //ControllerBase erişimi için Project/right click/edit project file
        //  <ItemGroup>
        //  <FrameworkReference Include = "Microsoft.AspNetCore.App" />
        //</ ItemGroup >
        //file içerisine bu prop eklenerek controller base sınıfına erişim sağlanabilir.
        public IActionResult CreateActionResultInstance<T>(Response<T> response) {
            return new ObjectResult(response) {
                StatusCode = response.StatusCode
            };
        }
    }
}
