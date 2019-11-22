using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTaxi.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        [MaxLength(6)]
        public string CarNumber { get; set; } //гос номер машины
        [MaxLength(30)]
        public string CarMark { get; set; }
        [MaxLength(30)]
        public string CarModl { get; set; }
        public int? CarColorId { get; set; }
        public CarColor CarColor { get; set; }
        public int? CarClassId { get; set; }
        public CarClass CarClass { get; set; }
        public Driver Driver { get; set; }
    }
}
