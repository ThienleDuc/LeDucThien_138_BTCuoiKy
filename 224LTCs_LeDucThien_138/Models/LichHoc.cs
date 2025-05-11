using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class LichHoc
    {
        [Key]
        public int? MaLich { get; set; }

        [StringLength(2)]
        public string ThuNgay { get; set; }

        public int? TietBatDau { get; set; }

        public int? TietKetThuc { get; set; }

        // Navigation property
        public ICollection<LopHocPhan> LopHocPhans { get; set; }
    }

    public class LichHocRepos
    {

    }

}
