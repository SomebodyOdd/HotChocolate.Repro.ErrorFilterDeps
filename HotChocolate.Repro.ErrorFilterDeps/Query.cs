namespace HotChocolate.Repro.ErrorFilterDeps
{
    public class Query
    {
        private readonly SomeCoolService _constructorService;
        public Query(SomeCoolService service)
        {
            _constructorService = service;
        }

        public Book GetBook([Service] SomeCoolService methodService)
        {
            if (!methodService.CheckSomething())
                throw new System.Exception();

            return new Book
            {
                Title = "C# in depth.",
                Author = new Author
                {
                    Name = "Jon Skeet"
                }
            };
        }
    }
}
