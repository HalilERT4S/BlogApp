# Blog App

Bu proje, kullanıcıların kendi düşüncelerini, deneyimlerini ve bilgilerini paylaşabileceği, diğer kullanıcıların gönderilerini okuyup yorumlayabileceği modern ve kullanıcı dostu bir blog uygulamasıdır. MVC (Model-View-Controller) tasarım deseni ve Onion (Soğan) mimarisi kullanılarak geliştirilmiş olup, sürdürülebilirlik, test edilebilirlik ve katman ayrımı hedeflenmiştir.

## Teknolojiler

* **Backend:** C# (.NET Core)
* **Framework:** ASP.NET Core MVC
* **Veritabanı:** SQL Server
* **ORM:** Entity Framework Core
* **Validasyon:** Fluent Validation
* **Mapping:** AutoMapper
* **Kimlik Doğrulama:** JWT (JSON Web Tokens) ile yetkilendirme, Cookie Middleware ile kimlik doğrulama kontrolleri
* **Yetkilendirme:** Custom Attribute'lar ile kullanıcı yetki ve rol kontrolleri
* **Frontend:** Bootstrap 5

## Onion Mimarisi

Bu proje, katmanlı bir mimari yaklaşımı olan Onion (Soğan) mimarisi üzerine inşa edilmiştir. Amaç, iş mantığını (Domain) uygulamanın diğer katmanlarından (özellikle altyapı ve sunum) soyutlayarak daha esnek, test edilebilir ve sürdürülebilir bir yapı oluşturmaktır.

Projemizdeki temel katmanlar ve sorumlulukları şunlardır:

* **BlogApp.Domain:** Uygulamanın çekirdeğini oluşturur. İçerisinde temel iş varlıkları (Entities - örneğin, Post, Comment, User), sabit değerler (Enums) ve iş mantığı için gerekli arayüzler (Interfaces) bulunur. Bu katman hiçbir dış katmana bağımlı değildir.

* **BlogApp.Application:** Uygulamanın iş kurallarını ve kullanım senaryolarını tanımlar. Domain katmanındaki varlıkları kullanarak işlemleri gerçekleştirir. İçerisinde iş mantığına özgü servisler (Services), veri transfer nesneleri (Models), veri eşleme tanımları (Mappings - AutoMapper ile sağlanır), validasyon kuralları (Validators - Fluent Validation ile tanımlanır) ve dış katmanlara sunulacak arayüzler (Interfaces) yer alır. Domain katmanına bağımlıdır.

* **BlogApp.Infrastructure:** Uygulamanın dış dünya ile etkileşimini yönetir. Veritabanı işlemleri (Data ve Repositories), veritabanı yapılandırması (Configuration), Entity Framework Core migrasyonları (Migrations), özel middleware'ler (Middlewares) ve diğer altyapısal servislerin implementasyonları bu katmanda bulunur. Application katmanındaki arayüzleri somutlaştırır ve Application katmanına bağımlıdır. Ayrıca, bağımlılık enjeksiyonu ayarları (Extensions) da bu katmanda yer alabilir.

* **BlogApp.Web:** Kullanıcı arayüzünü sunan ASP.NET Core MVC projesidir. Controller'lar aracılığıyla Application katmanındaki işlevlere erişir, kullanıcıdan gelen istekleri işler ve View'lar aracılığıyla kullanıcıya bilgi sunar. Custom Attribute'lar aracılığıyla kullanıcıların yetkileri ve rolleri kontrol edilerek sadece ilgili işlemleri gerçekleştirmeleri sağlanır. Application ve Infrastructure katmanlarına bağımlıdır.

## Projenin Özellikleri

Bu blog uygulaması, farklı kullanıcı rolleri için çeşitli özellikler sunmaktadır:

* **Misafir Kullanıcılar:**
    * Blog gönderilerini listeleyebilir.
    * Blog gönderilerini farklı kriterlere göre filtreleyebilir (örneğin, kategoriye göre).
    * Blog gönderilerinin detay sayfalarını görüntüleyebilir.
    * **Yapay Zeka Entegrasyonu:** Blog listeleme sayfasında, eğer bir gönderi yetişkinlere özel olarak işaretlenmişse, bu durum bir bilgi kartıyla belirtilir.

* **Kayıtlı Kullanıcılar:**
    * Misafir kullanıcıların tüm yeteneklerine sahiptir.
    * Yeni blog gönderileri ekleyebilir.
    * Mevcut blog gönderilerini güncelleyebilir.
    * Kendi blog gönderilerini silebilir.
    * Blog gönderilerine yorum yapabilir.
    * **Medya:** Blog gönderilerine isteğe bağlı olarak resim ekleyebilirler.
    * **Yapay Zeka Entegrasyonu:** Blog ekleyen kullanıcılar, içeriklerini OpenAI API'si aracılığıyla analiz ederek içeriğin genel kitleye mi yoksa yalnızca yetişkinlere mi uygun olduğunu belirleyebilirler. Ayrıca, yapılan yorumlar da OpenAI tarafından küfür içerip içermediği yönünde denetlenir ve küfürlü olduğu tespit edilen yorumlar kullanıcılara "Küfürlü İçerik" şeklinde uyarılır.
    * **Güvenlik:** Kullanıcı şifreleri veritabanında hem hashlenmiş hem de salt değeriyle birlikte saklanmaktadır.
    * **Yetkilendirme:** Custom Attribute'lar aracılığıyla kullanıcıların yetkileri ve rolleri kontrol edilerek sadece ilgili işlemleri gerçekleştirmeleri sağlanır.

