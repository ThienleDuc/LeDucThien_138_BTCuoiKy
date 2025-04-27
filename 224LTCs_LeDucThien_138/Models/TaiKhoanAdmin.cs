using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class TaiKhoanAdmin
    {
        // MaTaiKhoan là khóa chính (Primary Key)
        [Key]
        [StringLength(10)] // Độ dài của MaTaiKhoan là 10 ký tự
        public string MaTaiKhoan { get; set; }

        // MatKhau có độ dài tối đa là 10 ký tự
        [StringLength(10)]
        public string MatKhau { get; set; }

        // HoTen có độ dài tối đa là 50 ký tự
        [StringLength(50)]
        public string HoTen { get; set; }

        // GioiTinh là kiểu BIT, có thể là true/false
        public bool GioiTinh { get; set; }

        // DiaChi là kiểu NVARCHAR(MAX)
        public string DiaChi { get; set; }

        // Sdt có độ dài là 10 ký tự
        [StringLength(10)]
        public string Sdt { get; set; }

        // Email có thể chứa địa chỉ email, không có độ dài giới hạn cụ thể
        [EmailAddress]
        public string Email { get; set; }

        // Anh là kiểu VARCHAR(MAX), có thể chứa link ảnh hoặc dữ liệu ảnh
        public string Anh { get; set; }

        // Constructor không tham số
        public TaiKhoanAdmin() { }

        // Constructor có tham số để khởi tạo các thuộc tính
        public TaiKhoanAdmin(string maTaiKhoan, string matKhau, string hoTen, bool gioiTinh,
                              string diaChi, string sdt, string email, string anh)
        {
            MaTaiKhoan = maTaiKhoan;
            MatKhau = matKhau;
            HoTen = hoTen;
            GioiTinh = gioiTinh;
            DiaChi = diaChi;
            Sdt = sdt;
            Email = email;
            Anh = anh;
        }
    }

    public class TaiKhoanAdminRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public TaiKhoanAdminRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<TaiKhoanAdmin> GetAllAdmin()
        {
            List<TaiKhoanAdmin> list = new List<TaiKhoanAdmin>();
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaTaiKhoan, MatKhau, HoTen, GioiTinh, DiaChi, Sdt, Email, Anh FROM TaiKhoanAdmin;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TaiKhoanAdmin
                        {
                            MaTaiKhoan = reader["MaTaiKhoan"].ToString(),
                            MatKhau = reader["MatKhau"].ToString(),
                            HoTen = reader["HoTen"].ToString(),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            DiaChi = reader["DiaChi"].ToString(),
                            Sdt = reader["Sdt"].ToString(),
                            Email = reader["Email"].ToString(),
                            Anh = reader["Anh"].ToString()
                        });
                    }
                }
            }

            return list;
        }

    }
}
