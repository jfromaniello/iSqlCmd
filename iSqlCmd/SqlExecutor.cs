using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ConsoleApplication13
{
    public class SqlExecutor
    {
        private readonly TextWriter writer;
        private readonly LoginDetails loginDetails;

        public SqlExecutor(TextWriter writer, LoginDetails loginDetails)
        {
            this.writer = writer;
            this.loginDetails = loginDetails;
            loginDetails.Connection.Value.InfoMessage += ConnectionInfoMessage;
        }

        void ConnectionInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            writer.WriteLine(e.Message.RemoveEmptyLines());
        }

        public void Execute(string query)
        {
            using(var command = new SqlCommand(query, loginDetails.Connection.Value))
            {
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.RecordsAffected > 0)
                        {
                            writer.WriteLine("{0} records affected.", reader.RecordsAffected);
                        }

                        var headers = GetHeaders(reader);
                        if (string.IsNullOrEmpty(headers)) return;
                        writer.WriteLine(headers);
                        writer.WriteLine(new string('-', headers.Length));
                        WriteRows(reader);

                    }
                }
                catch (SqlException ex)
                {
                    
                    writer.WriteLine(ex.Message);
                }
            }
        }

        private void WriteRows(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                var schema = reader.GetSchemaTable();
                while (reader.Read())
                {
                    var fields = Enumerable.Range(0, reader.FieldCount)
                              .Select(i => GetPaddedValue(reader, schema, i));
                    writer.WriteLine(string.Join(" ", fields));
                }
            }
        }

        private string GetPaddedValue(IDataRecord reader, DataTable schema, int fieldIndex)
        {
            var lenght = GetWidth(reader, schema, fieldIndex);
            var fieldType = reader.GetFieldType(fieldIndex);
            var value = reader.GetValue(fieldIndex).ToString() ?? "(null)"; 
            if(numericTypes.Contains(fieldType))
            {
                return value.PadLeft(lenght);
            }
            return value.PadRight(lenght);
        }

        private readonly Type[] numericTypes = new[]
                                          {
                                              typeof (Int16), typeof (Int32), typeof (Int64), typeof (float),
                                              typeof (decimal), typeof (double), typeof (Single)
                                          };

        private static string GetHeaders(IDataReader reader)
        {
            var schema = reader.GetSchemaTable();
            var sql = Enumerable.Range(0, reader.FieldCount)
                .Select(i => reader.GetName(i).PadRight(GetWidth(reader, schema, i)));
            return String.Join(" ",sql);
        }

        private static int GetWidth(IDataRecord reader, DataTable schema, int fieldIndex)
        {
            var fieldType = reader.GetFieldType(fieldIndex);
            if(fieldType == typeof(Int32))
            {
                return int.MaxValue.ToString().Length;
            }
            if(fieldType == typeof(string))
            {
                return (int) schema.Rows[fieldIndex]["ColumnSize"];
            }
            return 10;
        }

        
    }
}