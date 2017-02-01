namespace QueryLibrary.Tests
{
    public class Console
    {
        public static readonly Console NES = new Console("Nintendo Entertainment System", true, false);

        public static readonly Console GB = new Console("Game Boy", false, true);

        public static readonly Console SNES = new Console("Super Nintendo Entertainment System", true, false);

        public static readonly Console N64 = new Console("Nintendo 64", true, false);

        public static readonly Console GBC = new Console("Game Boy Color", false, true);

        public static readonly Console GBA = new Console("Game Boy Advance", false, true);

        public static readonly Console GC = new Console("Nintendo GameCube", true, false);

        public static readonly Console DS = new Console("Nintendo DS", false, true);

        public static readonly Console Wii = new Console("Wii", true, false);

        public static readonly Console _3DS = new Console("Nintendo 3DS", false, true);

        public static readonly Console WiiU = new Console("Wii U", true, false);

        public static readonly Console Switch = new Console("Nintendo Switch", true, true);    

        public static readonly Console[] NintendoConsoles = new[] { NES, GB, SNES, N64, GBC, GBA, GC, DS, Wii, _3DS, WiiU, Switch, };

        public Console() { }

        public Console(string name, bool homeConsole, bool portableConsole)
        {
            Name = name;
            HomeConsole = homeConsole;
            PortableConsole = portableConsole;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public bool HomeConsole { get; set; }

        public bool PortableConsole { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Console ? this == (Console)obj : false;
        }

        public static bool operator ==(Console a, Console b)
        {
            var nullA = object.ReferenceEquals(a, null);
            var nullB = object.ReferenceEquals(b, null);

            if (nullA && nullB)
                return true;
            else if (nullA != nullB)
                return false;
            else
                return a.Id == b.Id &&
                    a.Name == b.Name &&
                    a.HomeConsole == b.HomeConsole &&
                    a.PortableConsole == b.PortableConsole;
        }

        public static bool operator !=(Console a, Console b)
        {
            return !(a == b);
        }
    }
}
