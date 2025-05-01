using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class LopSinhHoat
    {
        [Key]
        public int MaLSH { get; set; }

        public int? MaNganh { get; set; }

        [StringLength(10)]
        public string TenLSH { get; set; }

        [StringLength(4)]
        public string MaNK { get; set; }

        public ICollection<SinhVien> SinhViens { get; set; }
    }
}
