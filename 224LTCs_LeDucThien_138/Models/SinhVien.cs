using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class SinhVien
    {
        // MaSV là khóa chính (Primary Key)
        [Key]
        [StringLength(14)] // Độ dài của MaSV là 14 ký tự
        public string MaSV { get; set; }

        // MaLSH là một khóa ngoại, không cần ánh xạ đặc biệt nếu bạn đã có lớp LSH khác
        public int MaLSH { get; set; }

        // TenSV có độ dài tối đa là 50 ký tự
        [StringLength(50)]
        public string TenSV { get; set; }

        // GioiTinh là kiểu BIT, có thể là true/false
        public bool GioiTinh { get; set; }

        // NgaySinh là kiểu DATE
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        // Cccd có độ dài là 12 ký tự
        [StringLength(12)]
        public string Cccd { get; set; }

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
        public SinhVien() { }

        // Constructor có tham số để khởi tạo các thuộc tính
        public SinhVien(string maSV, int maLSH, string tenSV, bool gioiTinh, DateTime? ngaySinh, string cccd,
                        string diaChi, string sdt, string email, string matKhau, string anh)
        {
            MaSV = maSV;
            MaLSH = maLSH;
            TenSV = tenSV;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            Cccd = cccd;
            DiaChi = diaChi;
            Sdt = sdt;
            Email = email;
            MatKhau = matKhau;
            Anh = anh;
        }
    }

    public class SinhVienRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public SinhVienRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }
    }
}
