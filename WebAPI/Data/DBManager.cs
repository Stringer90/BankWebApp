using System.Data.SQLite;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=mydatabase.db;Version=3;";

        public static bool CreateAccount(int userID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Account (userID) VALUES (@userID);";

                        command.Parameters.AddWithValue("@userID", userID);

                        int rowsUpdated = command.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public static Account? GetAccount(int? accountNum)
        {
            Account? account = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT * FROM Account WHERE accountID = @AccountNum";
                        command.Parameters.AddWithValue("@AccountNum", accountNum);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account = new Account
                                {
                                    AccountNum = Convert.ToInt32(reader["accountID"]),
                                    UserID = Convert.ToInt32(reader["userID"]),
                                    Balance = Convert.ToDouble(reader["balance"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return account;
        }

        public static bool UpdateAccount(int? accountNum, double newBalance)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"UPDATE Account SET balance = @Balance WHERE accountID = @AccountID";
                        command.Parameters.AddWithValue("@Balance", newBalance);
                        command.Parameters.AddWithValue("@AccountID", accountNum);

                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();

                        return rowsUpdated > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating account: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteAccount(int accountNum)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"DELETE FROM Account WHERE accountID = @AccountID";
                        command.Parameters.AddWithValue("@AccountID", accountNum);

                        // Execute the SQL command
                        int rowsDeleted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static bool AccountWithdraw(int accountNum, double amount)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"UPDATE Account SET balance = balance - @Amount WHERE accountID = @AccountID";
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@AccountID", accountNum);

                        // Execute the SQL command
                        int rowsUpdated = command.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static bool AccountDeposit(int accountNum, double amount)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"UPDATE Account SET balance = balance + @Amount WHERE accountID = @AccountID";
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@AccountID", accountNum);

                        // Execute the SQL command
                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static List<Account> GetAccountsOfUser(int userID)
        {
            List<Account> accountList = new List<Account>();

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"SELECT * FROM Account WHERE userID = @UserID";
                        command.Parameters.AddWithValue("@UserID", userID);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account account = new Account();
                                account.AccountNum = Convert.ToInt32(reader["accountID"]);
                                account.UserID = Convert.ToInt32(reader["userID"]);
                                account.Balance = Convert.ToDouble(reader["balance"]);

                                accountList.Add(account);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return accountList;
        }

        public static bool CreateTransaction(int? senderNum, int? receiverNum, double amount, string description, string type, string date)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // SQL command to insert data
                        command.CommandText = @"INSERT INTO Transactions (senderNum, receiverNum, amount, description, type, date) VALUES (@SenderNum, @ReceiverNum, @Amount, @Description, @Type, @Date)";

                        // Define parameters for the query
                        command.Parameters.AddWithValue("@SenderNum", senderNum);
                        command.Parameters.AddWithValue("@ReceiverNum", receiverNum);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Type", type);
                        command.Parameters.AddWithValue("@Date", date);

                        // Execute the SQL command
                        int rowsInserted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static List<TransactionAccount> GetAccountTransactions(int accountNum, string startDate, string endDate)
        {
            List<TransactionAccount> transactionList = new List<TransactionAccount>();

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"
                            SELECT * FROM (
                                SELECT 
                                    t.date,
                                    t.type,
                                    'from: ' || u.username || ' (' || a.accountID || ')' AS counterparty,
                                    t.amount AS amount,
                                    t.description AS description
                                FROM 
                                    Transactions t
                                    JOIN Account a ON t.senderNum = a.accountID
                                    JOIN User u ON a.userID = u.userID
                                WHERE 
                                    t.receiverNum = @AccountID AND t.type = 'transfer' AND t.date BETWEEN @StartDate AND @EndDate

                                UNION ALL

                                SELECT 
                                    t.date,
                                    t.type,
                                    'to: ' || u.username || ' (' || a.accountID || ')' AS counterparty,
                                    -t.amount AS amount,
                                    t.description AS description
                                FROM 
                                    Transactions t
                                    JOIN Account a ON t.receiverNum = a.accountID
                                    JOIN User u ON a.userID = u.userID
                                WHERE 
                                    t.senderNum = @AccountID AND t.type = 'transfer' AND t.date BETWEEN @StartDate AND @EndDate

                                UNION ALL

                                SELECT 
                                    t.date,
                                    t.type,
                                    '-' AS counterparty,
                                    t.amount AS amount,
                                    t.description AS description
                                FROM 
                                    Transactions t
                                WHERE 
                                    t.receiverNum = @AccountID AND t.type = 'deposit' AND t.date BETWEEN @StartDate AND @EndDate

                                UNION ALL

                                SELECT 
                                    t.date,
                                    t.type,
                                    '-' AS counterparty,
                                    -t.amount AS amount,
                                    t.description AS description
                                FROM 
                                    Transactions t
                                WHERE 
                                    t.senderNum = @AccountID AND t.type = 'withdrawal' AND t.date BETWEEN @StartDate AND @EndDate
                            )
                            AS result
                            ORDER BY date DESC";

                        command.Parameters.AddWithValue("@AccountID", accountNum);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TransactionAccount transaction = new TransactionAccount();

                                transaction.Date = reader.GetDateTime(reader.GetOrdinal("date")).ToString("yyyy-MM-dd");
                                transaction.Type = reader["type"].ToString();
                                transaction.Counterparty = reader["counterparty"].ToString();
                                transaction.Amount = Convert.ToDouble(reader["amount"]);
                                transaction.Description = reader["description"].ToString();

                                transactionList.Add(transaction);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return transactionList;
        }

        public static List<Transaction> GetAllTransactionsDateDesc(string searchInput, string startDate, string endDate)
        {
            List<Transaction> transactionList = new List<Transaction>();

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        /*
                        command.CommandText = @"
                            SELECT * FROM Transactions t
                            WHERE 
                                (t.date BETWEEN @StartDate AND @EndDate)
                                AND
                                (t.description LIKE '%' || @SearchInput || '%' OR t.type LIKE '%' || @SearchInput || '%')
                            ORDER BY date DESC";
                        */
                        command.CommandText = @"
                            SELECT 
                                t.date,
                                    (SELECT u.username || ' (' || a.accountID || ')' 
                                    FROM Account a 
                                        JOIN User u ON a.userID = u.userID 
                                    WHERE a.accountID = t.senderNum) 
                                AS sender,
                                    (SELECT u.username || ' (' || a.accountID || ')' 
                                    FROM Account a 
                                        JOIN User u ON a.userID = u.userID 
                                    WHERE a.accountID = t.receiverNum) 
                                AS receiver,
                                t.type,
                                t.amount,
                                t.description
                            FROM Transactions t
                            WHERE 
                                (t.date BETWEEN @StartDate AND @EndDate)
                                AND
                                (t.description LIKE '%' || @SearchInput || '%' 
                                OR t.type LIKE '%' || @SearchInput || '%' 
                                OR (SELECT u.username || ' (' || a.accountID || ')' 
                                    FROM Account a 
                                    JOIN User u ON a.userID = u.userID 
                                    WHERE a.accountID = t.senderNum) LIKE '%' || @SearchInput || '%' 
                                OR (SELECT u.username || ' (' || a.accountID || ')' 
                                    FROM Account a 
                                    JOIN User u ON a.userID = u.userID 
                                    WHERE a.accountID = t.receiverNum) LIKE '%' || @SearchInput || '%' 
                            ORDER BY t.date DESC";

                        command.Parameters.AddWithValue("@SearchInput", searchInput);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction();

                                transaction.Date = reader.GetDateTime(reader.GetOrdinal("date")).ToString("yyyy-MM-dd");
                                transaction.Sender = reader["sender"]?.ToString();
                                transaction.Receiver = reader["receiver"]?.ToString();
                                transaction.Type = reader["type"].ToString();
                                transaction.Amount = Convert.ToDouble(reader["amount"]);
                                transaction.Description = reader["description"].ToString();

                                transactionList.Add(transaction);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return transactionList;
        }

        public static bool CreateUser(string username, string password, string email, string address, string phone, int admin = 0)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // SQL command to insert data
                        command.CommandText = @"INSERT INTO User (username, password, email, address, phone, admin) VALUES (@Username, @Password, @Email, @Address, @Phone, @Admin)";

                        // Define parameters for the query
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Admin", admin);

                        // Execute the SQL command
                        int rowsInserted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static User? GetUser(string searchString)
        {
            User? user = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM User WHERE username = @Username OR email = @Email OR phone = @Phone";
                        command.Parameters.AddWithValue("@Username", searchString);
                        command.Parameters.AddWithValue("@Email", searchString);
                        command.Parameters.AddWithValue("@Phone", searchString);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User();
                                user.UserID = Convert.ToInt32(reader["userID"]);
                                user.Username = reader["username"]?.ToString();
                                user.Password = reader["password"]?.ToString();
                                user.Email = reader["email"].ToString();
                                user.Address = reader["address"].ToString();
                                user.Phone = reader["phone"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return user;
        }

        public static User? GetUserWithID(int userID)
        {
            User? user = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM User WHERE userID = @UserID";
                        command.Parameters.AddWithValue("@UserID", userID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User();
                                user.UserID = Convert.ToInt32(reader["userID"]);
                                user.Username = reader["username"]?.ToString();
                                user.Password = reader["password"]?.ToString();
                                user.Email = reader["email"].ToString();
                                user.Address = reader["address"].ToString();
                                user.Phone = reader["phone"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return user;
        }

        public static bool UpdateUser(string username, string password, string email, string address, string phone, int userID)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"UPDATE User SET username = @Username, password = @Password, email = @Email, address = @Address, phone = @Phone WHERE userID = @UserID";
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@UserID", userID);

                        // Execute the SQL command
                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteUser(int userID)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"DELETE FROM User WHERE userID = @UserID";
                        command.Parameters.AddWithValue("@UserID", userID);

                        // Execute the SQL command
                        int rowsDeleted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static bool IsValidAuth(string usernameOrEmail, string password)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"SELECT * FROM User WHERE (username = @Username AND password = @Password) OR (email = @Email AND password = @Password);";
                        command.Parameters.AddWithValue("@Username", usernameOrEmail);
                        command.Parameters.AddWithValue("@Email", usernameOrEmail);
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the SQL command
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                        }
                    }
                    connection.Close();
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static bool IsAdminUser(string usernameOrEmail)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command
                        command.CommandText = @"SELECT * FROM User WHERE ((username = @Username) OR (email = @Email)) AND admin = 1;";
                        command.Parameters.AddWithValue("@Username", usernameOrEmail);
                        command.Parameters.AddWithValue("@Email", usernameOrEmail);

                        // Execute the SQL command
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                        }
                    }
                    connection.Close();
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static void DBInitialise()
        {
            CreateUserTable();
            CreateAccountTable();
            CreateTransactionTable();

            // Insert Admin user, userID will be 1
            CreateUser("admin", "admin", "admin@email.com", "1 Admin St", "0412345678", 1);

            // Data Seeding
            SeedData();
        }

        public static bool CreateUserTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"DROP TABLE IF EXISTS User";
                        command.ExecuteNonQuery();

                        // SQL command to create a table
                        command.CommandText = @"
                        CREATE TABLE User (
                            userID INTEGER PRIMARY KEY AUTOINCREMENT,
                            username VARCHAR(20) NOT NULL,
                            password VARCHAR(20) NOT NULL,
                            email VARCHAR(50) NOT NULL,
                            address VARCHAR(100) NOT NULL,
                            phone VARCHAR(10) NOT NULL,
                            admin INTEGER DEFAULT 0 CHECK (admin IN (0, 1))
                        )";

                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("Table created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false; // Create table failed
        }

        public static bool CreateAccountTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"DROP TABLE IF EXISTS Account";
                        command.ExecuteNonQuery();

                        // SQL command to create a table
                        command.CommandText = @"
                        CREATE TABLE Account (
                            accountID INTEGER PRIMARY KEY AUTOINCREMENT,
                            userID INTEGER NOT NULL,
                            balance DECIMAL(10, 2) NOT NULL DEFAULT 0,
                            FOREIGN KEY (userID) REFERENCES User(userID) ON DELETE CASCADE
                        )";

                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();

                        // Initialise Account table to make account number start at 10,000
                        command.CommandText = @"
                        INSERT INTO sqlite_sequence (name, seq) 
                        VALUES ('Account', 9999);";

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
                Console.WriteLine("Table created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false; // Create table failed
        }

        public static bool CreateTransactionTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"DROP TABLE IF EXISTS Transactions";
                        command.ExecuteNonQuery();

                        // SQL command to create a table
                        command.CommandText = @"
                        CREATE TABLE Transactions (
                            transactionID INTEGER PRIMARY KEY AUTOINCREMENT,
                            senderNum INTEGER,
                            receiverNum INTEGER,
                            amount DECIMAL(10, 2) NOT NULL,
                            description VARCHAR(100) NOT NULL DEFAULT '-',
                            type VARCHAR(10) NOT NULL CHECK(type IN ('deposit', 'withdrawal', 'transfer')),
                            date DATE NOT NULL,
                            FOREIGN KEY (senderNum) REFERENCES Account(accountID),
                            FOREIGN KEY (receiverNum) REFERENCES Account(accountID)
                        )";

                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("Table created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false; // Create table failed
        }

        public static void SeedData()
        {
            Random random = new Random();

            // Create list to store created user ids
            List<int?> userIds = new List<int?>();

            // Create list to store created account ids
            List<int?> accountIds = new List<int?>();

            // Create 5-10 users
            int numUsers = random.Next(5, 11);
            for (int i = 0; i < numUsers; i++)
            {
                string username = $"user{i + 1}";
                string password = $"password{i + 1}";
                string email = $"{username}@email.com";
                string address = $"{i + 1} Bank Street";
                string phone = $"04{random.Next(10000000, 99999999)}";
                CreateUser(username, password, email, address, phone);

                // Get the user ID and store them
                User user = GetUser(username);
                userIds.Add(user.UserID);
            }

            // Create 1-3 accounts for each user createdd
            foreach (int userId in userIds)
            {
                int numAccounts = random.Next(1, 4);
                for (int i = 0; i < numAccounts; i++)
                {
                    CreateAccount(userId);
                    // Get the id of the account just created above
                    // (get the account number of the most recently inserted account for the current user)
                    Account account = GetAccount(GetAccountsOfUser(userId).Last().AccountNum);
                    accountIds.Add(account.AccountNum);
                    // Set the account balance to amount between 5000-10000
                    double balance = random.NextDouble() * 5000 + 5000;
                    UpdateAccount(account.AccountNum, balance);
                }
            }

            // Create 3 transactions for each account, a deposit, a withdrawal and transfer
            foreach (int accountId in accountIds)
            {
                // Deposit
                double depositAmount = random.NextDouble() * 50 + 50;
                DateTime depositDate = new DateTime(2024, 9, random.Next(1, 31));
                CreateTransaction(null, accountId, depositAmount, "Deposit description", "deposit", depositDate.ToString("yyyy-MM-dd"));

                // Withdrawal
                double withdrawalAmount = random.NextDouble() * 50 + 50;
                DateTime withdrawalDate = new DateTime(2024, 9, random.Next(1, 31));
                CreateTransaction(accountId, null, withdrawalAmount, "Withdrawal description", "withdrawal", withdrawalDate.ToString("yyyy-MM-dd"));

                // Transfer
                int? transferAccountId = accountIds[random.Next(accountIds.Count)];
                // Assure an account is not transferring to itself
                while (transferAccountId == accountId)
                {
                    transferAccountId = accountIds[random.Next(accountIds.Count)];
                }
                double transferAmount = random.NextDouble() * 50 + 50;
                DateTime transferDate = new DateTime(2024, 9, random.Next(1, 31));
                CreateTransaction(accountId, transferAccountId, transferAmount, "Transfer description", "transfer", transferDate.ToString("yyyy-MM-dd"));
            }
        }
    }
}
