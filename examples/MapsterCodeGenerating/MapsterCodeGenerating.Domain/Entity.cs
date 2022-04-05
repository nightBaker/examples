namespace MapsterCodeGenerating
{
    public class Entity
    {
        public Entity(string description)
        {
            Description = description;
        }

        public int Id { get; private set; }

        public string Description { get; private set; }

        public void DoSomething() 
        {
            //buisness logic
        }
    }
}