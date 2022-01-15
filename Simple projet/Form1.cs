using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib; 
using System.Threading;
using CoD_Public_Cheater;

namespace Simple_projet
{
    public partial class Form1 : Form
    {
        static PS3API PS3 = new PS3API();
        /*
            SpecialOps.Mods.GiveWeapon(1, 0, new byte[] { 0x00, 0x00, 0x01 });
            SpecialOps.Mods.GiveWeapon(2, 0, new byte[] { 0x00, 0x00, 0x01 });
            SpecialOps.Mods.GiveWeapon(3, 0, new byte[] { 0x00, 0x00, 0x01 });
         */
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            PS3.ChangeAPI(SelectAPI.TargetManager);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            PS3.ChangeAPI(SelectAPI.ControlConsole); //API Changé pour CCAPI
        }
        public static class SpecialOps
        {
            public static UInt32 Index = 0xB0C8;
            public static class Offsets
            {
                public static class Addresses
                {
                    public static UInt32
                       Sv_Maprestart = 0x2D0788,
                       G_Client = 0x123D468,
                       G_Entity = 0x10EEAD8,
                       EntitySize = 0x270,
                       Add_Ammo = 0x145B34,
                       G_GivePlayerWeapon = 0x18E3BC,
                       Recoil = 0x6D798, //Off - 48 25 15 D1
                       SteadyAim = 0x2BF4D4, //2C 03 00 00 - On , 2C 03 00 02 - Off
                       Full_Auto = 0x2B0E84,// 39, 60, 00, 00 - On , 3B 40 00 01 - Off
                       Laser = 0x67EBC, //2C 14 00 00 - Off , 2C 14 00 01 - On
                       CBuf_AddText = 0x201488,
                       Dvar_GetBool = 0x279F18,
                       Va = 0x281470,
                       Sv_GameSendServerCommand = 0x2D94C8,
                       R_SetFrameFog = 0x386200;
                }

            }
            public static class Mods
            {
                #region mFlag
                public static void mFlag(UInt32 Client, string Type)
                {
                    if (Type == "Freeze")
                        Lib.SetMemory(0x012320df + (Client * 0xB0C8), new Byte[] { 0x04 });
                    else if (Type == "NoClip")
                        Lib.SetMemory(0x012320df + (Client * 0xB0C8), new Byte[] { 0x02 });
                    else if (Type == "Ufo")
                        Lib.SetMemory(0x012320df + (Client * 0xB0C8), new Byte[] { 0x01 });
                    else if (Type == "Normal")
                        Lib.SetMemory(0x012320df + (Client * 0xB0C8), new Byte[] { 0x00 });
                }
                #endregion
                #region AllPerks
                public static void AllPerks(UInt32 Client)
                {
                    Lib.SetMemory(0x01227788 + (Client * 0xB0C8), new Byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                }
                #endregion
                #region God Mode
                public static void GodMode(UInt32 Client)
                {
                    Lib.SetMemory(0x012272ea + (Client * 0xB0C8), new Byte[] { 0x00, 0x15 });
                }
                #endregion
                public class Offsets
                {
                    public static UInt32
                       Entry = 0x01227609,
                       Primary_1 = 0x0122762d,
                       Primary_2 = 0x012276a5,
                       AmmoPrim_1 = Primary_1 + 0x3,
                       AmmoPrim_2 = Primary_2 + 0x3,
                       AmmoPrim_3 = Primary_2 + 0x7,
                       Secondary_1 = 0x01227645,
                       Secondary_2 = 0x012276c9,
                       AmmoSeco_1 = Secondary_1 + 0x3,
                       AmmoSeco_2 = Secondary_1 + 0x3,
                       AmmoSeco_3 = Secondary_2 + 0x7;
                }
                public enum Bullets
                {
                    BigMissiles = 0x02,
                    CarePackage_Marker = 0x12,
                    Mig27_Rockets = 0x10,
                    Mig27_Bullets = 0x0F,
                    StingerBullets = 0x16,
                    Airsupport_Marker = 0x46,
                    Mortar = 0x4C,
                    PredatorMissile = 0x15
                }
                public static void BulletType(Int32 Client, Bullets BulletType)
                {
                    /*
                    Byte Bullet = (Byte)BulletType;
                    GiveWeapon(7, Client, new byte[] { 0 });
                    GiveWeapon(8, Client, new byte[] { 0 });
                    GiveWeapon(9, Client, new byte[] { 0 });
                    Lib.Sleep(50);
                    GiveWeapon(7, Client, new byte[] { Bullet });
                    GiveWeapon(8, Client, new byte[] { Bullet });
                    GiveWeapon(9, Client, new byte[] { Bullet });
                    */
                    Byte Bullet = (Byte)BulletType;
                    UInt32 clientIndex = (UInt32)Client * 0xB0C8;
                    UInt32 Ammo_Address_1 = 0x012276a8;
                    UInt32 Ammo_Address_2 = 0x01227630;
                    UInt32 Ammo_Address_3 = 0x012276cc;
                    UInt32 Ammo_Address_4 = 0x01227648;
                    Byte[] Ammo = new Byte[] { 0x0F, 255, 255, 255 };
                    Lib.WriteBytes(0x12276CB - 2 + clientIndex, new Byte[] { 0, 0, 0 });
                    Lib.WriteBytes(0x0122760B - 2 + clientIndex, new Byte[] { 0, 0, 0 });
                    Lib.WriteBytes(0x0122751B - 2 + clientIndex, new Byte[] { 0, 0, 0 });
                    Lib.WriteBytes(0x0122750F - 2 + clientIndex, new Byte[] { 0, 0, 0 });
                    Lib.WriteBytes(0x0122762F - 2 + clientIndex, new Byte[] { 0, 0, 0 });
                    Lib.WriteBytes(0x012276a7 - 2 + clientIndex, new Byte[] { 0, 0, 0 });
                    Lib.Sleep(50);
                    Lib.WriteBytes(0x12276CB + clientIndex, new Byte[] { Bullet });
                    Lib.WriteBytes(0x0122760B + clientIndex, new Byte[] { Bullet });
                    Lib.WriteBytes(0x0122751B + clientIndex, new Byte[] { Bullet });
                    Lib.WriteBytes(0x0122750F + clientIndex, new Byte[] { Bullet });
                    Lib.WriteBytes(0x0122762F + clientIndex, new Byte[] { Bullet });
                    Lib.WriteBytes(0x012276a7 + clientIndex, new Byte[] { Bullet });
                    Lib.SetMemory(Ammo_Address_1 + clientIndex, Ammo);
                    Lib.SetMemory(Ammo_Address_2 + clientIndex, Ammo);
                    Lib.SetMemory(Ammo_Address_3 + clientIndex, Ammo);
                    Lib.SetMemory(Ammo_Address_4 + clientIndex, Ammo);
                }
                public static void GiveWeapon(Int32 WeaponSetUp, Int32 Client, Byte[] Weapon)
                {
                    UInt32 Entry_1 = 0x0122751d;//+4 = Next Weapon
                    UInt32 Entry_2 = 0x0122764d; //+8 = Next Weapon
                    UInt32 Entry_3 = 0x012276d5; //+0xC = Next Weapon
                    UInt32 Ammo_1 = Entry_2 + 0x3; //+8 = Next Weapon
                    UInt32 Ammo_2 = Entry_3 + 0x3; //+0xC = Next Weapon
                    UInt32 Ammo_3 = Entry_3 + 0x7; //+0xC = Next Weapon
                    UInt32 clientIndex = (UInt32)Client * 0xB0C8;
                    UInt32 WeaponUI = (UInt32)WeaponSetUp;
                    Byte[] Ammo = new Byte[] { 0x0F, 255, 255, 255 };
                    if (WeaponSetUp == 0)
                    { }
                    if (WeaponSetUp == 1)
                    {
                        Lib.SetMemory(Offsets.Entry + clientIndex, Weapon);
                        Lib.SetMemory(Offsets.Primary_1 + clientIndex, Weapon);
                        Lib.SetMemory(Offsets.Primary_2 + clientIndex, Weapon);
                        Lib.SetMemory(Offsets.AmmoPrim_1 + clientIndex, Ammo);
                        Lib.SetMemory(Offsets.AmmoPrim_2 + clientIndex, Ammo);
                        Lib.SetMemory(Offsets.AmmoPrim_3 + clientIndex, Ammo);
                    }
                    else if (WeaponSetUp == 2)
                    {
                        Lib.SetMemory(Offsets.Entry + clientIndex, Weapon);
                        Lib.SetMemory(Offsets.Secondary_1 + clientIndex, Weapon);
                        Lib.SetMemory(Offsets.Secondary_2 + clientIndex, Weapon);
                        Lib.SetMemory(Offsets.AmmoSeco_1 + clientIndex, Ammo);
                        Lib.SetMemory(Offsets.AmmoSeco_2 + clientIndex, Ammo);
                        Lib.SetMemory(Offsets.AmmoSeco_3 + clientIndex, Ammo);
                    }
                    else if (WeaponSetUp == 3)
                    {
                        Lib.SetMemory(Entry_1 + clientIndex, Weapon);
                        Lib.SetMemory(Entry_2 + clientIndex, Weapon);
                        Lib.SetMemory(Entry_3 + clientIndex, Weapon);
                        Lib.SetMemory(Ammo_1 + clientIndex, Ammo);
                        Lib.SetMemory(Ammo_2 + clientIndex, Ammo);
                        Lib.SetMemory(Ammo_3 + clientIndex, Ammo);
                    }
                    if (WeaponSetUp >= 3)
                    {
                        Lib.SetMemory(Entry_1 + (WeaponUI - 3) * 4 + (clientIndex), Weapon);
                        Lib.SetMemory(Entry_2 + (WeaponUI - 3) * 8 + (clientIndex), Weapon);
                        Lib.SetMemory(Entry_3 + (WeaponUI - 3) * 12 + (clientIndex), Weapon);
                        Lib.SetMemory(Ammo_1 + (WeaponUI - 3) * 8 + (clientIndex), Ammo);
                        Lib.SetMemory(Ammo_2 + (WeaponUI - 3) * 12 + (clientIndex), Ammo);
                        Lib.SetMemory(Ammo_3 + (WeaponUI - 3) * 12 + (clientIndex), Ammo);
                    }
                }
            }
            public static class GiveWeapon
            {
                public static class Functions
                {
                    private static UInt32 WeaponDrop = Offsets.Primary.Weapon1;
                    #region SaveWapon
                    public static void SaveWeapon(Int32 ClientInt)
                    {
                        SaveWeapon1(ClientInt);
                    }
                    private static Byte[] lalal112;
                    private static void SaveWeapon1(Int32 ClientInt)
                    {
                        lalal112 = Lib.ReadBytes(WeaponDrop + ((UInt32)ClientInt * 0x3980), 572);
                    }
                    #endregion
                    #region Load Weapon
                    public static void Load_Weapon(Int32 ClientInt)
                    {

                    }
                    #endregion
                }
                public static class Offsets
                {
                    #region Primary
                    public static class Primary /*1*/
                    {
                        public static UInt32
                            Weapon1 = 0x01227609,
                            Weapon2 = 0x0122762d,
                            Weapon3 = 0x012276a5,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region Secondary
                    public static class Secondary /*2*/
                    {
                        public static UInt32
                            Weapon1 = 0x01227609,
                            Weapon2 = 0x01227645,
                            Weapon3 = 0x012276c9,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region 3
                    public static class DPAD_UP /*3*/
                    {
                        public static UInt32
                            Weapon1 = 0x0122751d, //+4 = Next Weapon
                            Weapon2 = 0x0122764d, //+8 = Next Weapon
                            Weapon3 = 0x012276d5, //+0xC = Next Weapon
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region 4
                    public static class DPAD_Down /*4*/
                    {
                        public static UInt32
                            Weapon1 = 0x01227521,
                            Weapon2 = 0x01227655,
                            Weapon3 = 0x012276e1,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region 5
                    public static class Weapon_5
                    {
                        public static UInt32
                            Weapon1 = DPAD_Down.Weapon1 + 0x4,
                            Weapon2 = DPAD_Down.Weapon2 + 0x8,
                            Weapon3 = DPAD_Down.Weapon3 + 0xC,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region 6
                    public static class Weapon_6
                    {
                        public static UInt32
                            Weapon1 = Weapon_5.Weapon1 + 0x4,
                            Weapon2 = Weapon_5.Weapon2 + 0x8,
                            Weapon3 = Weapon_5.Weapon3 + 0xC,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region 7
                    public static class Weapon_7
                    {
                        public static UInt32
                            Weapon1 = Weapon_6.Weapon1 + 0x4,
                            Weapon2 = Weapon_6.Weapon2 + 0x8,
                            Weapon3 = Weapon_6.Weapon3 + 0xC,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region 8
                    public static class Weapon_8
                    {
                        public static UInt32
                            Weapon1 = Weapon_7.Weapon1 + 0x4,
                            Weapon2 = Weapon_7.Weapon2 + 0x8,
                            Weapon3 = Weapon_7.Weapon3 + 0xC,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region 9
                    public static class Weapon_9
                    {
                        public static UInt32
                            Weapon1 = Weapon_8.Weapon1 + 0x4,
                            Weapon2 = Weapon_8.Weapon2 + 0x8,
                            Weapon3 = Weapon_8.Weapon3 + 0xC,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region 10
                    public static class Weapon_10
                    {
                        public static UInt32
                            Weapon1 = Weapon_9.Weapon1 + 0x4,
                            Weapon2 = Weapon_9.Weapon2 + 0x8,
                            Weapon3 = Weapon_9.Weapon3 + 0xC,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region Lethal
                    public static class Lethal
                    {
                        public static UInt32
                            Weapon1 = 0x01227511,
                            Weapon2 = 0x012275fd,
                            Weapon3 = 0x012276b1,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                    #region Tactical
                    public static class Tactical
                    {
                        public static UInt32
                            Weapon1 = 0x01227515,
                            Weapon2 = 0x0122763d,
                            Weapon3 = 0x012276bd,
                            AmmoClip = Weapon2 + 0x3,
                            AmmoBullets = Weapon3 + 0x3,
                            AkimboAmmo = Weapon3 + 0x7;
                    }
                    #endregion
                }
                public static class List
                {
                    #region Strings
                    public static class strings
                    {
                        public static String
                            //4D - FF  = Freeze!
                        Mortar = "0x00,0x00,0x4C",
                        M1014 /*Secret Weapon*/ = "0x00,0x00,0x4A",
                        Claymore_Fake = "0x00,0x00,0x49",
                        Flash_Granade = "0x00,0x00,0x48",
                        Smoke_Granade = "0x00,0x00,0x47",
                        Airsupport_Marker = "0x00,0x00,0x46",
                        RPG_7 = "0x00,0x00,0x45",
                        Claymore = "0x00,0x00,0x44",
                        Barret_50Cal = "0x00,0x00,0x43",
                        AS50 = "0x00,0x00,0x42",
                        L118A = "0x00,0x00,0x41",
                        RSASS = "0x00,0x00,0x40",
                        Dragunov = "0x00,0x00,0x3F",
                        MSR = "0x00,0x00,0x3E",
                        AA12 = "0x00,0x00,0x3D",
                        Striker = "0x00,0x00,0x3C",
                        KSG_12 = "0x00,0x00,0x3B",
                        SPAS_12 = "0x00,0x00,0x3A",
                        USAS_12 = "0x00,0x00,0x39",
                        Model1887 = "0x00,0x00,0x38",
                        Type95 = "0x00,0x00,0x37",
                        MK14 = "0x00,0x00,0x36",
                        CM901 = "0x00,0x00,0x35",
                        G36c = "0x00,0x00,0x34",
                        FAD = "0x00,0x00,0x33",
                        AK47 = "0x00,0x00,0x32",
                        ACR = "0x00,0x00,0x31",
                        M16A4 = "0x00,0x00,0x2F",
                        M4A1 = "0x00,0x00,0x2E",
                        MG36 = "0x00,0x00,0x2D",
                        L68LSW = "0x00,0x00,0x2C",
                        MK46 = "0x00,0x00,0x2B",
                        PKP_Pecheneg = "0x00,0x00,0x2A",
                        M60E4 = "0x00,0x00,0x29",
                        P90 = "0x00,0x00,0x28",
                        PP90M1 = "0x00,0x00,0x27",
                        PM_9 = "0x00,0x00,0x26",
                        MP7 = "0x00,0x00,0x25",
                        UMP45 = "0x00,0x00,0x24",
                        MP5 = "0x00,0x00,0x23",
                        FMG9 = "0x00,0x00,0x22",
                        MP9 = "0x00,0x00,0x21",
                        Skorpion = "0x00,0x00,0x20",
                        G18 = "0x00,0x00,0x1F",
                        P99 = "0x00,0x00,0x1E",
                        Magnum44 = "0x00,0x00,0x1D",
                        DesertEagle = "0x00,0x00,0x1C",
                        MP412 = "0x00,0x00,0x1B",
                        USP45 = "0x00,0x00,0x1A",
                        FiveSeven = "0x00,0x00,0x19",
                        WiredWeapon_1/*(Freeze When Shoot)*/ = "0x00,0x00,0x18",
                        WiredWeapon_2/*(Freeze When Shoot)*/ = "0x00,0x00,0x17",
                        WiredWeapon_3/*(Freeze When Shoot)*/ = "0x00,0x00,0x11",
                        StingerBullets = "0x00,0x00,0x16",
                        C4 = "0x00,0x00,0x14",
                        Fake_C4 = "0x00,0x00,0x13",
                        CarePackage_Marker = "0x00,0x00,0x12",
                        Mig27_Rockets = "0x00,0x00,0x10",
                        Mig27_Bullets = "0x00,0x00,0x0F",
                        RiotShield = "0x00,0x00,0x07",
                        M9 = "0x00,0x00,0x05",
                        M67_Frag = "0x00,0x00,0x03",
                        BigMissiles /*HOLY FUCK*/= "0x00,0x00,0x02",
                        DefaultWeapon = "0x00,0x00,0x01";
                    }
                    #endregion

                }
                public static String PredatorMissile_Strings = "0x00,0x00,0x15";
            }
            public static class RPC
            {
                #region CallFunction
                private static UInt32 RPCFogAddr = Offsets.Addresses.R_SetFrameFog;
                public static void Enable()
                {
                    Lib.SetMemory(RPCFogAddr, new Byte[] { 0x4E, 0x80, 0x00, 0x20 });
                    Lib.Sleep(20);
                    Byte[] memory = new Byte[]
                    { 0x7C, 0x08, 0x02, 0xA6, 0xF8, 0x01, 0x00, 0x80, 0x3C, 0x60, 0x10, 0x02, 0x81, 0x83, 0x00, 0x4C,
            0x2C, 0x0C, 0x00, 0x00, 0x41, 0x82, 0x00, 0x64, 0x80, 0x83, 0x00, 0x04, 0x80, 0xA3, 0x00, 0x08,
            0x80, 0xC3, 0x00, 0x0C, 0x80, 0xE3, 0x00, 0x10, 0x81, 0x03, 0x00, 0x14, 0x81, 0x23, 0x00, 0x18,
            0x81, 0x43, 0x00, 0x1C, 0x81, 0x63, 0x00, 0x20, 0xC0, 0x23, 0x00, 0x24, 0xc0, 0x43, 0x00, 0x28,
            0xC0, 0x63, 0x00, 0x2C, 0xC0, 0x83, 0x00, 0x30, 0xC0, 0xA3, 0x00, 0x34, 0xc0, 0xC3, 0x00, 0x38,
            0xC0, 0xE3, 0x00, 0x3C, 0xC1, 0x03, 0x00, 0x40, 0xC1, 0x23, 0x00, 0x48, 0x80, 0x63, 0x00, 0x00,
            0x7D, 0x89, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x21, 0x3C, 0x80, 0x10, 0x02, 0x38, 0xA0, 0x00, 0x00,
            0x90, 0xA4, 0x00, 0x4C, 0x90, 0x64, 0x00, 0x50, 0xE8, 0x01, 0x00, 0x80, 0x7C, 0x08, 0x03, 0xA6,
            0x38, 0x21, 0x00, 0x70, 0x4E, 0x80, 0x00, 0x20 };
                    Lib.SetMemory(RPCFogAddr + 4, memory);
                    Lib.SetMemory(0x10020000, new Byte[0x2854]);
                    Lib.SetMemory(RPCFogAddr, new Byte[] { 0xF8, 0x21, 0xFF, 0x91 });
                }

