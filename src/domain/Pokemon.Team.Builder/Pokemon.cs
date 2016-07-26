using System;
namespace Pokemon.Team.Builder
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return $"#{Id} - {Name}";
        }
    }
}
