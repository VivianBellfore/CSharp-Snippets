 //   CSharp-Snippets, Copyright(C) 2024  by Vivian Bellfore

 //   This file is free software: you can redistribute it and/or modify
 //   it under the terms of the GNU General Public License as published by
 //   the Free Software Foundation, either version 3 of the License, or
 //   (at your option) any later version.

 //   This file is distributed in the hope that it will be useful,
 //   but WITHOUT ANY WARRANTY; without even the implied warranty of
 //   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 //   GNU General Public License for more details.

 //   You should have received a copy of the GNU General Public License
 //   along with this file.  If not, see <https://www.gnu.org/licenses/>.



using System;
using System.IO;
using System.Dynamic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;

using MySqlConnector;




/// <summary>
/// This class is handeling all data base functions for sql and xml.
/// </summary>
public class MySQLWrapper
{
	private static string mySqlConnectionString;


	#region XML
	/// <summary>
	/// Streamreader and xml serializer to read data from Config.xml.
	/// </summary>
	/// <returns>Data from config in given type.</returns>
	public static T ReadFromCharacterData<T>() where T : new()
	{
		TextReader reader = null;

		try
		{
			var serializer = new XmlSerializer(typeof(T));
			reader = new StreamReader("Config.xml");

			return (T)serializer.Deserialize(reader);
		}
		finally
		{
			reader.Close();
		}
	}

	/// <summary>
	/// Setup SQL connection strings for all data bases.
	/// </summary>
	public static void ReadDataBaseStrings()
	{
		DataBase data = ReadFromCharacterData<DataBase>();

		mySqlConnectionString = data.ConnectionString;

		Console.WriteLine($"[Info {DateTime.Now.ToShortTimeString()}] Database connection strings are setup.");
	}

	/// <summary>
	/// Got the data base connection string based on an id.
	/// </summary>
	private static string GetDataBaseConnectionString(int dataBaseId)
	{
		string connectionString = string.Empty;

		if (dataBaseId == 1) connectionString = mySqlConnectionString;

		if (connectionString == string.Empty)
			throw new ArgumentOutOfRangeException(nameof(connectionString), $"[Error {DateTime.Now.ToShortTimeString()}] Database connection string could not be found.");

		return connectionString;
	}
	#endregion



	#region Sql statement builder
	/// <summary>
	/// Building the MySqlCommand for all data base functions.
	/// </summary>
	private static MySqlCommand BuildMySqlCommand(MySqlConnection sql_connection, string sqlStatment, Dictionary<string, object> arguments = null)
	{
		MySqlCommand cmd = new MySqlCommand
		{
			Connection = sql_connection,
			CommandText = sqlStatment
		};

		if (arguments != null)
		{
			foreach (var obj in arguments)
			{
				cmd.Parameters.AddWithValue(obj.Key, obj.Value);
			}
		}

		return cmd;
	}

	/// <summary>
	/// This functon returns first column of the first row in the result set, or a null reference if the result set is empty. Returns a maximum of 2033 characters.
	/// </summary>
	public static async Task<object> SQLExecuteScalar(int dataBaseId, string sqlStatment, Dictionary<string, object> arguments = null)
	{
		try
		{
			string connectionString = GetDataBaseConnectionString(dataBaseId);
			MySqlConnection sql_connection = new MySqlConnection(connectionString);

			await sql_connection.OpenAsync();
			MySqlCommand cmd = BuildMySqlCommand(sql_connection, sqlStatment, arguments);
			object result = await cmd.ExecuteScalarAsync();
			await sql_connection.CloseAsync();

			if (result == DBNull.Value)
				return null;
			else
				return result;

		}
		catch (Exception ex)
		{
			await Utilities.SendLogMessage(1, $"# MySqlWraper, SQLExecuteScalar\n{ex}");
			return null;
		}
	}

	/// <summary>
	/// Executes a Transact-SQL statement against the connection and returns the number of rows affected.<para/>
	/// Returning:<br/>
	/// int - number of rows that was updated/inserted/deleted.
	/// </summary>
	public static async Task<int> SQLExecuteNonQuery(int dataBaseId, string sqlStatment, Dictionary<string, object> arguments = null)
	{
		try
		{
			string connectionString = GetDataBaseConnectionString(dataBaseId);

			MySqlConnection sql_connection = new MySqlConnection(connectionString);
			await sql_connection.OpenAsync();
			MySqlCommand cmd = BuildMySqlCommand(sql_connection, sqlStatment, arguments);

			int updateCount = await cmd.ExecuteNonQueryAsync();
			await sql_connection.CloseAsync();

			return updateCount;
		}
		catch (Exception exception)
		{
			await Utilities.SendLogMessage(1, $"# MySqlWraper, SQLExecuteNonQuery\n{exception.Message}");
			return 0;
		}
	}

