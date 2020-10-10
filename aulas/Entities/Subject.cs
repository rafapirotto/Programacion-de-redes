namespace Entities
{
    public class Subject
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public Subject(string name)
        {
            Name = name;
        }
    }
}