using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class PhongHoc
    {
        // MaPhong là khóa chính và sử dụng Identity trong CSDL
        [Key]
        public int MaPhong { get; set; }

        // TenPhong có độ dài tối đa là 50 ký tự
        [StringLength(50)]
        public string TenPhong { get; set; }

        // SucChua là kiểu INT, không cần giới hạn độ dài
        public int SucChua { get; set; }

        // Constructor không có tham số
        public PhongHoc() { }

        // Constructor có tham số
        public PhongHoc(int maPhong, string tenPhong, int sucChua)
        {
            MaPhong = maPhong;
            TenPhong = tenPhong;
            SucChua = sucChua;
        }
    }

    public class PhongHocRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public PhongHocRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }
    }
}
