using System;
using System.Collections.Generic;

namespace SistemMBG
{
    // 1. Class Sekolah
    public class Sekolah
    {
        public string IdSekolah { get; set; }
        public string NamaSekolah { get; set; }
        public int JumlahPaketDiterima { get; set; }

        public Sekolah(string idSekolah, string namaSekolah, int jumlahPaketDiterima)
        {
            IdSekolah = idSekolah;
            NamaSekolah = namaSekolah;
            JumlahPaketDiterima = jumlahPaketDiterima;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"ID Sekolah           : {IdSekolah}");
            Console.WriteLine($"Nama Sekolah         : {NamaSekolah}");
            Console.WriteLine($"Jumlah Paket Diterima: {JumlahPaketDiterima}");
            Console.WriteLine("----------------------------------------");
        }
    }

    // 2. Abstract Class PaketMBG & Encapsulation
    public abstract class PaketMBG
    {
        private string idPaket;
        private string namaPaket;
        private int stokDapur;

        public string IdPaket
        {
            get { return idPaket; }
            set { idPaket = value; }
        }

        public string NamaPaket
        {
            get { return namaPaket; }
            set { namaPaket = value; }
        }

        public int StokDapur
        {
            get { return stokDapur; }
            set
            {
                if (value >= 0)
                {
                    stokDapur = value;
                }
                else
                {
                    Console.WriteLine("Peringatan: Nilai stok dapur tidak boleh bernilai negatif!");
                }
            }
        }

        protected PaketMBG(string idPaket, string namaPaket, int stokDapur)
        {
            IdPaket = idPaket;
            NamaPaket = namaPaket;
            StokDapur = stokDapur;
        }

        public void KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= stokDapur)
            {
                stokDapur -= jumlah;
            }
            else
            {
                Console.WriteLine("Gagal mengurangi stok: Jumlah tidak valid atau melebihi stok yang tersedia.");
            }
        }

        public abstract int HitungBiaya(int jumlah);
    }

    // 3 & 4. Inheritance & Polymorphism
    public class PaketReguler : PaketMBG
    {
        public PaketReguler(string idPaket, string namaPaket, int stokDapur)
            : base(idPaket, namaPaket, stokDapur) { }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 15000;
        }
    }

    public class PaketKhusus : PaketMBG
    {
        public string JenisDiet { get; set; }

        public PaketKhusus(string idPaket, string namaPaket, int stokDapur, string jenisDiet)
            : base(idPaket, namaPaket, stokDapur)
        {
            JenisDiet = jenisDiet;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }

    // 6. Interface & Distribusi
    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    public class DistribusiMBG : DapatDidistribusikan
    {
        public Sekolah SekolahTujuan { get; set; }
        public PaketMBG PaketMakanan { get; set; }
        public int JumlahKirim { get; set; }

        public DistribusiMBG(Sekolah sekolahTujuan, PaketMBG paketMakanan, int jumlahKirim)
        {
            SekolahTujuan = sekolahTujuan;
            PaketMakanan = paketMakanan;
            JumlahKirim = jumlahKirim;
        }

        public void KirimPaket()
        {
            if (JumlahKirim <= 0)
            {
                Console.WriteLine($"[Gagal] Pengiriman ke {SekolahTujuan.NamaSekolah}: Jumlah paket harus lebih dari 0.");
                return;
            }

            if (JumlahKirim > PaketMakanan.StokDapur)
            {
                Console.WriteLine($"[Gagal] Pengiriman ke {SekolahTujuan.NamaSekolah}: Melebihi stok dapur.");
                return;
            }

            PaketMakanan.KurangiStok(JumlahKirim);
            SekolahTujuan.JumlahPaketDiterima += JumlahKirim;
            Console.WriteLine($"Distribusi MBG berhasil! {JumlahKirim} paket dikirim ke {SekolahTujuan.NamaSekolah}.");
        }
    }

    // 7. PengelolaMBG (Aggregation)
    public class PengelolaMBG
    {
        private List<Sekolah> daftarSekolah;
        private List<PaketMBG> daftarPaket;

        public PengelolaMBG()
        {
            daftarSekolah = new List<Sekolah>();
            daftarPaket = new List<PaketMBG>();
        }

        public void TambahSekolah(Sekolah sekolah)
        {
            daftarSekolah.Add(sekolah);
        }

        public void TambahPaket(PaketMBG paket)
        {
            daftarPaket.Add(paket);
        }

        public void TampilkanDataMBG()
        {
            Console.WriteLine("=== DATA SEKOLAH PENERIMA PROGRAM MBG ===");
            foreach (var sekolah in daftarSekolah)
            {
                sekolah.TampilInfo();
            }

            Console.WriteLine("\n=== DATA PERSEDIAAN PAKET DI DAPUR ===");
            foreach (var paket in daftarPaket)
            {
                Console.WriteLine($"ID Paket    : {paket.IdPaket}");
                Console.WriteLine($"Nama Paket  : {paket.NamaPaket}");
                Console.WriteLine($"Stok Dapur  : {paket.StokDapur}");
                Console.WriteLine("----------------------------------------");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            Sekolah sekolah1 = new Sekolah("SCH001", "SDN Merdeka 01", 50);
            Sekolah sekolah2 = new Sekolah("SCH002", "SMPN Tunas Bangsa", 30);

            sekolah1.TampilInfo();
            sekolah2.TampilInfo();

            PaketMBG paket1 = new PaketReguler("PKG01", "Paket Reguler Standar", 200);
            PaketMBG paket2 = new PaketKhusus("PKG02", "Paket Diet Khusus", 100, "Rendah Gula");

            int jumlahPesan = 5;
            Console.WriteLine($"Biaya {jumlahPesan} {paket1.NamaPaket}: Rp {paket1.HitungBiaya(jumlahPesan)}");
            Console.WriteLine($"Biaya {jumlahPesan} {paket2.NamaPaket}: Rp {paket2.HitungBiaya(jumlahPesan)}");

            DistribusiMBG distribusi1 = new DistribusiMBG(sekolah1, paket1, 25);
            distribusi1.KirimPaket();

            PengelolaMBG pengelola = new PengelolaMBG();
            pengelola.TambahSekolah(sekolah1);
            pengelola.TambahSekolah(sekolah2);
            pengelola.TambahPaket(paket1);
            pengelola.TambahPaket(paket2);

            pengelola.TampilkanDataMBG();

            Console.ReadKey();
        }
    }
}