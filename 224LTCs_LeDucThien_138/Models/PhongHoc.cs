using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace _224LTCs_LeDucThien_138.Models
{
    public class PhongHoc
    {
        // MaPhong là khóa chính và sử dụng Identity trong CSDL
        [Key]
        public int MaPhong { get; set; }

        // TenPhong có độ dài tối đa là 50 ký tự
        [StringLength(50)]
        public string? TenPhong { get; set; }

        // SucChua là kiểu INT, không cần giới hạn độ dài
        public int? SucChua { get; set; }

        // Constructor không có tham số
        public PhongHoc() { }

        // Constructor có tham số

        public PhongHoc(int maPhong, string tenPhong, int? sucChua)
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

        public List<PhongHoc> GetAllPhong()
        {
            List<PhongHoc> list = new List<PhongHoc>();
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaPhong, TenPhong, SucChua FROM Phong;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhongHoc
                        {
                            MaPhong = reader.GetInt32(reader.GetOrdinal("MaPhong")),
                            TenPhong = reader["TenPhong"]?.ToString(),
                            SucChua = reader["SucChua"] != null ? Convert.ToInt32(reader["SucChua"]) : null
                        });
                    }
                }
            }

            return list;
        }

        public PhongHoc GetPhongnById(int maPhong)
        {
            PhongHoc phong = null;
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaPhong, TenPhong, SucChua 
                        FROM Phong
                        WHERE MaPhong = @MaPhong;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        phong = new PhongHoc
                        {
                            MaPhong = reader.GetInt32(reader.GetOrdinal("MaPhong")),
                            TenPhong = reader["TenPhong"]?.ToString(),
                            SucChua = reader["SucChua"] != null ? Convert.ToInt32(reader["SucChua"]) : null
                        };
                    }
                }
            }

            return phong;
        }

        public List<PhongHoc> TimKiemPhong(string? keyword)
        {
            List<PhongHoc> danhSach = new List<PhongHoc>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SearchPhong", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", !string.IsNullOrEmpty(keyword) ? keyword : (object)DBNull.Value);
                
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PhongHoc phong = new PhongHoc
                        {
                            MaPhong = reader.GetInt32(reader.GetOrdinal("MaPhong")),
                            TenPhong = reader["TenPhong"]?.ToString(),
                            SucChua = reader["SucChua"] != null ? Convert.ToInt32(reader["SucChua"]) : null
                        };

                        danhSach.Add(phong);
                    }
                }
            }

            return danhSach;
        }

        public bool IsTenPhongExists(string? tenPhong)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Phong WHERE TenPhong = @TenPhong";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenPhong", !string.IsNullOrEmpty(tenPhong) ? tenPhong : (object)DBNull.Value);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool IsThisPhong(int maPhong, string? tenPhong)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Phong WHERE TenPhong = @TenPhong AND MaPhong = @MaPhong";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                    cmd.Parameters.AddWithValue("@TenPhong", !string.IsNullOrEmpty(tenPhong) ? tenPhong : (object)DBNull.Value);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool AddPhong(PhongHoc phong)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("AddPhong", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenPhong", phong.TenPhong ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SucChua", phong.SucChua.HasValue ? phong.SucChua : (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool UpdatePhong(PhongHoc phong)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("UpdatePhong", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaPhong", phong.MaPhong);
                cmd.Parameters.AddWithValue("@TenPhong", phong.TenPhong ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SucChua", phong.SucChua.HasValue ? phong.SucChua : (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeletePhong(int maPhong)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = "DELETE FROM Phong WHERE MaPhong = @MaPhong";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhong", maPhong);

                conn.Open();

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

    }
}
