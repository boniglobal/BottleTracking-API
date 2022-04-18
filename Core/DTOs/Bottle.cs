using static Core.Constants.BottleConstants;

namespace Core.DTOs
{
    public class Bottle
    {
        public class BottleStatusGetResponse
        {
            /// <summary>Damacana takibi için oluşturulan benzersiz kimlik</summary>
            /// <example>e04646f9-4a82-4aed-9298-0f9cc4c6a994</example>
            public string TrackingId { get; set; }
            public int QrPrintCount { get; set; }
            /// <summary>Damacana kullanım durumu</summary>
            /// <example>2</example>
            public UsageStatus Status { get; set; }
            /// <summary>Üretim tarihi</summary>
            /// <example>2022-04-06T09:24:50.375521+00:00</example>
            public DateTimeOffset ProductionDate { get; set; }
            /// <summary>Dolum sayısı</summary>
            /// <example>3</example>
            public int RefillCount { get; set; }
            /// <summary>Son dolum tarihi</summary>
            /// <example>2022-04-08T09:24:50.375521+00:00</example>
            public DateTimeOffset LastRefillDate { get; set; }
        }

        public class BottleView
        {
            /// <summary>Benzersiz kimlik numarası</summary>
            /// <example>123</example>
            public int Id { get; set; }
            /// <summary>Üretim tarihi</summary>
            /// <example>2022-04-06T09:24:50.375521+00:00</example>
            public DateTimeOffset ProductionDate { get; set; }
            /// <summary>Toplam dolum sayısı</summary>
            /// <example></example>
            public int RefillCount { get; set; }
            /// <summary>Son dolum tarihi</summary>
            /// <example>2022-04-06T09:24:50.375521+00:00</example>
            public DateTimeOffset LastRefillDate { get; set; }
            /// <summary>Damacana türü</summary>
            /// <example>1</example>
            public BottleTypes BottleType { get; set; }
            /// <summary>Damacana için oluşturulan benzersiz QR kodu </summary>
            /// <example>725eb7d6-d06e-419d-9e3c-e6f194733e4420224</example>
            public string QrCode { get; set; }
            /// <summary>Toplam basılan QR kod sayısı</summary>
            /// <example>1</example>
            public int QrPrintCount { get; set; }
            /// <summary>Damacana kullanım durumu</summary>
            /// <example>1</example>
            public UsageStatus Status { get; set; }
            /// <summary>Damacanın sisteme kaydedildiği tarih</summary>
            /// <example>2022-04-06T09:24:50.375521+00:00</example>
            public DateTimeOffset CreateDate { get; set; }
            /// <summary>Damacana takibi için oluşturulan benzersiz kimlik</summary>
            /// <example>e04646f9-4a82-4aed-9298-0f9cc4c6a994</example>
            public string TrackingId { get; set; }
        }

        public class BottleAdd
        {
            /// <summary>
            /// Üretim tarihi
            /// <br />
            /// <b>Not: </b>Üretim tarihi 'AA/YYYY' formatında olmalı. Örneğin; 01/2022
            /// </summary><br />
            /// <example>01/2022</example>
            public string ProductionDate { get; set; }
            /// <summary>Dolum sayısı<br />Belirtilmez ise sisteme 0 olarak kaydedilir.</summary>
            public int? RefillCount { get; set; }
            /// <summary>Damacana türü</summary>
            /// <example>1</example>
            public BottleTypes BottleType { get; set; }
        }
        public class BottleUpdate
        {
            /// <summary>Güncellenecek damacananın benzersiz kimlik numarası</summary>
            /// <example>1</example>
            public int Id { get; set; }
            /// <summary>
            /// Üretim tarihi
            /// <br />
            /// <b>Not: </b>Üretim tarihi 'AA/YYYY' formatında olmalı. Örneğin; 01/2022
            /// </summary><br />
            /// <example>01/2022</example>
            public string ProductionDate { get; set; }
            /// <summary>Damacana türü</summary>
            /// <example>1</example>
            public BottleTypes BottleType { get; set; }
        }

        public class BottleStatistics
        {
            /// <summary>Sisteme kayıtlı toplam damacana sayısı</summary>
            /// <example>521</example>
            public int TotalNumberOfBottles { get; set; }
            /// <summary>Toplam kullanıma müsait damacana sayısı</summary>
            /// <example>300</example>
            public int TotalNumberOfBottlesInUse { get; set; }
            /// <summary>Toplam son kullanma tarihi geçmiş ama hala kullanılan damacana sayısı</summary>
            /// <example>121</example>
            public int TotalNumberOfBottlesExpiredAndInUse { get; set; }
            /// <summary>Toplam son kullanma tarihi geçmiş ve kullanımda olmayan damacana sayısı</summary>
            /// <example>100</example>
            public int TotalNumberOfBottlesOutOfUse { get; set; }
        }
    }
}
