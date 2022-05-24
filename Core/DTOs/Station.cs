using static Core.Constants.StationConstants;

namespace Core.DTOs
{
    public class Station
    {
        public class StationListView
        {
            /// <summary>Benzersiz kimlik numarası</summary>
            /// <example>1</example>
            public int Id { get; set; }
            /// <summary>Oluşturulma tarihi</summary>
            public DateTimeOffset CreateDate { get; set; }
            /// <summary>Dolum bandı numarası</summary>
            /// <example>1</example>
            public int ProductionLine { get; set; }
            /// <summary>Dolum öncesi-sonrası bilgisi tipi</summary>
            /// <example>1</example>
            public Locations Location { get; set; }
            /// <summary>İstasyona atanan kullanıcının benzersiz kimlik numarası</summary>
            /// <example>1</example>
            public int? PanelUserId { get; set; }
            /// <summary>Kullanıcı adı ve soyadı</summary>
            /// <example>Uğur Timurçin</example>
            public string Fullname { get; set; }
        }

        public class StationAdd
        {
            /// <summary>Dolum bandı numarası</summary>
            /// <example>2</example>
            public int ProductionLine { get; set; }
            /// <summary>Dolum öncesi-sonrası bilgisi</summary>
            /// <example>1</example>
            public Locations Location { get; set; }
            /// <summary>İstasyona atanacak kullanıcının benzersiz kimlik numarası</summary>
            /// <example>2</example>
            public int PanelUserId { get; set; }
        }

        public class StationUpdate
        {
            /// <summary>Güncellenecek istasyonun benzersiz kimlik numarası</summary>
            /// <example>3</example>
            public int StationId { get; set; }
            /// <summary>Band numarası</summary>
            /// <example>4</example>
            public int ProductionLine { get; set; }
            /// <summary>Konum</summary>
            /// <example>1</example>
            public Locations Location { get; set; }
            /// <summary>İstasyona atanacak kullanıcının benzersiz kimlik numarası</summary>
            /// <example>3</example>
            public int PanelUserId { get; set; }
        }

        public class StationStatistics
        {
            /// <summary>Toplam band sayısı </summary>
            /// <example>8</example>
            public int TotalNumberOfLines { get; set; }
            /// <summary>Toplam istasyon sayısı</summary>
            /// <example>4</example>
            public int TotalNumberOfStations { get; set; }
        }
    }
}
