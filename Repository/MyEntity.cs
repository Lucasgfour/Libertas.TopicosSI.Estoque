using System.Text;

namespace Repository {
    public class MyEntity<Entity> {

        private MyCommand Command;
        public string TableName;

        internal MyEntity(MyCommand command) {
            Command = command;
            TableName = typeof(Entity).Name;
        }

        public Entity? Select(Object? where = null) => SelectAll(where).FirstOrDefault();

        public List<Entity> SelectAll(Object? where = null) {

            var sql = new StringBuilder();
            var firstWhere = false;

            Command.ClearParameters();

            sql.Append($"SELECT * FROM {TableName}");

            if(where != null) {

                var fields = where.GetType().GetProperties();

                foreach(var field in fields) {

                    if(firstWhere) {
                        sql.Append($"{Environment.NewLine} AND {field.Name} = @{field.Name} ");
                    } else {
                        sql.Append($"{Environment.NewLine} WHERE {field.Name} = @{field.Name} ");
                        where = true;
                    }

                }

                Command.AddParameters(where);

            }

            Command.SetCommandText(sql.ToString());

            return Command.ReadAll<Entity>();

        }

        public void Insert(Entity entity) {

            var sql = new StringBuilder();
            var fields = entity.GetType().GetProperties().ToList();
            var firstLine = true;

            Command.ClearParameters();

            sql.Append($"INSERT INTO {TableName} SET");

            foreach(var field in fields) {

                sql.Append($"{Environment.NewLine} {(firstLine ? "" : ", ")} {field.Name} = @{field.Name}");
                firstLine = false;

            }

            Command.SetCommandText(sql.ToString());
            Command.AddParameters(entity);
            Command.Execute();

        }

    }
}
