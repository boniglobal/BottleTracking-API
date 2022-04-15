namespace Core.Models
{
    public class PagedData<T>
    {
        /// <summary>Veriler</summary>
        public List<T> Items { get; set; }
        /// <summary>Önceki sayfa</summary>
        /// <example>false</example>
        public bool Previous { get; set; }
        /// <summary>Sonraki sayfa</summary>
        /// <example>true</example>
        public bool Next { get; set; }
        ///<summary>Sayfa numarası</summary>
        ///<example>1</example>
        public int PageNumber { get; set; }
        ///<summary>Alınan toplam veri sayısı</summary>
        ///<example>1</example>
        public int TotalCount { get; set; }
    }
}