* **Admin Kullanıcıları:**
    * Kayıtlı kullanıcıların tüm yeteneklerine sahiptir.
    * Blog kategorilerini yönetebilir (yeni kategori ekleme, mevcut kategorileri düzenleme).

## Kurulum ve Çalıştırma

Bu projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

1.  **Gerekli Yazılımlar:**
    * **.NET SDK:** ASP.NET Core uygulamalarını çalıştırmak için gereklidir. En güncel sürümünü [bu adresten](https://dotnet.microsoft.com/download) indirebilirsiniz. (Önerilen sürüm: Projeniz hangi .NET sürümüyle geliştirildiyse o veya üzeri bir sürüm)
    * **SQL Server:** Veritabanı olarak SQL Server kullanılmıştır. Eğer yerelinizde kurulu değilse, [SQL Server Express](https://www.microsoft.com/tr-tr/sql-server/sql-server-downloads) gibi ücretsiz bir sürümünü indirebilirsiniz.
    * **SQL Server Management Studio (SSMS):** SQL Server veritabanını yönetmek için kullanışlı bir araçtır. [Buradan indirebilirsiniz](https://learn.microsoft.com/tr-tr/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16). (Veritabanını kurduktan sonra bu aracı da kurmanız önerilir.)
    * **(Opsiyonel) Bir IDE:** Visual Studio ([buradan indirebilirsiniz](https://visualstudio.microsoft.com/tr/downloads/)) veya Visual Studio Code ([buradan indirebilirsiniz](https://code.visualstudio.com/download)) gibi bir geliştirme ortamı, projeyi açmak ve üzerinde çalışmak için faydalı olabilir.

2.  **Projeyi Klonlayın:**
    * Terminali (Komut İstemi veya PowerShell) açın.
    * Projenin bulunduğu GitHub deposunun URL'sini kullanarak aşağıdaki komutu çalıştırın:
        ```bash
        git clone [https://github.com/HalilERT4S/BlogApp](https://github.com/HalilERT4S/BlogApp)
        cd BlogApp
        ```

3.  **Yapılandırma Dosyalarının Düzenlenmesi:**
    * Proje klasöründe bulunan `appsettings.json` dosyasını bir metin düzenleyiciyle açın.
    * **Veritabanı Bağlantı Ayarları:** `ConnectionStrings` başlığı altındaki veritabanı bağlantı ayarlarını kendi SQL Server kurulumunuza göre düzenleyin. Aşağıdaki gibi alanları doğru bilgilerle doldurun:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
        }
        ```
        * `YOUR_SERVER_NAME`: SQL Server sunucu adınız (örneğin, `localhost\SQLEXPRESS` veya `.` gibi).
        * `YOUR_DATABASE_NAME`: Oluşturmak istediğiniz veya var olan veritabanının adı.
        * `YOUR_USERNAME`: SQL Server kullanıcı adınız.
        * `YOUR_PASSWORD`: SQL Server kullanıcı şifreniz.
        * `TrustServerCertificate=True`: Geliştirme ortamında sertifika sorunlarını önlemek için eklenebilir, ancak üretim ortamında daha güvenli bir yapılandırma tercih edilmelidir.
    * **OpenAI API Anahtarı:** Aynı `appsettings.json` dosyasında bulunan `AppSettings` bölümündeki `OpenAIKey` alanına size mail yoluyla ulaştırdığım veya kendi OpenAI API anahtarınızı girin:
        ```json
        "AppSettings": {
          "OpenAIKey": "YOUR_OPENAI_API_KEY_HERE"
        }
        ```
        **Önemli Not:** Bu kısmı atlamanız durumunda, blog ekleme, güncelleme ve yorum yapma işlevlerinde hatalarla karşılaşabilirsiniz.

4.  **Bağımlılıkları Yükleyin:**
    * Terminalde (proje dizininizin içindeyken) aşağıdaki komutu çalıştırın:
        ```bash
        dotnet restore
        ```
        Bu komut, projenizin `.csproj` dosyalarında listelenen tüm gerekli NuGet paketlerini indirecektir.

5.  **Veritabanını Güncelleyin (Entity Framework Migrations):**
    * Eğer projenizde Entity Framework Core Migrations kullandıysanız, veritabanınızı oluşturmak veya en son şemayla güncellemek için aşağıdaki komutları çalıştırın:
        ```bash
        dotnet ef database update
        ```
        Bu komut, `appsettings.json` dosyasındaki bağlantı bilgilerinizi kullanarak veritabanınızı oluşturacak veya gerekli şema değişikliklerini uygulayacaktır. Eğer ilk kurulumsa, bu komut veritabanınızı da oluşturabilir (yapılandırmanıza bağlı olarak).

6.  **Uygulamayı Çalıştırın:**
    * Terminalde (hala proje dizininin içindeyken) aşağıdaki komutu çalıştırın:
        ```bash
        dotnet run
        ```
        Bu komut, ASP.NET Core uygulamanızı derleyecek ve çalıştıracaktır. Uygulama genellikle `http://localhost:5xxx` gibi bir adreste çalışmaya başlayacaktır. Terminaldeki çıktıda uygulamanın hangi adreste çalıştığını görebilirsiniz.

## Katkıda Bulunma (Opsiyonel)

Eğer bu projeye katkıda bulunmak isterseniz, lütfen aşağıdaki adımları izleyin:

1.  Projeyi fork edin.
2.  Kendi branch'inizi oluşturun (`git checkout -b feature/harika-ozellik`).
3.  Değişikliklerinizi commit edin (`git commit -am 'Harika bir özellik eklendi'`).
4.  Branch'inizi push edin (`git push origin feature/harika-ozellik`).
5.  Yeni bir Pull Request oluşturun.

Lütfen kod stilimize ve genel proje yapısına dikkat edin.
