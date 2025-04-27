using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class GiangVien
    {
        // MaGV là khóa chính (Primary Key)
        [Key]
        [StringLength(10)] // Độ dài của MaGV là 10 ký tự
        public string MaGV { get; set; }

        // MaHocVi, MaChucVu, MaKhoa là các khóa ngoại, không cần ánh xạ đặc biệt nếu bạn đã có lớp khác cho chúng
        public int MaHocVi { get; set; }
        public int MaChucVu { get; set; }
        public int MaKhoa { get; set; }

        // TenGV có độ dài tối đa là 50 ký tự
        [StringLength(50)]
        public string TenGV { get; set; }

        // GioiTinh là kiểu BIT, có thể là true/false
        public bool GioiTinh { get; set; }

        // DiaChi là kiểu NVARCHAR(MAX)
        public string DiaChi { get; set; }

        // Sdt có độ dài là 10 ký tự
        [StringLength(10)]
        public string Sdt { get; set; }

        // Email là kiểu VARCHAR(MAX), có thể có độ dài linh hoạt
        [EmailAddress]
        public string Email { get; set; }

        // MatKhau có độ dài tối đa là 10 ký tự
        [StringLength(10)]
        public string MatKhau { get; set; }

        // Anh là kiểu VARCHAR(MAX), có thể chứa link ảnh hoặc dữ liệu ảnh
        public string Anh { get; set; }

        // Constructor không tham số
        public GiangVien() { }

        // Constructor có tham số để khởi tạo các thuộc tính
        public GiangVien(string maGV, int maHocVi, int maChucVu, int maKhoa, string tenGV, bool gioiTinh,
                         string diaChi, string sdt, string email, string matKhau, string anh)
        {
            MaGV = maGV;
            MaHocVi = maHocVi;
            MaChucVu = maChucVu;
            MaKhoa = maKhoa;
            TenGV = tenGV;
            GioiTinh = gioiTinh;
            DiaChi = diaChi;
            Sdt = sdt;
            Email = email;
            MatKhau = matKhau;
            Anh = anh;
        }
    }
    public class GiangVienRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public GiangVienRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }
    }
}
