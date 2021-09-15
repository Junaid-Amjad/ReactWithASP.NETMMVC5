using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class LogFile
    {
        [Key]
        public Int64 ID { get; set; }
        public string TransactionID { get; set; }
        public string sqlCommand { get; set; }
        public string Description { get; set; }
        public DateTime EntryDate { get; set; }
        public string UserID { get; set; }
        public string UserIP { get; set; }
        public string UserSystem { get; set; }
    }
}