namespace Repository {
    public class MyContext {

        private String? ConnectionString;

        public MyContext() { }

        public void SetConnectionString(String connectionString) => ConnectionString = connectionString; 

        private MyConnection BuildConnection() {

            if (ConnectionString == null)
                throw new Exception("ConnectionString uninformed.");

            return new MyConnection(ConnectionString);

        }

        public MyCommand BuildMyCommand() => new MyCommand(BuildConnection());
        public MyCommand BuildMyCommand(String CommandText) => new MyCommand(BuildConnection(), CommandText);
        public MyEntity<Entity> BuildMyEntity<Entity>() => new MyEntity<Entity>(new MyCommand(BuildConnection()));

    }
}
