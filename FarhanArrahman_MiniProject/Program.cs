using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FarhanArrahman_MiniProject
{
    class Program
    {
        SqlConnection sqlConnection;

        string connectionString = "Data Source=localhost;" +
            "Initial Catalog=DbAppPresensi;User ID=sa;Password=SQL Server 2017;";

        static Program program = new Program();

        string[] pegawaiSession = new string[] { };
        int akunSession = 0;

        static void Main(string[] args)
        {
            program.MainMenu();
        }

        void MainMenu() // Method Void
        {
            Console.WriteLine("-- Aplikasi Presensi --");
            if (pegawaiSession.Length > 0)
            {
                Console.WriteLine("Halo " + pegawaiSession[1]);
                Console.WriteLine("1. Daftar");
                Console.WriteLine("2. Lihat Data Pegawai");
                Console.WriteLine("3. Login");
                Console.WriteLine("7. Ubah Password");
                Console.WriteLine("8. Hapus Akun");
                Console.WriteLine("9. Logout/Keluar");
                Console.Write("Masukkan Pilihan : ");
                program.Processor(program.NumberPicker());
            }
            else
            {
                Console.WriteLine("1. Daftar");
                Console.WriteLine("2. Lihat Data Pegawai");
                Console.WriteLine("3. Login");
                Console.WriteLine("7. Ubah Password");
                Console.WriteLine("8. Hapus Akun");
                Console.WriteLine("9. Keluar/Logout");
                Console.Write("Masukkan Pilihan : ");
                program.Processor(program.NumberPicker());
            }
        }

        char NumberPicker() // Method Non-Void
        {
            char chosenNumber = Convert.ToChar(Console.ReadLine());
            return chosenNumber;
        }

        void Processor(char chosenNumber) // Method Void
        {
            switch (chosenNumber) // Decision
            {
                case '1':
                    //Console.Clear();
                    Pegawai pegawai = new Pegawai();
                    Pegawai dataPegawai = program.Register();
                    Akun akunPegawai = program.CreateAccount(dataPegawai);
                    program.InsertPegawai(dataPegawai);
                    program.InsertAkun(akunPegawai);
                    pegawaiSession = new string[2] { dataPegawai.Id, dataPegawai.Nama };
                    akunSession = akunPegawai.Id;
                    //Console.Clear();
                    Console.WriteLine("Berhasil Mendaftar dan Login!");
                    Console.WriteLine("Segera Ubah Password Anda Sebelum Logout!\n");
                    program.MainMenu();
                    break;
                case '2':
                    //Console.Clear();
                    Console.WriteLine("\n-- Data Pegawai --");
                    program.SelectAllPegawai();
                    Console.Write("\n");
                    program.MainMenu();
                    break;
                case '3':
                    //Console.Clear();
                    string[] forSession = program.Login();
                    //Console.Clear();
                    if (forSession.Length == 0)
                    {
                        Console.WriteLine("Email/Password Salah!");
                    }
                    else
                    {
                        pegawaiSession = new string[2] { forSession[1], forSession[2] };
                        akunSession = Convert.ToInt32(forSession[0]);
                        Console.WriteLine("\nBerhasil Login!\n");
                    }
                    program.MainMenu();
                    break;
                case '7':
                    if (akunSession == 0)
                    {
                        Console.WriteLine("\nAnda Belum Login!\n");
                    }
                    else
                    {
                        Console.WriteLine("\n-- Ubah Password --");
                        Console.Write("Masukkan Password Baru: ");
                        string userPw = Console.ReadLine();
                        program.Update(userPw, akunSession);
                        Console.WriteLine("\nBerhasil Ubah Password!\n");
                    }
                    program.MainMenu();
                    break;
                case '8':
                    if (akunSession == 0)
                    {
                        Console.WriteLine("\nAnda Belum Login!\n");
                    }
                    else
                    {
                        Console.WriteLine("\n-- Hapus Akun --");
                        Console.Write("Anda Yakin Menghapus Akun [Y/n]? ");
                        string userInput = Console.ReadLine();
                        if (userInput == "Y" || userInput == "y")
                        {
                            program.Delete(pegawaiSession[0], akunSession);
                            pegawaiSession = new string[] { };
                            akunSession = 0;
                            Console.WriteLine("\nBerhasil Menghapus Akun!\n");
                        }
                    }
                    program.MainMenu();
                    break;
                case '9':
                    if (akunSession > 0)
                    {
                        pegawaiSession = new string[] { };
                        akunSession = 0;
                        Console.WriteLine("\nBerhasil Logout!\n");
                        program.MainMenu();
                    }
                    else
                    {
                        Console.WriteLine("\nSampai Jumpa!");
                        break;
                    }
                    break;
                default:
                    //Console.Clear();
                    Console.WriteLine("Pilihan tidak tersedia.\n");
                    program.MainMenu();
                    break;
            }
        }

        Pegawai Register() // Method Non-Void
        {
            Console.WriteLine("\n-- Form. Pendaftaran --");
            string[] variables = new string[]
            {
                "Nama",
                "Tempat Lahir",
                "Tanggal Lahir (YYYY-MM-DD)",
                "Gender (L/P)",
                "Agama",
                "Alamat",
                "Email",
                "Nomor Handphone",
                "Foto (NamaFile.jpeg/jpg/png)"
            };
            string[] data = new string[variables.Length];
            foreach (var variable in variables)
            {
                Console.Write(variable + " : ");
                string userInput = Console.ReadLine();
                data[Array.IndexOf(variables, variable)] = userInput;
            }

            Random random = new Random();
            int posisiId = random.Next(160007, 160017);
            int kantorId;
            if (posisiId < 160011)
            {
                kantorId = 110001;
            }
            else if (posisiId < 160015)
            {
                kantorId = 110002;
            }
            else
            {
                kantorId = 110003;
            }
            Pegawai pegawai = new Pegawai()
            {
                Id = program.PegawaiIdGenerator(),
                Nama = data[0],
                TmptLahir = data[1],
                TglLahir = data[2],
                Gender = Convert.ToChar(data[3]),
                Agama = data[4],
                Alamat = data[5],
                Email = data[6],
                NoHp = data[7],
                Foto = data[8],
                PosisiId = random.Next(160007, 160017),
                KantorId = kantorId
            };
            return pegawai;
        }

        string PasswordGenerator()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[8];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }

        Akun CreateAccount(Pegawai pegawai)
        {
            Akun akun = new Akun()
            {
                Id = program.AkunIdGenerator(),
                PegawaiEmail = pegawai.Email,
                PegawaiNoHp = pegawai.NoHp,
                Pw = program.PasswordGenerator()
            };
            return akun;
        }

        string[] Login() // Method Non-Void
        {
            Console.WriteLine("\n-- Login --");
            Console.Write("Masukkan Email/No. Hp: ");
            string userContact = Console.ReadLine();
            Console.Write("Masukkan Password: ");
            string userPw = Console.ReadLine();
            string[] selected = program.SelectAkun(userContact, userPw);
            //Console.WriteLine(selected[0]);
            string[] forSession;
            if ((userContact == selected[1] || userContact == selected[2]) && userPw == selected[3])
            {
                forSession = new string[3] { selected[0], selected[4], selected[5] };
                return forSession;
            }
            else
            {
                return forSession = new string[] { };
            }
        }

        string[] SelectAkun(string userContact, string pw)
        {
            string query =
                "SELECT Ak.Id, Ak.PegawaiEmail, Ak.PegawaiNoHp, Ak.Pw, Pe.Id, Pe.Nama " +
                "FROM Akun Ak JOIN Pegawai Pe ON Ak.PegawaiEmail = Pe.Email " +
                "WHERE (PegawaiEmail = '" + userContact + "' OR PegawaiNoHp = '" + userContact + "') " + "AND Pw = '" + pw + "'";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    List<string> dataAkun;
                    if (sqlDataReader.HasRows)
                    {
                        dataAkun = new List<string>();
                        while (sqlDataReader.Read())
                        {
                            dataAkun.Add(Convert.ToString(sqlDataReader[0]));
                            dataAkun.Add(Convert.ToString(sqlDataReader[1]));
                            dataAkun.Add(Convert.ToString(sqlDataReader[2]));
                            dataAkun.Add(Convert.ToString(sqlDataReader[3]));
                            dataAkun.Add(Convert.ToString(sqlDataReader[4]));
                            dataAkun.Add(Convert.ToString(sqlDataReader[5]));
                        }

                        return dataAkun.ToArray();
                    }
                    else
                    {
                        Console.WriteLine("Has No Rows");
                        dataAkun = new List<string>();
                        return dataAkun.ToArray();
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return new string[] { };
            }
        }

        void InsertPegawai(Pegawai pegawai)
        {
            using (SqlConnection sqlConnection =
                new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();

                using (SqlCommand pegawaiCommand = sqlConnection.CreateCommand())
                {
                    pegawaiCommand.Transaction = sqlTransaction;

                    pegawaiCommand.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@pegawaiid", pegawai.Id),
                        new SqlParameter("@nama", pegawai.Nama),
                        new SqlParameter("@tmptlahir", pegawai.TmptLahir),
                        new SqlParameter("@tgllahir", pegawai.TglLahir),
                        new SqlParameter("@gender", pegawai.Gender),
                        new SqlParameter("@agama", pegawai.Agama),
                        new SqlParameter("@alamat", pegawai.Alamat),
                        new SqlParameter("@email", pegawai.Email),
                        new SqlParameter("@nohp", pegawai.NoHp),
                        new SqlParameter("@foto", pegawai.Foto),
                        new SqlParameter("@posisiid", pegawai.PosisiId),
                        new SqlParameter("@kantorid", pegawai.KantorId)
                    });

                    try
                    {
                        pegawaiCommand.CommandText = "INSERT INTO Pegawai " +
                            "VALUES (@pegawaiid, @nama, @tmptlahir, @tgllahir, @gender, @agama, @alamat, @email, @nohp, @foto, @posisiid, @kantorid)";
                        pegawaiCommand.ExecuteNonQuery();
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                    }
                }
            }
        }

        void InsertAkun(Akun akun)
        {
            using (SqlConnection sqlConnection =
                new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();

                using (SqlCommand akunCommand = sqlConnection.CreateCommand())
                {
                    akunCommand.Transaction = sqlTransaction;

                    akunCommand.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@akunid", Convert.ToInt32(akun.Id)),
                        new SqlParameter("@email", akun.PegawaiEmail),
                        new SqlParameter("@nohp", akun.PegawaiNoHp),
                        new SqlParameter("@pw", akun.Pw)
                    });

                    try
                    {
                        akunCommand.CommandText = "INSERT INTO Akun VALUES (@akunid, @email, @nohp, @pw)";
                        akunCommand.ExecuteNonQuery();
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                    }
                }
            }
        }

        string PegawaiIdGenerator()
        {
            string pegawaiId = program.SelectMaxPegawaiId();

            int pegawaiNumber = Convert.ToInt32(pegawaiId.Substring(pegawaiId.Length - 6));
            pegawaiNumber++;
            int pegawaiNumLen = Convert.ToString(pegawaiNumber).Length;
            int multiplier = 6 - pegawaiNumLen;
            string newPegawaiId = "161GW" + new string('0', multiplier) + Convert.ToString(pegawaiNumber);
            return newPegawaiId;
        }

        int AkunIdGenerator()
        {
            int akunId = program.SelectMaxAkunId();
            int newAkunId = akunId++;
            return newAkunId;
        }

        int SelectMaxAkunId()
        {
            string query = "SELECT MAX(Id) FROM Akun";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    int akunId = 0;
                    if (sqlDataReader.HasRows)
                    {

                        while (sqlDataReader.Read())
                        {
                            akunId = Convert.ToInt32(sqlDataReader[0]);
                        }
                        return akunId;
                    }
                    else
                    {
                        Console.WriteLine("Has No Rows");
                        return akunId;
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return 0;
            }
        }

        string SelectMaxPegawaiId()
        {
            string query = "SELECT MAX(Id) FROM Pegawai";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    string pegawaiId = "NULL";
                    if (sqlDataReader.HasRows)
                    {

                        while (sqlDataReader.Read())
                        {
                            pegawaiId = Convert.ToString(sqlDataReader[0]);
                        }
                        return pegawaiId;
                    }
                    else
                    {
                        Console.WriteLine("Has No Rows");
                        return pegawaiId;
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return "Exception";
            }
        }

        void SelectAllPegawai()
        {
            string query = "SELECT Pe.Id, Pe.Nama, Pe.TmptLahir, Pe.Gender, Pe.Agama, Pe.Alamat, Pe.Email, Pe.NoHp, Pe.Foto, Po.Nama 'PosisiNama', Ka.Alamat 'KantorAlamat' " +
                           "FROM Pegawai Pe " +
                           "JOIN Posisi Po ON Pe.PosisiId = Po.Id " +
                           "JOIN Kantor Ka ON Pe.KantorId = Ka.Id";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {

                        Console.WriteLine("Id - Nama - Tempat Lahir - " +
                            "Tanggal Lahir - Gender - Agama - Alamat - " +
                            "Email - No. Hp - Foto - Posisi - Kantor");
                        while (sqlDataReader.Read())
                        {
                            Console.WriteLine(
                                sqlDataReader[0] +
                                " - " + sqlDataReader[1] +
                                " - " + sqlDataReader[2] +
                                " - " + sqlDataReader[3] +
                                " - " + sqlDataReader[4] +
                                " - " + sqlDataReader[5] +
                                " - " + sqlDataReader[6] +
                                " - " + sqlDataReader[7] +
                                " - " + sqlDataReader[8] +
                                " - " + sqlDataReader[9] +
                                " - " + sqlDataReader[10]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Has No Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        void Update(string newPassword, int id)
        {
            using (SqlConnection sqlConnection =
                new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                sqlCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@pw", newPassword)
                });

                try
                {
                    sqlCommand.CommandText = "UPDATE Akun SET " +
                        "Pw = @pw " +
                        "WHERE Id = @id";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
        }

        void Delete(string pegawaiId, int akunId)
        {
            using (SqlConnection sqlConnection =
                new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                try
                {
                    sqlCommand.CommandText = "DELETE FROM Pegawai " +
                        "WHERE Id = " + pegawaiId;
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();

                    sqlCommand.CommandText = "DELETE FROM Akun " +
                        "WHERE Id = " + akunId;
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
        }
    }

    class Pegawai
    {
        public string Id { get; set; }
        public string Nama { get; set; }
        public string TmptLahir { get; set; }
        public string TglLahir { get; set; }
        public char Gender { get; set; }
        public string Agama { get; set; }
        public string Alamat { get; set; }
        public string Email { get; set; }
        public string NoHp { get; set; }
        public string Foto { get; set; }
        public int PosisiId { get; set; }
        public int KantorId { get; set; }
    }

    class Akun
    {
        public int Id { get; set; }
        public string PegawaiEmail { get; set; }
        public string PegawaiNoHp { get; set; }
        public string Pw { get; set; }
    }
}
