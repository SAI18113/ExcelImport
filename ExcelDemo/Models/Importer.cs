namespace ExcelDemo.Models;

public class Importer
{
      public List<KeyValuePair<string, string>> Mappings;
    public List<T> Import<T>(string file)
    {
        try
        {
            int index = 0;
            string headerLine = "";
            List<T> list = new List<T>();
            List<string> lines = System.IO.File.ReadAllLines(file).ToList();
            foreach (var line in lines)
            {
                index++;
                if (line.Contains("ORIGIN"))
                {
                    headerLine = line;
                    break;
                }

            }

            var headerInfo = headerLine.Split(',').ToList().Select((v, i) => new { colValue = v, colIndex = i });




            Type type = typeof(T);
            var properties = type.GetProperties();




            var dataLines = lines.Skip(index);
            dataLines.ToList().ForEach(line => {
                var values = line.Split(',');
                T obj = (T)Activator.CreateInstance(type);



                //set values to obj parameters from csv columns
                foreach (var prop in properties)
                {
                    var mapping = Mappings.SingleOrDefault(m => m.Value == prop.Name);
                    var colName = mapping.Key;
                    var colIndex = headerInfo.SingleOrDefault(s => s.colValue == colName).colIndex;
                    var value = values[colIndex];
                    var propType = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(value, propType));
                }



                list.Add(obj);
            });




            return list;
        }
        catch(Exception)
        {
            throw;
        }
       
    }
}
