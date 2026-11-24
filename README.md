# BudgetManager

Görsel programlama dersi için C# ve Windows Forms ile geliştirdiğim kişisel bütçe yönetimi uygulaması. Gelir ve giderlerinizi kaydeder, kategorilere göre grafiklerle özetler. Uygulama arayüzü İngilizcedir.

## Özellikler

- **Kullanıcı hesabı:** Kayıt olma, giriş yapma ve şifre değiştirme.
- **Gelir/gider takibi:** Tarih, tür, kategori, açıklama ve tutarla kayıt ekleme ve silme.
- **Özet:** Toplam gelir, toplam gider ve bakiye.
- **Raporlama:** Kategorilere göre gelir ve gider pasta grafikleri, istatistikler.

## Kullanılan teknolojiler

| Katman | Araç |
| --- | --- |
| Dil ve platform | C#, .NET Framework 4.7.2 |
| Arayüz | Windows Forms, Bunifu UI |
| Grafikler | Windows Forms DataVisualization |
| Veritabanı | SQLite |

## Çalıştırma

1. Depoyu Windows'ta klonlayın ve `Proje_1.sln` dosyasını Visual Studio ile açın.
2. NuGet paketlerini geri yükleyin.
3. Projeyi derleyip başlatın. Veritabanı dosyası yoksa ilk açılışta oluşturulur.

> Arayüzde kullanılan Bunifu bileşenleri ticari lisanslıdır.

## Proje yapısı

| Form | Görevi |
| --- | --- |
| `Form1` | Giriş ve kayıt |
| `Form2` | Ana menü |
| `Form3` | Kullanıcı ayarları ve şifre değiştirme |
| `Form4` | Gelir/gider yönetimi |
| `Form5` | Raporlar ve grafikler |

## Not

Bu bir ders projesidir. Şifreler veritabanında düz metin olarak saklandığı için gerçek bir şifrenizi kullanmayın.
