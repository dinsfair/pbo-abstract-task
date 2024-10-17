using System;
using System.Collections.Generic;

public interface IKemampuan
{
    void Gunakan(Robot robot);
}

public class Perbaikan : IKemampuan
{
    public void Gunakan(Robot robot)
    {
        int energiPulih = new Random().Next(20, 50);
        robot.Energi += energiPulih;
        Console.WriteLine($"{robot.Nama} menggunakan Perbaikan, energi pulih sebesar {energiPulih}. Energi sekarang: {robot.Energi}");
    }
}

public class SeranganListrik : IKemampuan
{
    public void Gunakan(Robot robot)
    {
        int damage = new Random().Next(10, 30);
        robot.Energi -= damage;
        Console.WriteLine($"{robot.Nama} terkena Serangan Listrik, energi berkurang sebesar {damage}. Energi sekarang: {robot.Energi}");
    }
}

public class SeranganGenshin : IKemampuan
{
    public void Gunakan(Robot robot)
    {
        int damage = new Random().Next(30, 70);
        robot.Energi -= damage;
        Console.WriteLine($"{robot.Nama} terkena Serangan Genshin, energi berkurang sebesar {damage}. Energi sekarang: {robot.Energi}");
    }
}

public class PertahananSuper : IKemampuan
{
    public void Gunakan(Robot robot)
    {
        int tambahanArmor = new Random().Next(20, 40);
        robot.Armor += tambahanArmor;
        Console.WriteLine($"{robot.Nama} menggunakan Super Shield, armor bertambah sebesar {tambahanArmor}. Armor sekarang: {robot.Armor}");
    }
}

public abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public abstract void Serang(Robot target);
    public abstract void GunakanKemampuan(IKemampuan kemampuan);

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }
}

public class RobotBiasa : Robot
{
    public RobotBiasa(string nama) : base(nama, 100, 30, 40) { }

    public override void Serang(Robot target)
    {
        int damage = Serangan - target.Armor;
        if (damage < 0) damage = 0;
        target.Energi -= damage;
        Console.WriteLine($"{Nama} menyerang {target.Nama}, menyebabkan damage {damage}. Energi {target.Nama} tersisa: {target.Energi}");
    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        kemampuan.Gunakan(this);
    }
}

public class BosRobot : Robot
{
    public int PertahananExtra { get; set; }

    public BosRobot(string nama) : base(nama, 300, 70, 80)
    {
        PertahananExtra = 20;
    }

    public override void Serang(Robot target)
    {
        int damage = Serangan - (target.Armor + PertahananExtra);
        if (damage < 0) damage = 0;
        target.Energi -= damage;
        Console.WriteLine($"{Nama} menyerang {target.Nama}, menyebabkan damage {damage}. Energi {target.Nama} tersisa: {target.Energi}");
    }

    public void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan - PertahananExtra;
        if (damage < 0) damage = 0;
        Energi -= damage;
        Console.WriteLine($"{Nama} diserang oleh {penyerang.Nama}, menerima damage {damage}. Energi sekarang: {Energi}");
        if (Energi <= 0)
        {
            Mati();
        }
    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        kemampuan.Gunakan(this);
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah mati!");
    }
}

public class SimulatorPertarungan
{
    public static void Main(string[] args)
    {

        Robot robot1 = new RobotBiasa("Robot A");
        Robot robot2 = new RobotBiasa("Robot B");
        BosRobot bosRobot = new BosRobot("Bos Robot X");

        IKemampuan perbaikan = new Perbaikan();
        IKemampuan seranganListrik = new SeranganListrik();
        IKemampuan seranganGenshin = new SeranganGenshin();
        IKemampuan pertahananSuper = new PertahananSuper();

        Console.WriteLine("\n--- Informasi Robot ---");
        robot1.CetakInformasi();
        robot2.CetakInformasi();
        bosRobot.CetakInformasi();

        Console.WriteLine("\n--- Pertarungan Dimulai ---\n");

        while (bosRobot.Energi > 0 && (robot1.Energi > 0 || robot2.Energi > 0))
        {
            Console.WriteLine("Pilih aksi untuk Robot A:");
            Console.WriteLine("1. Serang Bos Robot");
            Console.WriteLine("2. Gunakan Kemampuan (Perbaikan)");
            int pilihanA = Convert.ToInt32(Console.ReadLine());

            if (pilihanA == 1)
            {
                robot1.Serang(bosRobot);
            }
            else if (pilihanA == 2)
            {
                robot1.GunakanKemampuan(perbaikan);
            }

            Console.WriteLine("Pilih aksi untuk Robot B:");
            Console.WriteLine("1. Serang Bos Robot");
            Console.WriteLine("2. Gunakan Kemampuan (Serangan Genshin)");
            int pilihanB = Convert.ToInt32(Console.ReadLine());

            if (pilihanB == 1)
            {
                robot2.Serang(bosRobot);
            }
            else if (pilihanB == 2)
            {
                robot2.GunakanKemampuan(seranganGenshin);
            }

            Random rand = new Random();
            int target = rand.Next(1, 3);
            if (target == 1 && robot1.Energi > 0)
            {
                bosRobot.Serang(robot1);
            }
            else if (target == 2 && robot2.Energi > 0)
            {
                bosRobot.Serang(robot2);
            }

            if (bosRobot.Energi <= 0)
            {
                bosRobot.Mati();
                break;
            }
            if (robot1.Energi <= 0 && robot2.Energi <= 0)
            {
                Console.WriteLine("Semua robot mati! Bos Robot menang!");
                break;
            }

            Console.WriteLine("\n--- Status Setelah Giliran ---");
            robot1.CetakInformasi();
            robot2.CetakInformasi();
            bosRobot.CetakInformasi();
            Console.WriteLine("\n");
        }
    }
}

//Class : Robot, RobotBiasa, dan BosRobot
//           Perbaikan, SeranganListrik, Serangan Genshin, PertahananSuper
//           SimulatorPertarungan
//Subclass : RobotBiasa dan BosRobot merupakan subclass dari Robot
//Inheritance : RobotBiasa dan BosRobot mewarisi Nama, Energi, Armor, dan Serangan
//Dan juga Serang, GunakanKemampuan, CetakInformasi
//Override : RobotBiasa dan BosRobot melakukan override terhadap metode Serang() dan GunakanKemampuan() dari abstract class Robot.
//           Pada class RobotBiasa, metode Serang() menghitung damage dengan mengurangi armor target dari nilai serangan.
//           Pada class BosRobot, metode Serang() memperhitungkan PertahananExtra yang dimiliki oleh Bos.
//Method : Serang() untuk menyerang robot lain
//         GunakanKemampuan() untuk menggunakan kemampuan tertentu terhadap robot
//         CetakInformasi() menampilkan informasi tentang robot dari nama,energi,armor dan serangan