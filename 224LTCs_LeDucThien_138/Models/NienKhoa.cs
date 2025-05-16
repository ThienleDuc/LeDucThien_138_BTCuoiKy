using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class NienKhoa
    {
        [Key]
        [StringLength(4)]
        public string MaNK { get; set; }

        [StringLength(50)]
        public string TenNK { get; set; }

        ICollection<LopSinhHoat> LopSinhHoat { get; set; }

        public NienKhoa(string maNK, string tenNK, ICollection<LopSinhHoat> lopSinhHoat)
        {
            MaNK = maNK;
            TenNK = tenNK;
            LopSinhHoat = lopSinhHoat;
        }

        public NienKhoa()
        {
        }
    }

    public class NienKhoaRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public NienKhoaRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<NienKhoa> GetAllNienKhoa()
        {
            List<NienKhoa> list = new List<NienKhoa>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = "SELECT MaNK, TenNK FROM NienKhoa ORDER BY MaNK DESC;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new NienKhoa
                        {
                            MaNK = reader.GetString(reader.GetOrdinal("MaNK")),
                            TenNK = reader.GetString(reader.GetOrdinal("TenNK"))
                        });
                    }
                }
            }

            return list;
        }

    }
}
