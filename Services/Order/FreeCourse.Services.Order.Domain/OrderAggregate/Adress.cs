using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate {

    //kimse dışarıdan state değiştirememesi için set private olarak ayarlanır 
    //business kuralları da burda tanımlanacaktır
    public class Address : ValueObject {
        public string Province { get; private set; }

        public string District { get; private set; }

        public string Street { get; private set; }

        public string ZipCode { get; private set; }

        public string Line { get; private set; }


        //set etme private olduğundan oluşturma işlemi için bir ctor tanımlanır 
        public Address(string province, string district, string street, string zipCode, string line) {
            Province = province;
            District = district;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Province;
            yield return District;
            yield return Street;
            yield return ZipCode;
            yield return Line;
        }
    }
}
