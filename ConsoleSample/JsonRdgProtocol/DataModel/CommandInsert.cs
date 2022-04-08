using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class CommandInsert
    {
        private string Command = "DbInsert";
        private string Table { get; set; }
        private object Type { get; set; }
        
        private List<ObjectModel> ObjectsList = new List<ObjectModel>();

        public void SetDestinationTable(string table)
        {
            Table = table;
        }

        public void SetType(object type)
        {
            Type = type;
        }
        
        public void AddObject(string dbName, object value)
        {
            ObjectModel @object = new ObjectModel
            {
                DbName = dbName,
                Value = value
            };
            ObjectsList.Add(@object);
        }

        public string BuildQuery()
        {
            StringBuilder query = new StringBuilder();
            query.Append("{");
            query.Append("\"Command\":\"" + Command + "\"");
            query.Append(", \"Table\":\"" + Table + "\"");
            foreach (var objectModel in ObjectsList)
            {
                string value;
                if (!Type.ToString().Equals("7") && !Type.ToString().Equals("3"))
                {
                    value = "\":\"" + objectModel.Value + "\"";
                }else
                try
                {
                    Convert.ToDecimal(objectModel.Value);
                    value = "\":" + objectModel.Value;
                }
                catch
                {
                    value = "\":\"" + objectModel.Value + "\"";
                }
                query.Append(", \"" + objectModel.DbName + value);
            }

            query.Append("}");
            return query.ToString();
        }
    }
}