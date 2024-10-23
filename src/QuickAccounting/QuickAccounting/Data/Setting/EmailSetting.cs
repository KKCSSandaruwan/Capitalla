using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting
{
    public class EmailSetting
    {
        [Key]
        public int EmailSettingId { get; set; }
        [Required]
        public string MailHost { get; set; }
        [Required]
        public int MailPort { get; set; }
        [Required]
        public string MailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string MailFromname { get; set; }
        public string EncryptionName { get; set; }
    }
}
