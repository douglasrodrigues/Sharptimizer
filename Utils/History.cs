namespace Optimizer.Utils
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Text;
    using System.Text.Json;
    
    public class History : DynamicObject
    {
        public Dictionary<string, object> DynamicProperties = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            DynamicProperties.Add(binder.Name, value);

            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return DynamicProperties.TryGetValue(binder.Name, out result);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var property in DynamicProperties)
            {
                sb.AppendLine($"Property '{property.Key}' = '{property.Value}'");
            }

            return sb.ToString();
        }

        public string Dump()
        {
            string json = JsonSerializer.Serialize(DynamicProperties);

            return json;
        }

        public void Save(string path)
        {
            using (var tw = new StreamWriter(path, false))
            {
                tw.Write(this.Dump().ToString());
                tw.Close();
            }
        }
        
        public void Load(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                DynamicProperties = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            }
        }
    }
}