	/// <summary>
	/// Sends the CommandText to the Connection and builds a SqlDataReader. Gives back a dynamic List of objects.<para/>
	/// Values can be get by the column name of the data.
	/// </summary>
	public static async Task<List<dynamic>> SQLExecuteReader(int dataBaseId, string sqlStatment, Dictionary<string, object> arguments = null)
	{
		try
		{
			List<dynamic> result = new List<dynamic>();
			string connectionString = GetDataBaseConnectionString(dataBaseId);
			MySqlConnection sql_connection = new MySqlConnection(connectionString);

			await sql_connection.OpenAsync();
			MySqlCommand cmd = BuildMySqlCommand(sql_connection, sqlStatment, arguments);

			using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
			{
				var columns = await reader.GetColumnSchemaAsync();

				while (await reader.ReadAsync())
				{
					dynamic resultSet = new ExpandoObject();
					IDictionary<string, object> row = resultSet;

					for (int i = 0; i < columns.Count; i++)
					{
						row.Add(columns[i].ColumnName, reader.IsDBNull(i) ? null : reader.GetValue(i));
					}
					result.Add(row);
				}
			}

			await sql_connection.CloseAsync();
			return result;
		}
		catch (Exception exception)
		{
			await Utilities.SendLogMessage(1, $"# MySqlWraper, SQLExecuteReader\n{exception}");
			return new List<dynamic>();
		}
	}

	/// <summary>
	/// Gives back a List of all objects from a specific column without dynamic.<para/>
	/// If no column is called (*) it will give back the content from the first column in the table.<para/>
	/// Returns an empty list, if nothing is found.
	/// </summary>
	public static async Task<List<T>> SQLExecuteColumnReader<T>(int dataBaseId, string sqlStatment, Dictionary<string, object> arguments = null)
	{
		try
		{
			List<T> result = new List<T>();
			string connectionString = GetDataBaseConnectionString(dataBaseId);
			MySqlConnection sql_connection = new MySqlConnection(connectionString);

			await sql_connection.OpenAsync();
			MySqlCommand cmd = BuildMySqlCommand(sql_connection, sqlStatment, arguments);

			using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
			{
				var columns = await reader.GetColumnSchemaAsync();

				while (await reader.ReadAsync())
				{
					if (!reader.IsDBNull(0))
						result.Add((T)reader.GetValue(0));
				}
			}

			await sql_connection.CloseAsync();
			return result;
		}
		catch (Exception exception)
		{
			await Utilities.SendLogMessage(1, $"# MySqlWraper, SQLExecuteColumnReader\n{exception}");
			return new List<T> { };
		}
	}
	#endregion



	/// <summary>
	/// This function is setting, adding or removing integer amounts from a table and column in database that is related to a user id.<para/>
	/// Returns false if updated could not be saved.<para/>
	/// 0 = To set amount.<br/>
	/// 1 = To add amount.<br/>
	/// 2 = To remove amount.
	/// </summary>
	public static async Task<bool> SetIntegerForDB(string table, string column, ulong userId, int integer, int setting, bool canBeNegative)
	{
		object currentAmount = await SQLExecuteScalar(1,
			$"SELECT `{column}` FROM `{table}` WHERE `user_id` = @user_id",
			new Dictionary<string, object>() { { "user_id", userId } });

		if (currentAmount == null)
		{
			await Utilities.SendLogMessage(1, $"# RPGManager, SetIntegerForDB\ncould not get current amount from database.\n User id ||{userId}|| and table `{table}` and column `{column}`.");
			return false;
		}

		int newAmount;

		if (setting == 1)
			newAmount = Convert.ToInt32(currentAmount) + integer;
		else if (setting == 2)
		{
			newAmount = Convert.ToInt32(currentAmount) - integer;
			if (newAmount < 0 && !canBeNegative) newAmount = 0; // we dont want negative integer in every case
		}
		else
			newAmount = integer;

		int updateCount = await SQLExecuteNonQuery(1,
			$"UPDATE `{table}` SET `{column}` = @column WHERE `user_id` = @user_id",
			new Dictionary<string, object>() { { "user_id", userId }, { "column", newAmount } });

		if (updateCount == 0)
			return false;
		else
			return true;
	}
}


/// <summary>
/// This class is building an object for our data base connection information.
/// </summary>
public class DataBase
{
	public string ConnectionString { get; set; }
}