                public static Int32 Call(UInt32 func_address, params object[] parameters)
                {
                    Int32 length = parameters.Length;
                    Int32 index = 0;
                    UInt32 num3 = 0;
                    UInt32 num4 = 0;
                    UInt32 num5 = 0;
                    UInt32 num6 = 0;
                    while (index < length)
                    {
                        if (parameters[index] is Int32)
                        {
                            Lib.WriteInt32(0x10020000 + (num3 * 4), (Int32)parameters[index]);
                            num3++;
                        }
                        else if (parameters[index] is UInt32)
                        {
                            Lib.WriteUInt32(0x10020000 + (num3 * 4), (UInt32)parameters[index]);
                            num3++;
                        }
                        else
                        {
                            UInt32 num7;
                            if (parameters[index] is string)
                            {
                                num7 = 0x10022000 + (num4 * 0x400);
                                Lib.WriteString(num7, Convert.ToString(parameters[index]));
                                Lib.WriteUInt32(0x10020000 + (num3 * 4), num7);
                                num3++;
                                num4++;
                            }
                            else if (parameters[index] is float)
                            {
                                Lib.WriteFloat(0x10020024 + (num5 * 4), (float)parameters[index]);
                                num5++;
                            }
                            else if (parameters[index] is float[])
                            {
                                float[] input = (float[])parameters[index];
                                num7 = 0x10021000 + (num6 * 4);
                                Lib.WriteSingle(num7, input);
                                Lib.WriteUInt32(0x10020000 + (num3 * 4), num7);
                                num3++;
                                num6 += (UInt32)input.Length;
                            }
                        }
                        index++;
                    }
                    Lib.WriteUInt32(0x1002004C, func_address);
                    Lib.Sleep(20);
                    return Lib.ReadInt32(0x10020050);
                }
                #endregion
                #region CBuf_AddText
                public static void CBuf_AddText(Int32 client, string Command)
                {
                    Call(Offsets.Addresses.CBuf_AddText, (UInt32)client, 0, Command, 0, 0);
                    Lib.WriteString(0x2000000, Command);
                    Lib.SetMemory(0x262b28, new Byte[] { 0x38, 0x60, 0, 0, 60, 0x80, 2, 0, 0x4b, 0xf9, 0xe9, 0x59 });
                    Lib.Sleep(15);
                    Lib.SetMemory(0x2000000, new Byte[100]);
                    Lib.SetMemory(0x262b28, new Byte[] { 60, 0x60, 1, 0xda, 0x80, 0x63, 0xe1, 0xcc, 0x88, 0x63, 0, 8 });
                }
                #endregion
                #region SV_GameSendServerCommand
                public static void SV_GameSendServerCommand(Int32 client, string command)
                {
                    Call(Offsets.Addresses.Sv_GameSendServerCommand, (UInt32)client, 0, Lib.DetectButtonsCodes(command));
                    Lib.Sleep(20);
                }
                #endregion
                #region iPrintln
                public static void iPrintln(Int32 client, string Text)
                {

                    SV_GameSendServerCommand(client, "c \"" + Text + "\"");
                }
                #endregion
                #region iPrintlnBold
                public static void iPrintlnBold(Int32 client, string Text)
                {
                    SV_GameSendServerCommand(client, "f \"" + Text + "\"");
                }
                #endregion
                #region Set_ClientDvar
                public static void Set_ClientDvar(Int32 client, string Text)
                {
                    SV_GameSendServerCommand(client, "q " + Text);
                    Lib.Sleep(20);
                }
                #endregion
                #region Key_IsDown
                public static string Key_IsDown(UInt32 ClientNum)
                {
                    UInt32 Key_isDown = 0x01232075;
                    UInt32 Index = 0xB0C8;
                    Byte[] Key = new Byte[3];
                    Lib.GetMemoryR(Key_isDown + (Index * ClientNum), ref Key);
                    string mystring = null;
                    mystring = BitConverter.ToString(Key);
                    string result = mystring.Replace("-", "");
                    string result1 = result.Replace(" ", "");
                    string key = result1;
                    string KeyPressed = "";
                    if (key == "000000")
                    {
                        KeyPressed = "Stand";
                    }
                    else if (key == "080C20")
                    {
                        KeyPressed = "[ ] + X + L1";
                    }
                    else if (key == "000224")
                    {
                        KeyPressed = "Crouch + R3 + [ ]";
                    }
                    else if (key == "008001")
                    {
                        KeyPressed = "R1 + L2";
                    }
                    else if (key == "082802")
                    {
                        KeyPressed = "L1 + L3";
                    }
                    else if (key == "002402")
                    {
                        KeyPressed = "X + L3";
                    }
                    else if (key == "000020")
                    {
                        KeyPressed = "[  ]";
                    }
                    else if (key == "000200")
                    {
                        KeyPressed = "Crouch";
                    }
                    else if (key == "004020")
                    {
                        KeyPressed = "R2 + [ ]";
                    }
                    else if (key == "000220")
                    {
                        KeyPressed = "[ ] + Crouch";
                    }
                    else if (key == "000100")
                    {
                        KeyPressed = "Prone";
                    }
                    else if (key == "400100")
                    {
                        KeyPressed = "Left + Prone";
                    }
                    else if (key == "000400")
                    {
                        KeyPressed = "X";
                    }
                    else if (key == "000004")
                    {
                        KeyPressed = "R3";
                    }
                    else if (key == "002002")
                    {
                        KeyPressed = "L3";
                    }
                    else if (key == "004000")
                    {
                        KeyPressed = "R2";
                    }
                    else if (key == "008000")
                    {
                        KeyPressed = "L2";
                    }
                    else if (key == "080800")
                    {
                        KeyPressed = "L1";
                    }
                    else if (key == "000001")
                    {
                        KeyPressed = "R1";
                    }
                    else if (key == "002006")
                    {
                        KeyPressed = "R3 + L3";
                    }
                    else if (key == "000204")
                    {
                        KeyPressed = "R3";
                    }
                    else if (key == "002202")
                    {
                        KeyPressed = "L3";
                    }
                    else if (key == "004200")
                    {
                        KeyPressed = "R2";
                    }
                    else if (key == "008004")
                    {
                        KeyPressed = "R3 + L2";
                    }
                    else if (key == "008200")
                    {
                        KeyPressed = "L2";
                    }
                    else if (key == "082902")
                    {
                        KeyPressed = "Prone + L1 + L3";
                    }
                    else if (key == "082906")
                    {
                        KeyPressed = "Prone + L1 + L3 + R3";
                    }
                    else if (key == "00C100")
                    {
                        KeyPressed = "Prone + R2 + L2";
                    }
                    else if (key == "00C000")
                    {
                        KeyPressed = "R2 + L2";
                    }
                    else if (key == "002206")
                    {
                        KeyPressed = "Crouch L3 + R3";
                    }
                    else if (key == "002222")
                    {
                        KeyPressed = "Crouch L3 + [ ]";
                    }
                    else if (key == "Up")
                    {
                        KeyPressed = "R2 + L2";
                    }
                    else if (key == "002122")
                    {
                        KeyPressed = "Prone + L3 + [ ]";
                    }
                    else if (key == "000420")
                    {
                        KeyPressed = "X + [ ]";
                    }
                    else if (key == "002106")
                    {
                        KeyPressed = "Prone + R3 + L3";
                    }
                    else
                    {
                        KeyPressed = key;
                    }
                    return KeyPressed;
                }
                public static string GetNames(Int32 clientNum)
                {
                    UInt32 NameInGame = 0x012320bc;
                    UInt32 Index = 0xB0C8;
                    string name;
                    Byte[] name1 = new Byte[18];
                    Lib.GetMemoryR(NameInGame + ((UInt32)clientNum * Index), ref name1);
                    name = Encoding.ASCII.GetString(name1);
                    name.Replace(Convert.ToChar(0x0).ToString(), string.Empty);
                    return name;
                }
                #endregion
                #region DPAD_IsDown
                public static string DPAD_IsDown(UInt32 Client)
                {
                    string key = Lib.ReadString(0x018c47d2 + (Client * 0));
                    string KeyPressed = "";
                    if (key == "16")
                        KeyPressed = "KEINER";
                    else if (key == "15")
                        KeyPressed = "Up";
                    else if (key == "17")
                        KeyPressed = "Down";
                    else if (key == "19")
                        KeyPressed = "Left";
                    else if (key == "21")
                        KeyPressed = "Right";
                    else
                        KeyPressed = key;
                    return KeyPressed;
                }
                #endregion
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        public static void SetDebugLocalString(string Text)
        {
            SpecialOps.RPC.CBuf_AddText(0, "set ui_debug_localVarString " + Text);
        }
        private void flatButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (PS3.ConnectTarget() && PS3.AttachProcess())
                {
                    SpecialOps.RPC.Enable();
                    SetDebugLocalString("^4By ^5M^7r^5N^7i^5a^7t^5o ^1-- ^7www.^5allcodrecovery^7.com ^1- ^2Facebook : ^3Guillaume MrNiato   ");
                    MessageBox.Show("Playstation 3 Linked !", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); //connecté
                }
                else
                {
                    MessageBox.Show("Not Connected !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); //Non connecté
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Not Connected !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); //Non connecté
            }
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GodMode((uint)Players.SelectedIndex);
        }
        public static string get_player_name(uint client)
        {
            string getnames = PS3.Extension.ReadString(0x012320bc + client * 0xB0C8);
            return getnames;
        }
        private void getClientsNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Players.Items.Clear();
            for (uint i = 0; i < 0x12; i++)
            {
                Players.Items.Add(get_player_name(i));
            }
        }

