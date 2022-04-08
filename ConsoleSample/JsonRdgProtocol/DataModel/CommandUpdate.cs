using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class CommandUpdate
    {
        private string Command = "DbUpdate";
        private string Table { get; set; }
        private object Type { get; set; }
        private Int64 Id { get; set; }
        private List<ObjectModel> ObjectsList = new List<ObjectModel>();

        public void SetDestinationTable(string table)
        {
            Table = table;
        }

        public void SetRecordId(Int64 id)
        {
            Id = id;
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
            query.Append(", \"Id\":" + Id);
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