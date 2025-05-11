using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class HocPhan
    {
        [Key]
        [StringLength(7)]
        public string MaHP { get; set; }

        [StringLength(4)]
        public string MaNK { get; set; }

        [StringLength(50)]
        public string TenHP { get; set; }

        // Navigation properties
        [ForeignKey("MaNK")]
        public NienKhoa NienKhoa { get; set; }

        public ICollection<LopHocPhan> LopHocPhans { get; set; }
    }

    public class HocPhanRepos
    {

    }

}