        private void unlimitedAmmoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bigMissilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void formSkin1_Click(object sender, EventArgs e)
        {

        }
        
        private void flatButton2_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x02 });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x02 });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x02 });
        }

        private void flatButton3_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[4];
            PS3.GetMemory(0x01881591, buffer);
            flatNumeric1.Value = BitConverter.ToInt32(buffer, 0);
            PS3.GetMemory(0x01881595, buffer);
            flatNumeric2.Value = BitConverter.ToInt32(buffer, 0);
            PS3.GetMemory(0x01881599, buffer);
            flatNumeric4.Value = BitConverter.ToInt32(buffer, 0);
            PS3.GetMemory(0x018815a5, buffer);
            flatNumeric3.Value = BitConverter.ToInt32(buffer, 0);
            PS3.GetMemory(0x0188159D, buffer);
            flatNumeric5.Value = BitConverter.ToInt32(buffer, 0);
            PS3.GetMemory(0x018815a1, buffer);
            flatNumeric6.Value = BitConverter.ToInt32(buffer, 0);
        }

        private void flatCheckBox1_CheckedChanged(object sender)
        {
            if (flatCheckBox1.Checked)
            {
                PS3.SetMemory(0x018814f1, new byte[] { 0xF4, 0x35, 0x21 });
            }
            else
            {
                PS3.SetMemory(0x018814f1, new byte[] { 0x00, 0x00, 0x00 });
            }
        }

