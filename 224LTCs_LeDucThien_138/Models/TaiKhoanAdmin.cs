using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace _224LTCs_LeDucThien_138.Models
{
    public class TaiKhoanAdmin
    {
        [Key]
        [StringLength(10)]
        public string MaTaiKhoan { get; set; }

        [StringLength(10)]
        public string MatKhau { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        public bool GioiTinh { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        public string DiaChi { get; set; }

        [StringLength(10)]
        public string Sdt { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Anh { get; set; }

        public TaiKhoanAdmin() { }

        public TaiKhoanAdmin(string maTaiKhoan, string matKhau, string hoTen, bool gioiTinh,
                             DateTime ngaySinh, string diaChi, string sdt, string email, string anh)
        {
            MaTaiKhoan = maTaiKhoan;
            MatKhau = matKhau;
            HoTen = hoTen;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
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
                string query = @"SELECT MaTaiKhoan, MatKhau, HoTen, GioiTinh, NgaySinh, DiaChi, Sdt, Email, Anh FROM TaiKhoanAdmin;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TaiKhoanAdmin
                        {
                            MaTaiKhoan = reader.GetString(reader.GetOrdinal("MaTaiKhoan")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
                            HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader.GetDateTime("NgaySinh") : DateTime.Now,
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Anh = reader.GetString(reader.GetOrdinal("Anh"))
                        });
                    }
                }
            }

            return list;
        }


        public TaiKhoanAdmin GetAdminById(string maTaiKhoan)
        {
            TaiKhoanAdmin admin = null;
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaTaiKhoan, MatKhau, HoTen, GioiTinh, NgaySinh, DiaChi, Sdt, Email, Anh 
                         FROM TaiKhoanAdmin 
                         WHERE MaTaiKhoan = @MaTaiKhoan;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        admin = new TaiKhoanAdmin
                        {
                            MaTaiKhoan = reader.GetString(reader.GetOrdinal("MaTaiKhoan")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
                            HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader.GetDateTime("NgaySinh") : DateTime.Now,
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Anh = reader.GetString(reader.GetOrdinal("Anh"))
                        };
                    }
                }
            }

            return admin;
        }

        public bool UpdateTaiKhoanAdmin(TaiKhoanAdmin taiKhoanAdmin)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("UpdateTaiKhoanAdmin", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaTaiKhoan", taiKhoanAdmin.MaTaiKhoan ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MatKhau", taiKhoanAdmin.MatKhau ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@HoTen", taiKhoanAdmin.HoTen ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", taiKhoanAdmin.GioiTinh ? 1: 0);
                cmd.Parameters.AddWithValue("@NgaySinh", taiKhoanAdmin.NgaySinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", taiKhoanAdmin.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sdt", taiKhoanAdmin.Sdt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", taiKhoanAdmin.Email ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool UpdateAvatarTaiKhoanAdmin(TaiKhoanAdmin taiKhoanAdmin)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("UpdateAvatarTaiKhoanAdmin", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaTaiKhoan", taiKhoanAdmin.MaTaiKhoan ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Anh", taiKhoanAdmin.Anh ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}
