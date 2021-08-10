using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TddDemo.Bussiness;
using TddDemo.DataAccess;
using TddDemo.Entities;

namespace TddDemo.Business.Tests
{
    // veri erişim katmanına gitmeyeceğim. Bunu ispat etmek için de veri erişim katmanında somut sınıf oluşturmayacağım. Tamamen soyut sınıftan gideceğim.

    [TestClass]
    public class CustomerManagerTests
    {
        // Mock nesnesini oluşturuyorum.
        Mock<ICustomerDal> _mockCustomerDal;   // direkt mock demememin nedeni, burada başka interfaceler de olabilir.

        // Moq ile, ICustomerDal'ın sahtesini oluşturuyoruz. 

        // gerçek veritabanına gidilmez diye kısıtlandık. Bu yüzden bana veri tabanı görevi görecek bir yapı gerek

        List<Customer> _dbCustormers;

        [TestInitialize]  // bütün testleri başlatırken çalışan bloktur.
        public void Start() // ya da setup gibi bir isim verilir.
        {
            _mockCustomerDal = new Mock<ICustomerDal>(); // ICustomerDal'ın sanki arka tarafta concrete'i varmış gibi. Reflection ile yapıyor bunu. Sanki böyle bir nesnemiz varmış gibi.

            _dbCustormers = new List<Customer>  // içerisinde 5 müşterinin olduğu bir durum çizmek istiyorum.
            {
                new Customer{Id=1,FirstName="Ali"},
                new Customer{Id=2,FirstName="Ahmet"},
                new Customer{Id=3,FirstName="Ayşe"},
                new Customer{Id=4,FirstName="Aydın"},
                new Customer{Id=5,FirstName="Burhan"},
            };

            // veritabanından getirmiş gibi yukarda oluşturduğum nesneleri getirebilmek için :
            _mockCustomerDal.Setup(m => m.GetAll()).Returns(_dbCustormers);  // mockCustomerDal'ın getall metodu dbCustomers'ı return eder diyoruz.
        }

        [TestMethod]
        public void Tum_musteriler_listelenebilmelidir()
        {
            // Arrange 

            ICustomerService customerService = new CustomerManager(_mockCustomerDal.Object); // bize o referansı veriyor.

            // Act

            List<Customer> customers = customerService.GetAll();

            // Assert

            Assert.AreEqual(5, customers.Count);
        }


        [TestMethod]
        public void A_ile_baslayan_dort_musteri_gelmelidir()
        {

            // Arrange 
            ICustomerService customerService = new CustomerManager(_mockCustomerDal.Object); // bize o referansı veriyor.

            // Act

            List<Customer> customers = customerService.GetCustomersByInitial("A");

            // Assert

            Assert.AreEqual(4, customers.Count);
        }

        [TestMethod]
        public void Kucuk_a_ile_baslayan_dort_musteri_gelmelidir()
        {

            // Arrange 
            ICustomerService customerService = new CustomerManager(_mockCustomerDal.Object); // bize o referansı veriyor.

            // Act

            List<Customer> customers = customerService.GetCustomersByInitial("a");

            // Assert

            Assert.AreEqual(4, customers.Count);
        }
    }
}

// kurallar :  -- senaryoya test case deniyor. Test Case : 5 elemanlı bir listem olsun.
// müşteriler listelenebilmelidir.
// müşteriler baş harflerine göre sayfalanabilmelidir.


// test yazarken üç tane alan vardır : Arrange , Act , Assert

// Arrange : senin testin için gerekli olan ortamı oluşturmak. yani bu müşterileri listeyebilmek için CustomerManager'a ihtiyacım var gibi.

// Act : Aksiyon alırız. metotları çağırırız gibi.

// Assert : Beklenen => bu senaryo için, customers'ın Countlarından 5 tane bekliyorum gibi.

//TDD: test driven development : test güdümlü geliştirme

//BDD : Behavior driven development : davranış güdümlü geliştirme. 

// ** dal katmanında fazla testleri yapılmaz. genelde ormlerle yapıldığı için çok yaygın değildir. Ama direkt veri erişim katmanına gidilir.