        private void flatButton4_Click(object sender, EventArgs e)
        {
            PS3.SetMemory(0x01881591, BitConverter.GetBytes((int)flatNumeric1.Value));
            PS3.SetMemory(0x01881595, BitConverter.GetBytes((int)flatNumeric2.Value));
            PS3.SetMemory(0x01881599, BitConverter.GetBytes((int)flatNumeric4.Value));
            PS3.SetMemory(0x018815a5, BitConverter.GetBytes((int)flatNumeric3.Value));
            PS3.SetMemory(0x0188159D, BitConverter.GetBytes((int)flatNumeric5.Value));
            PS3.SetMemory(0x018815a1, BitConverter.GetBytes((int)flatNumeric6.Value));
        }

        private void flatButton5_Click(object sender, EventArgs e)
        {
            PS3.Extension.WriteString(0x01827EA4, flatTextBox1.Text);
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x012272ea + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0x00, 0x00 });
        }

        private void unlimitedAmmoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x01227631 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x01227649 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276cd + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276d9 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276b5 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276c1 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276d9 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276e5 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
        }

        private void allPerksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.AllPerks((uint)Players.SelectedIndex);
        }

        private void toggleNoClipToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        public static void GetMemoryR(uint Address, ref byte[] Bytes)
        {
            PS3.GetMemory(Address, Bytes);
        }


