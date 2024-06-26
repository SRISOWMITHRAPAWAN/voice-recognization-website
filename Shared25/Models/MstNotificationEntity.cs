using System.ComponentModel.DataAnnotations;

namespace Shared25.Models
{
    public class MstNotificationEntity
    {

        public int NotificationID { get; set; }



        [Required]
        public string? NotificationName { get; set; }



        [Required]
        public string? Lock { get; set; }


    }
}