using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

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

        public HocPhan(string maHP, string maNK, string tenHP, NienKhoa nienKhoa, ICollection<LopHocPhan> lopHocPhans)
        {
            MaHP = maHP;
            MaNK = maNK;
            TenHP = tenHP;
            NienKhoa = nienKhoa;
            LopHocPhans = lopHocPhans;
        }

        public HocPhan()
        {
        }
    }

    public class HocPhanRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public HocPhanRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<HocPhan> GetHocPhanByNienKhoa(string maNK)
        {
            List<HocPhan> list = new List<HocPhan>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaHP, MaNK, TenHP 
                         FROM HocPhan
                         WHERE MaNK = @MaNK
                         ORDER BY MaHP DESC;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNK", maNK);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new HocPhan
                        {
                            MaHP = reader.IsDBNull(reader.GetOrdinal("MaHP")) ? null : reader.GetString(reader.GetOrdinal("MaHP")),
                            MaNK = reader.IsDBNull(reader.GetOrdinal("MaNK")) ? null : reader.GetString(reader.GetOrdinal("MaNK")),
                            TenHP = reader.IsDBNull(reader.GetOrdinal("TenHP")) ? null : reader.GetString(reader.GetOrdinal("TenHP"))
                        });
                    }
                }
            }

            return list;
        }

    }

}
