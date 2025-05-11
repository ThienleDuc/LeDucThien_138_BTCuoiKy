using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class MonHoc
    {
        [Key]
        public int? MaMH { get; set; }

        [StringLength(50)]
        public string TenMH { get; set; }

        // Navigation property
        public ICollection<LopHocPhan> LopHocPhans { get; set; }
    }

    public class MonHocRepos
    {

    }
}
