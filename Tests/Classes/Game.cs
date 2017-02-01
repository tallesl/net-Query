namespace QueryLibrary.Tests
{
    using System;

    public class Game
    {
        public static readonly Game StarFox = new Game("Star Fox", Console.SNES, new DateTime(1993, 02, 21));

        public static readonly Game StarFox2 = new Game("Star Fox 2", Console.SNES, null);

        public static readonly Game StarFox64 = new Game("Star Fox 64", Console.SNES, new DateTime(1997, 04, 27));

        public static readonly Game StarFoxAdventures = new Game("Star Fox Adventures", Console.GC, new DateTime(2002, 09, 22));

        public static readonly Game StarFoxAssault = new Game("Star Fox: Assault", Console.GC, new DateTime(2005, 02, 14));

        public static readonly Game StarFoxCommand = new Game("Star Fox Command", Console.DS, new DateTime(2006, 08, 03));

        public static readonly Game StarFox643D = new Game("Star Fox 64 3D", Console._3DS, new DateTime(2011, 07, 14));

        public static readonly Game StarFoxZero = new Game("Star Fox Zero", Console.WiiU, new DateTime(2016, 04, 21));

        public static readonly Game[] StarFoxGames = new[] { StarFox, StarFox2, StarFox64, StarFoxAdventures, StarFoxAssault, StarFoxCommand, StarFox643D, StarFoxZero, };

        public Game() { }

        public Game(string name, Console system, DateTime? releaseDate)
        {
            Name = name;
            Console = system;
            ReleaseDate = releaseDate;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public Console Console { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Game ? this == (Game)obj : false;
        }

        public static bool operator ==(Game a, Game b)
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
                    a.Console == b.Console &&
                    a.ReleaseDate == b.ReleaseDate;
        }

        public static bool operator !=(Game a, Game b)
        {
            return !(a == b);
        }
    }
}