        #region Key_IsDown
        public static string Key_IsDown(uint ClientNum)
        {
            uint NameInGame = 0x012320bc;
            uint Key_isDown = 0x01232075;
            uint Index = 0xB0C8;
            byte[] Key = new byte[3];
            GetMemoryR(Key_isDown + (Index * ClientNum), ref Key);
            string mystring = null;
            mystring = BitConverter.ToString(Key);
            string result = mystring.Replace("-", "");
            string result1 = result.Replace(" ", "");
            string key = result1;
            string KeyPressed = "";
            if (key == "000000")
            {
                KeyPressed = "Stand";
            }
            else if (key == "080C20")
            {
                KeyPressed = "[ ] + X + L1";
            }
            else if (key == "000224")
            {
                KeyPressed = "Crouch + R3 + [ ]";
            }
            else if (key == "008001")
            {
                KeyPressed = "R1 + L2";
            }
            else if (key == "082802")
            {
                KeyPressed = "L1 + L3";
            }
            else if (key == "002402")
            {
                KeyPressed = "X + L3";
            }
            else if (key == "000020")
            {
                KeyPressed = "[  ]";
            }
            else if (key == "000200")
            {
                KeyPressed = "Crouch";
            }
            else if (key == "004020")
            {
                KeyPressed = "R2 + [ ]";
            }
            else if (key == "000220")
            {
                KeyPressed = "[ ] + Crouch";
            }
            else if (key == "000100")
            {
                KeyPressed = "Prone";
            }
            else if (key == "400100")
            {
                KeyPressed = "Left + Prone";
            }
            else if (key == "000400")
            {
                KeyPressed = "X";
            }
            else if (key == "000004")
            {
                KeyPressed = "R3";
            }
            else if (key == "002002")
            {
                KeyPressed = "L3";
            }
            else if (key == "004000")
            {
                KeyPressed = "R2";
            }
            else if (key == "008000")
            {
                KeyPressed = "L2";
            }
            else if (key == "080800")
            {
                KeyPressed = "L1";
            }
            else if (key == "000001")
            {
                KeyPressed = "R1";
            }
            else if (key == "002006")
            {
                KeyPressed = "R3 + L3";
            }
            else if (key == "000204")
            {
                KeyPressed = "R3";
            }
            else if (key == "002202")
            {
                KeyPressed = "L3";
            }
            else if (key == "004200")
            {
                KeyPressed = "R2";
            }
            else if (key == "008004")
            {
                KeyPressed = "R3 + L2";
            }
            else if (key == "008200")
            {
                KeyPressed = "L2";
            }
            else if (key == "082902")
            {
                KeyPressed = "Prone + L1 + L3";
            }
            else if (key == "082906")
            {
                KeyPressed = "Prone + L1 + L3 + R3";
            }
            else if (key == "00C100")
            {
                KeyPressed = "Prone + R2 + L2";
            }
            else if (key == "00C000")
            {
                KeyPressed = "R2 + L2";
            }
            else if (key == "002206")
            {
                KeyPressed = "Crouch L3 + R3";
            }
            else if (key == "002222")
            {
                KeyPressed = "Crouch L3 + [ ]";
            }
            else if (key == "Up")
            {
                KeyPressed = "R2 + L2";
            }
            else if (key == "002122")
            {
                KeyPressed = "Prone + L3 + [ ]";
            }
            else if (key == "000420")
            {
                KeyPressed = "X + [ ]";
            }
            else if (key == "002106")
            {
                KeyPressed = "Prone + R3 + L3";
            }
            else
            {
                KeyPressed = key;
            }
            return KeyPressed;
        }
        public static string GetNames(int clientNum)
        {
            uint NameInGame = 0x012320bc;
            uint Index = 0xB0C8;
            string name;
            byte[] name1 = new byte[18];
            GetMemoryR(NameInGame + ((uint)clientNum * Index), ref name1);
            name = Encoding.ASCII.GetString(name1);
            name.Replace(Convert.ToChar(0x0).ToString(), string.Empty);
            return name;
        }
        #endregion
        private void enableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        public static bool NoClip = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (NoClip == false)
            {
                if (Key_IsDown((uint)Players.SelectedIndex) == "R3 + L3")
                {
                    PS3.SetMemory(0x012320df, new byte[] { 0x02 });
                    NoClip = true;
                }
            }
            else if (NoClip == true)
            {
                if (Key_IsDown((uint)Players.SelectedIndex) == "R3 + L3")
                {
                    PS3.SetMemory(0x012320df, new byte[] { 0x00 });
                    NoClip = false;
                }
            }
        }

        private void disableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            PS3.SetMemory(0x012320df, new byte[] { 0x00 });
            PS3.SetMemory(0x012320df + 0xB0C8, new byte[] { 0x00 });
        }
        public static class Weapons
        {
            public static Byte[]
               Mortar = new Byte[] { 0x00, 0x00, 0x4C },
               M1014 /*Secret Weapon*/ = new Byte[] { 0x00, 0x00, 0x4A },
               Claymore_Fake = new Byte[] { 0x00, 0x00, 0x49 },
               Flash_Granade = new Byte[] { 0x00, 0x00, 0x48 },
               Smoke_Granade = new Byte[] { 0x00, 0x00, 0x47 },
               Airsupport_Marker = new Byte[] { 0x00, 0x00, 0x46 },
               RPG_7 = new Byte[] { 0x00, 0x00, 0x45 },
               Claymore = new Byte[] { 0x00, 0x00, 0x44 },
               Barret_50Cal = new Byte[] { 0x00, 0x00, 0x43 },
               AS50 = new Byte[] { 0x00, 0x00, 0x42 },
               L118A = new Byte[] { 0x00, 0x00, 0x41 },
               RSASS = new Byte[] { 0x00, 0x00, 0x40 },
               Dragunov = new Byte[] { 0x00, 0x00, 0x3F },
               MSR = new Byte[] { 0x00, 0x00, 0x3E },
               AA12 = new Byte[] { 0x00, 0x00, 0x3D },
               Striker = new Byte[] { 0x00, 0x00, 0x3C },
               KSG_12 = new Byte[] { 0x00, 0x00, 0x3B },
               SPAS_12 = new Byte[] { 0x00, 0x00, 0x3A },
               USAS_12 = new Byte[] { 0x00, 0x00, 0x39 },
               Model1887 = new Byte[] { 0x00, 0x00, 0x38 },
               Type95 = new Byte[] { 0x00, 0x00, 0x37 },
               MK14 = new Byte[] { 0x00, 0x00, 0x36 },
               CM901 = new Byte[] { 0x00, 0x00, 0x35 },
               G36c = new Byte[] { 0x00, 0x00, 0x34 },
               FAD = new Byte[] { 0x00, 0x00, 0x33 },
               AK47 = new Byte[] { 0x00, 0x00, 0x32 },
               ACR = new Byte[] { 0x00, 0x00, 0x31 },
               M16A4 = new Byte[] { 0x00, 0x00, 0x2F },
               M4A1 = new Byte[] { 0x00, 0x00, 0x2E },
               MG36 = new Byte[] { 0x00, 0x00, 0x2D },
               L68LSW = new Byte[] { 0x00, 0x00, 0x2C },
               MK46 = new Byte[] { 0x00, 0x00, 0x2B },
               PKP_Pecheneg = new Byte[] { 0x00, 0x00, 0x2A },
               M60E4 = new Byte[] { 0x00, 0x00, 0x29 },
               P90 = new Byte[] { 0x00, 0x00, 0x28 },
               PP90M1 = new Byte[] { 0x00, 0x00, 0x27 },
               PM_9 = new Byte[] { 0x00, 0x00, 0x26 },
               MP7 = new Byte[] { 0x00, 0x00, 0x25 },
               UMP45 = new Byte[] { 0x00, 0x00, 0x24 },
               MP5 = new Byte[] { 0x00, 0x00, 0x23 },
               FMG9 = new Byte[] { 0x00, 0x00, 0x22 },
               MP9 = new Byte[] { 0x00, 0x00, 0x21 },
               Skorpion = new Byte[] { 0x00, 0x00, 0x20 },
               G18 = new Byte[] { 0x00, 0x00, 0x1F },
               P99 = new Byte[] { 0x00, 0x00, 0x1E },
               Magnum44 = new Byte[] { 0x00, 0x00, 0x1D },
               DesertEagle = new Byte[] { 0x00, 0x00, 0x1C },
               MP412 = new Byte[] { 0x00, 0x00, 0x1B },
               USP45 = new Byte[] { 0x00, 0x00, 0x1A },
               FiveSeven = new Byte[] { 0x00, 0x00, 0x19 },
               WiredWeapon_1/*(Freeze When Shoot)*/ = new Byte[] { 0x00, 0x00, 0x18 },
               WiredWeapon_2/*(Freeze When Shoot)*/ = new Byte[] { 0x00, 0x00, 0x17 },
               WiredWeapon_3/*(Freeze When Shoot)*/ = new Byte[] { 0x00, 0x00, 0x11 },
               StingerBullets = new Byte[] { 0x00, 0x00, 0x16 },
               C4 = new Byte[] { 0x00, 0x00, 0x14 },
               Fake_C4 = new Byte[] { 0x00, 0x00, 0x13 },
               CarePackage_Marker = new Byte[] { 0x00, 0x00, 0x12 },
               Mig27_Rockets = new Byte[] { 0x00, 0x00, 0x10 },
               Mig27_Bullets = new Byte[] { 0x00, 0x00, 0x0F },
               RiotShield = new Byte[] { 0x00, 0x00, 0x07 },
               M9 /*Secret */ = new Byte[] { 0x00, 0x00, 0x05 },
               M67_Frag = new Byte[] { 0x00, 0x00, 0x03 },
               BigMissiles /*HOLY FUCK*/= new Byte[] { 0x00, 0x00, 0x02 },
               DefaultWeapon = new Byte[] { 0x00, 0x00, 0x01 },
                PredatorMissile = new Byte[] { 0, 0, 0x15 };
        }
        private void defaultWeaponsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x01 });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x01 });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x01 });
            GiveAmmo();
        }

        private void m9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void mP412ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        public void GiveAmmo()
        {
            PS3.SetMemory((0x01227631 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x01227649 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276cd + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276d9 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276b5 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276c1 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276d9 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
            PS3.SetMemory((0x012276e5 + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xff, 0xff, 0xff });
        }
        private void uSP45ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void fiveSevenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void handGunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x05 });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x05 });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x05 });
            GiveAmmo();
        }

        private void uSP45ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x1A });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x1A });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x1A });
            GiveAmmo();
        }

        private void gToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x1D });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x1D });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x1D });
            GiveAmmo();
        }

        private void radioButton2_CheckedChanged(object sender)
        {
            PS3.ChangeAPI(SelectAPI.ControlConsole);
        }

        private void radioButton1_CheckedChanged(object sender)
        {
            PS3.ChangeAPI(SelectAPI.TargetManager);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void flatButton6_Click(object sender, EventArgs e)
        {
            Random randNum = new Random();
            flatNumeric1.Value = randNum.Next(0, 100000);
            flatNumeric2.Value = randNum.Next(0, 100000);
            flatNumeric3.Value = randNum.Next(0, 100000);
            flatNumeric4.Value = randNum.Next(0, 100000);
            flatNumeric5.Value = randNum.Next(0, 100000);
            flatNumeric6.Value = randNum.Next(0, 100000);
        }
       
        private void flatButton8_Click(object sender, EventArgs e)
        {
        }
        private void flatButton7_Click(object sender, EventArgs e)
        {
           
        }

        private void flatCheckBox2_CheckedChanged(object sender)
        {
            if (flatCheckBox2.Checked)
            {
                PS3.SetMemory(0x67EBC, new byte[] { 0x2C, 0x14, 0x00, 0x01 });
            }
            else
            {
                PS3.SetMemory(0x67EBC, new byte[] { 0x2C, 0x14, 0x00, 0x00 });
            }
        }

        private void flatCheckBox3_CheckedChanged(object sender)
        {
            if (flatCheckBox3.Checked)
            {
                PS3.SetMemory(0x6D798, new byte[] { 0x60, 0x00, 0x00, 0x00 });
            }
            else
            {
                PS3.SetMemory(0x6D798, new byte[] { 0x48, 0x25, 0x15, 0xD1 });
            }
        }

        private void flatCheckBox5_CheckedChanged(object sender)
        {
            if (flatCheckBox5.Checked)
            {
                PS3.SetMemory(0x2BF4D4, new byte[] { 0x2C, 0x03, 0x00, 0x00 });
            }
            else
            {
                PS3.SetMemory(0x2BF4D4, new byte[] { 0x2C, 0x03, 0x00, 0x02 });
            }
        }

        private void flatCheckBox4_CheckedChanged(object sender)
        {
            if (flatCheckBox4.Checked)
            {
                PS3.SetMemory(0x2B0E84, new byte[] { 0x39, 0x60, 0x00, 0x00 });
            }
            else
            {
                PS3.SetMemory(0x2B0E84, new byte[] { 0x3B, 0x40, 0x00, 0x01 });
            }
        }

        private void flatButton9_Click(object sender, EventArgs e)
        {
           
        }

        private void flatButton10_Click(object sender, EventArgs e)
        {
            
        }
      
        private void flatButton9_Click_1(object sender, EventArgs e)
        {
        }

        private void flatButton10_Click_1(object sender, EventArgs e)
        {
        }

        private void enableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x012320df + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0x04 });
        }

        private void disableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x012320df + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0x00 });
        }

        private void flatButton7_Click_1(object sender, EventArgs e)
        {

        }

        private void enableToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x0122778a + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xFF });
        }

        private void disableToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x0122778a + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0x00 });
        }

        private void enabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x0122778b + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0xFF });
        }

        private void disableToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x0122778b + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0x00 });
        }

        private void lockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x012272ef + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0x01 });
        }

        private void dislockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x012272ef + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0x00 });
        }

        private void kickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pedID = (Players.SelectedIndex);
            PS3.SetMemory((0x012272ef + (uint)Players.SelectedIndex * 0xB0C8), new byte[] { 0x04, 0xFF });
        }

        private void rPG7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x45 });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x45 });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x45 });
            GiveAmmo();
        }

        private void airSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x46 });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x46 });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x46 });
            GiveAmmo();
        }

        private void stingerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x16 });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x16 });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x16 });
            GiveAmmo();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void aK47ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x32 });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x32 });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x32 });
            GiveAmmo();
        }

        private void mIG27RocketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps.Mods.GiveWeapon(1, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x4C });
            SpecialOps.Mods.GiveWeapon(2, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x4C });
            SpecialOps.Mods.GiveWeapon(3, Players.SelectedIndex, new byte[] { 0x00, 0x00, 0x4C });
            GiveAmmo();
        }
    }
}
