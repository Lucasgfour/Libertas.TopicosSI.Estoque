using MySql.Data.MySqlClient;

namespace Repository {
    public class MyConnection {

        private MySqlConnection? Connection;
        private String ConnectionString;

        internal MyConnection(String connectionString) {
            ConnectionString = connectionString;
        }

        public MySqlConnection Build() {

            if(Connection == null)
                Connection = new MySqlConnection(ConnectionString);

            if(Connection.State != System.Data.ConnectionState.Open)
                Connection = new MySqlConnection(ConnectionString);

            Connection.Open();

            return Connection;

        }

        public void Close() {

            if (Connection == null)
                return;

            if (Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();

            Connection = null;

        }

    }
}
