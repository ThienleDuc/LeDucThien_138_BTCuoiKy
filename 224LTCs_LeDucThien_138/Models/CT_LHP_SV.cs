using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class CT_LHP_SV
    {
        [Key, Column(Order = 0)]
        [StringLength(14)]
        public string MaSV { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(20)]
        public string MaLHP { get; set; }

        public string GhiChu { get; set; }

        // Navigation property
        [ForeignKey("MaLHP")]
        public LopHocPhan LopHocPhan { get; set; }

        // Nếu bạn có class SinhVien thì thêm:
        [ForeignKey("MaSV")]
        public SinhVien SinhVien { get; set; }
    }

    public class CT_LHP_SVRepos
    {

    }

}
