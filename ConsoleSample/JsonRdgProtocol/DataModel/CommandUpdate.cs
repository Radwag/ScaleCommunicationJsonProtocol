using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class CommandUpdate
    {
        private string Command = "DbUpdate";
        private string Table { get; set; }
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
                query.Append(", \"" + objectModel.DbName + "\":\"" + objectModel.Value + "\"");
            }

            query.Append("}");
            return query.ToString();
        }
    }
}