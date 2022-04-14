using static Core.Constants.BottleConstants;

namespace Core.DTOs
{
    public class StationLog
    {
        public class StationLogGetResponse
        {
            /// <summary>Benzersiz kimlik numarası</summary>
            /// <example>1</example>
            public int Id { get; set; }
            /// <summary>Üretim tarihi</summary>
            /// <example>2022-04-06T09:24:50.375521+00:00</example>
            public DateTimeOffset BottleProductionDate { get; set; }
            /// <summary>Damacana için oluşturulan benzersiz kimlik</summary>
            /// <example>e04646f9-4a82-4aed-9298-0f9cc4c6a994</example>
            public string TrackingId { get; set; }
            /// <summary>Dolum bandı numarası</summary>
            /// <example>1</example>
            public string ProductionLine { get; set; }
            /// <summary>Dolum öncesi-sonrası bilgisi</summary>
            /// <example>1</example>
            public int Location { get; set; }
            /// <summary>Damacana kullanım durumu</summary>
            /// <example>1</example>
            public UsageStatus BottleStatus { get; set; }
            /// <summary>Sorgunun yapıldığı tarih </summary>
            /// <example>2022-04-06T09:24:50.375521+00:00</example>
            public DateTimeOffset CreatedDate { get; set; }
        }

        public class StationLogStatistics
        {
            /// <summary>Toplam sorgu sayısı</summary>
            /// <example>9</example>
            public int TotalQuery { get; set; }
            /// <summary>Günlük ortalama sorgu sayısı</summary>
            /// <example>3</example>
            public int AverageOfDailyQuery { get; set; }
            /// <summary>Bugün yapılan sorgu sayısı</summary>
            /// <example>3</example>
            public int NumberOfTodayQuery { get; set; }
            /// <summary>Bugünkü damacana sayısı</summary>
            /// <example>3</example>
            public int NumberOfTodayBottle { get; set; }
        }
    }
}
