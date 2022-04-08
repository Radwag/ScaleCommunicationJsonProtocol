using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.RegularExpressions;
using ConsoleSample.JsonRdgProtocol.DataModel;
using Services.Commands;
using Services.Responds;
using Services.Senders;

namespace ConsoleSample.JsonRdgProtocol
{
    public class JsonRdgProtocol
    {
        // Declare variables
        public string? IpAddress = String.Empty;
        public ISender Sender = null;
        public WINFO ScaleInfo;
        public string Status = "NotConnected";
        public List<TableItem> Tables = new List<TableItem>();
        public List<DbInfo> DbInfos = new List<DbInfo>();

        internal void Connect()
        {
            // Connect to device.
            Console.WriteLine("Enter the ip address:\n------------------------\n");
            while (Status == "NotConnected")
            {
                IpAddress = Console.ReadLine();

                if (IpAddress != null && !Regex.IsMatch(IpAddress,
                    @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"))
                {
                    Console.WriteLine(
                        "Invalid Ip address format\n------------------------\nEnter the ip address:\n------------------------\n");
                }
                else
                {
                    if (Sender == null || !Sender.IsConnected)
                    {
                        Sender = new TcpSender(IpAddress, 4001);
                        Console.WriteLine("Connecting...\n------------------------\n");
                        if (Sender.ConnectAsync().Result)
                        {
                            InitializeConnection();
                        }
                        else
                        {
                            Console.WriteLine("Error\n------------------------\n");
                        }
                    }
                    else
                    {
                        PerformDisconnection();
                    }

                    Status = "Connected";
                }
            }
        }

        //Get Tables Method
        internal void GetTables(bool canConsoleWrite)
        {
            var tablesJson = JsonDocument
                .Parse(RemoveCrc(SendAndReceive(CraftQuery("{\"Command\":\"DbGetTables\"}"), canConsoleWrite)))
                .RootElement.GetProperty("Tables");
            if (tablesJson.ValueKind == JsonValueKind.Array)
            {
                if (canConsoleWrite) Console.WriteLine("Tables:\n------------------------");
                foreach (JsonElement jsonElement in tablesJson.EnumerateArray())
                {
                    var tableItem = JsonSerializer.Deserialize<TableItem>(jsonElement.ToString());
                    Tables.Add(tableItem);

                    if (canConsoleWrite)
                        Console.WriteLine(GetPropertyNameAndValue(() => tableItem.Name) + ", " +
                                          GetPropertyNameAndValue(() => tableItem.TranslationCurrent) + ", " +
                                          GetPropertyNameAndValue(() => tableItem.TranslationEn) + ", " +
                                          GetPropertyNameAndValue(() => tableItem.Alterability));
                }
            }
        }

//Get DbInfo metgods
        internal void GetDbInfos(bool canConsoleWrite)
        {
            foreach (TableItem tableItem in Tables)
            {
                GetDbInfo(tableItem.Name, canConsoleWrite);
            }
        }

        internal DbInfo GetDbInfo(string tableName, bool canConsoleWrite)
        {
            DbInfo dbInfoItem = default;
            var tablesJson = JsonDocument.Parse(RemoveCrc(SendAndReceive(
                CraftQuery("{\"Table\":\"" + tableName + "\", \"Command\":\"TableInfo\"}"), canConsoleWrite)));
            if (tablesJson.RootElement.ValueKind == JsonValueKind.Object)
            {
                dbInfoItem = JsonSerializer.Deserialize<DbInfo>(tablesJson.RootElement.ToString());
                var dbInfoItemExist = DbInfos.FirstOrDefault(info => info.Table == tableName);
                if (dbInfoItemExist != null) DbInfos.Remove(dbInfoItemExist);
                if (dbInfoItem != null) DbInfos.Add(dbInfoItem);
            }

            return dbInfoItem;
        }

//List Records From Table
        internal void ListRecordsFromTable(string tableName)
        {
            var dbInfoItem = DbInfos.FirstOrDefault(info => info.Table == tableName);
            dbInfoItem?.Records.Clear();

            for (int index = 0; index < DbInfos.FirstOrDefault(item => item.Table == tableName)?.Count; index++)
            {
                dbInfoItem?.Records.Add(GetRecordFromTable(tableName, index, null));
            }
        }

//Get record by id
        internal void GetRecordById(string tableName, Int64? id)
        {
            var item = GetRecordFromTable(tableName, null, id);
            var dbInfoItem = DbInfos.FirstOrDefault(info => info.Table == tableName);
            List<ColumnRecordItem> ColumnRecordItems = null;
            foreach (List<ColumnRecordItem> columnRecordItems in dbInfoItem.Records)
            {
                if (columnRecordItems.Any(item => item.Value.ToString() == id.ToString()))
                    ColumnRecordItems = columnRecordItems;
            }

            if (ColumnRecordItems == null)
            {
                dbInfoItem?.Records.Add(item);
            }
            else
            {
                dbInfoItem?.Records.Remove(ColumnRecordItems);
                dbInfoItem?.Records.Add(item);
            }
        }
//Add record to table

        internal void AddRecordToTable(string tableName)
        {
            CommandInsert commandInsert = new CommandInsert();
            commandInsert.SetDestinationTable(tableName);
            var dbInfoItem = GetDbInfo(tableName, true);
            foreach (ColumnSpecification columnSpecification in dbInfoItem.Items)
            {
                if (columnSpecification.Alterability == 0)
                {
                    string enumValues = string.Empty;
                    if (columnSpecification.ValuesEn != null)
                        enumValues = ", Enum values: [" + string.Join(", ", columnSpecification.ValuesEn) + "]";
                    Console.WriteLine("Enter value (" + (TableInfoItemType) columnSpecification.Type + ") " +
                                      enumValues + " for: " + columnSpecification.Name);
                    columnSpecification.Value = (object) Console.ReadLine();
                    commandInsert.AddObject(columnSpecification.DbName, columnSpecification.Value);
                }
            }

            var jsonDocument =
                JsonDocument.Parse(RemoveCrc(SendAndReceive(CraftQuery(commandInsert.BuildQuery()), true)));
            GetRecordById(tableName, jsonDocument.RootElement.GetProperty("Id").GetInt64());
        }

//Update record        
        internal void UpdateRecordFromTable(string tableName)
        {
            CommandUpdate commandUpdate = new CommandUpdate();
            commandUpdate.SetDestinationTable(tableName);
            var dbInfoItem = GetDbInfo(tableName, true);
            Console.WriteLine("Available records in the table " + tableName + "\n------------------------\n");
            ListRecordsFromTable(tableName);
            Console.WriteLine("Enter the record id.");
            var id = Convert.ToInt64(Console.ReadLine());
            List<ColumnRecordItem> ColumnRecordItems = null;
            foreach (List<ColumnRecordItem> columnRecordItems in dbInfoItem.Records)
            {
                if (columnRecordItems.Any(item => item.Value.ToString() == id.ToString()))
                    ColumnRecordItems = columnRecordItems;
            }

            if (ColumnRecordItems == null)
            {
                Console.WriteLine("Not found records");
            }
            else
            {
                foreach (ColumnRecordItem columnSpecification in ColumnRecordItems)
                {
                    if (columnSpecification.Alterability == 0)
                    {
                        string enumValues = string.Empty;
                        if (columnSpecification.ValuesEn != null)
                            enumValues = ", Enum values: [" + string.Join(", ", columnSpecification.ValuesEn) + "]";
                        Console.WriteLine("Enter value (" + (TableInfoItemType) columnSpecification.Type + ") " +
                                          enumValues + " for: " + columnSpecification.Name + ",Old value: " +
                                          columnSpecification.Value);
                        columnSpecification.Value = (object) Console.ReadLine();
                        commandUpdate.AddObject(columnSpecification.DbName, columnSpecification.Value);
                    }
                }

                commandUpdate.SetRecordId(id);
                var jsonDocument =
                    JsonDocument.Parse(RemoveCrc(SendAndReceive(CraftQuery(commandUpdate.BuildQuery()), true)));
                GetRecordById(tableName, jsonDocument.RootElement.GetProperty("Id").GetInt64());
            }
        }

//Delete record
        public void DeleteRecordFromTable(string tableName)
        {
            Console.WriteLine("Available records in the table " + tableName + "\n------------------------\n");
            ListRecordsFromTable(tableName);
            Console.WriteLine("Enter the record id.");
            var id = Convert.ToInt64(Console.ReadLine());
            var dbInfoItem = DbInfos.FirstOrDefault(info => info.Table == tableName);
            List<ColumnRecordItem> ColumnRecordItems = null;
            foreach (List<ColumnRecordItem> columnRecordItems in dbInfoItem.Records)
            {
                if (columnRecordItems.Any(item => item.Value.ToString() == id.ToString()))
                    ColumnRecordItems = columnRecordItems;
            }

            if (ColumnRecordItems != null)
            {
                CommandDelete commandDelete = new CommandDelete();
                commandDelete.SetDestinationTable(tableName);
                commandDelete.SetRecordId(id);
                dbInfoItem?.Records.Remove(ColumnRecordItems);
                JsonDocument.Parse(RemoveCrc(SendAndReceive(
                    CraftQuery(commandDelete.BuildQuery()),
                    true)) ?? string.Empty);
            }
        }

        private List<ColumnRecordItem> GetRecordFromTable(string tableName, int? index, Int64? id)
        {
            List<ColumnRecordItem> record = new List<ColumnRecordItem>();
            JsonDocument? jsonElement = null;
            if (index != null)
                jsonElement = JsonDocument.Parse(RemoveCrc(SendAndReceive(
                    CraftQuery("{\"Table\":\"" + tableName + "\", \"Index\":" + index + ", \"Command\":\"DbRead\"}"),
                    true)) ?? string.Empty);
            if (id != null)
                jsonElement = JsonDocument.Parse(RemoveCrc(SendAndReceive(
                    CraftQuery("{\"Table\":\"" + tableName + "\", \"Id\":" + id + ", \"Command\":\"DbRead\"}"),
                    true)) ?? string.Empty);

            if (jsonElement?.RootElement.ValueKind == JsonValueKind.Object)
            {
                var dbInfoItem = DbInfos.FirstOrDefault(info =>
                    info.Table == jsonElement.RootElement.GetProperty("Table").ToString());

                if (dbInfoItem?.Items != null)
                    foreach (var column in dbInfoItem.Items)
                    {
                        var col = new ColumnRecordItem
                        {
                            Type = column.Type,
                            Tag = column.Tag,
                            Value = jsonElement.RootElement.GetProperty(column.DbName).ToString(),
                            Values = column.Values,
                            ValuesEn = column.ValuesEn,
                            Unit = column.Unit,
                            Min = column.Min,
                            Max = column.Max,
                            Name = column.Name,
                            NameEn = column.NameEn,
                            Alterability = column.Alterability,
                            DbName = column.DbName,
                            DecPlaces = column.DecPlaces,
                            Table = column.Table
                        };

                        if (!string.IsNullOrEmpty(col.Table)) GetForeigns(col);

                        record.Add(col);
                    }
            }

            return record;
        }

        private void GetForeigns(ColumnRecordItem foreignColumn)
        {
            #region Foreigns

            try
            {
                ListRecordsFromTable(foreignColumn.Table);

                if (foreignColumn.ForeignValues?.Count > 0)
                {
                    foreignColumn.ForeignValues.Clear();
                }

                if (foreignColumn.ForeignValues == null) foreignColumn.ForeignValues = new Dictionary<int, string>();
                var dbInfoItem = DbInfos.FirstOrDefault(info => info.Table == foreignColumn.Table)?.Records;

                if (dbInfoItem != null)
                    foreach (var columnRecordItems in dbInfoItem)
                    {
                        foreignColumn.ForeignValues.Add(
                            (int) columnRecordItems?.FirstOrDefault(item => item.DbName == "Id")?.Value,
                            (string) columnRecordItems?.FirstOrDefault(item => item.DbName == "Name")?.Value);
                    }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                //throw;
            }

            #endregion
        }

        //Send query and receive data from device       
        internal string? SendAndReceive(string query, bool canConsoleWrite)
        {
            if (canConsoleWrite)
            {
                Console.WriteLine("Sending query: \n" + query);
            }

            var respond = Sender?.SendAndWait(query);

            if (canConsoleWrite)
            {
                Console.WriteLine("Received response: \n" + respond);
                Console.WriteLine("------------------------\n");
            }

            return respond;
        }

// Performing operation after established connection with scale.
        private void InitializeConnection()
        {
            GetScaleInformation();
        }

// Performing disconnection with scale.
        internal void PerformDisconnection()
        {
            DisposeSender();
            Console.WriteLine("Disconnected");
            Console.WriteLine("------------------------\n");
        }

// Disposing sender.
        private void DisposeSender()
        {
            Sender.Dispose();
            Sender = null;
        }

// Getting information from scale.
        private void GetScaleInformation()
        {
            try
            {
                ScaleInfo = new WINFOCmd().Send(Sender);
                Console.WriteLine("Connected to: " + ScaleInfo.DeviceName);
                Console.WriteLine("DeviceType: " + ScaleInfo.DeviceType);
                Console.WriteLine("SerialNumber: " + ScaleInfo.SerialNumber);
                Console.WriteLine("------------------------\n");
            }
            catch (TimeoutException)
            {
            }
        }

        private string CalculateCrc(string query)
        {
            int crc = 0;
            foreach (var t in query)
            {
                crc ^= t;
            }

            return crc.ToString("X2");
        }

        private string? RemoveCrc(string? respond)
        {
            int index = respond.LastIndexOf('}');
            if (index == -1) return respond;
            int crlf = respond.IndexOf("\r", StringComparison.Ordinal);
            string crc = respond.Substring(index + 1, respond.Length - crlf);
            if (crc.Length > 0)
            {
                return respond.Remove(index + 1, respond.Length - crlf);
            }

            return respond;
        }

        private string CraftQuery(string query)
        {
            return query + CalculateCrc(query) + "\r\n";
        }

        private string? GetRecordAfterWrite(string response)
        {
            string? message = "ES";

            if (response == "ES\r\n") return string.Empty;
            try
            {
                var element = JsonDocument.Parse(response).RootElement;

                var table = element.GetProperty("Table").ToString();
                var id = element.GetProperty("Id").ToString();
                message = Sender.SendAndWait(CraftQuery("{\"Table\":\"" + table + "\", \"Id\":" + id +
                                                        ", \"Command\":\"DbRead\"}"));
            }
            catch
            {
                // ignored
            }

            if (message != null &&
                (message == "ES" || message.Contains("LackOfParam") || message.Contains("RecNotExist")))
                return "The operation has not been performed.\n";
            Console.WriteLine("Data modified / added correctly\n");
            return message;
        }

        private string GetPropertyNameAndValue<T>(Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException(
                    "You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name + ": " + propertyLambda.Compile().Invoke();
        }
    }
}