using MySql.Data.MySqlClient;

namespace Repository {
    public class MyCommand {

        private MyConnection Connection;
        private List<MyCommandParameter> Parameters;
        private String? CommandString;

        internal MyCommand(MyConnection connection) {

            Connection = connection;
            Parameters = new List<MyCommandParameter>();

        }

        internal MyCommand(MyConnection connection, String commandString) {

            Connection = connection;
            Parameters = new List<MyCommandParameter>();
            CommandString = commandString;

        }

        private MySqlCommand CreateCommand() {

            var result = Connection.Build().CreateCommand();

            if (CommandString == null)
                throw new Exception("CommandText uninformed.");

            result.CommandText = CommandString;

            Parameters.ForEach((parameter) => { 
                result.Parameters.AddWithValue(parameter.Name, parameter.Value); 
            });

            return result;

        }

        public void SetCommandText(String CommandText) => CommandString = CommandText;

        public void AddParameter(String name, Object value) => Parameters.Add(new MyCommandParameter(name, value));
        public void AddParameters(Object value) {

            var fields = value.GetType().GetProperties().ToList();

            fields.ForEach(field => {

                var pValue = field.GetValue(value);

                if (pValue != null)
                    Parameters.Add(new MyCommandParameter($"@{field.Name}", pValue));

            });

        }
        public void RemoveParameter(String name) => Parameters = Parameters.Where(x => !x.Name.Equals(name)).ToList();
        public void ClearParameters() => Parameters.Clear();
        

        public int Execute() {

            var command = CreateCommand();

            var result = command.ExecuteNonQuery();

            Connection.Close();

            return result;

        }

        public Entity? Read<Entity>() => ReadAll<Entity>().FirstOrDefault();

        public List<Entity> ReadAll<Entity>() {

            var result = new List<Entity>();
            var command = CreateCommand();
            var reader = command.ExecuteReader();

            while (reader.Read()) {

                var entity = Activator.CreateInstance<Entity>();

                if (entity == null)
                    throw new Exception($"Error on create a instance of '{typeof(Entity).Name}'");

                for (int fieldIndex = 0; fieldIndex < reader.FieldCount; fieldIndex++) {

                    var fieldName = reader.GetName(fieldIndex);
                    var fieldEntity = entity.GetType().GetProperty(fieldName);
                    var fieldType = reader.GetValue(fieldIndex).GetType().Name;

                    if (fieldEntity != null && !fieldType.ToLower().Contains("int")) 
                        fieldEntity.SetValue(entity, reader.GetValue(fieldIndex));
                    else if (fieldEntity != null)
                        fieldEntity.SetValue(entity, int.Parse(reader.GetValue(fieldIndex).ToString()));

                }

                result.Add(entity);

            }

            Connection.Close();

            return result;

        }

    }

    internal class MyCommandParameter {

        public string Name { get; set; }
        public object Value { get; set; }

        public MyCommandParameter(string name, object value) {
            Name = name;
            Value = value;
        }
    }
}
