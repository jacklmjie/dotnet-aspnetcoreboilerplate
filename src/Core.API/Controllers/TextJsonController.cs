using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
using System.Text;

namespace Core.API.Controllers
{
    /// <summary>
    /// https://www.cnblogs.com/waku/p/11026630.html
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TextJsonController : ControllerBase
    {
        [HttpGet("get")]
        public void Get()
        {
            var model = new WeatherForecast()
            {
                Date = DateTime.Now,
                TemperatureC = 10,
                Summary = "注释"
            };
            var json = Serialize(model);
            var json2 = SerializePrettyPrint(model);
            var mode2 = Deserialize(json);
            var list = new List<WeatherForecast>();
            list.Add(model);
            list.Add(mode2);
            var jsonList = JsonSerializer.Serialize<List<WeatherForecast>>(list);
            var averageTemp = ComputeAverageTemperatures(jsonList);
            Write();
            Read(json);
        }

        string Serialize(WeatherForecast value)
        {
            //生成缩小的json
            return JsonSerializer.Serialize<WeatherForecast>(value);
        }

        string SerializePrettyPrint(WeatherForecast value)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            //生成易读的json
            return JsonSerializer.Serialize<WeatherForecast>(value, options);
        }

        WeatherForecast Deserialize(string json)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };
            //允许后面的逗号
            return JsonSerializer.Deserialize<WeatherForecast>(json, options);
        }

        /// <summary>
        /// 读取属性
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        double ComputeAverageTemperatures(string json)
        {
            var options = new JsonDocumentOptions
            {
                AllowTrailingCommas = true
            };

            using (JsonDocument document = JsonDocument.Parse(json, options))
            {
                int sumOfAllTemperatures = 0;
                int count = 0;

                foreach (JsonElement element in document.RootElement.EnumerateArray())
                {
                    DateTimeOffset date = element.GetProperty("Date").GetDateTimeOffset();

                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        int temp = element.GetProperty("temp").GetInt32();
                        sumOfAllTemperatures += temp;
                        count++;
                    }
                }

                var averageTemp = (double)sumOfAllTemperatures / count;
                return averageTemp;
            }
        }

        /// <summary>
        /// 写入
        /// </summary>
        void Write()
        {
            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartObject();
                    writer.WriteString("date", DateTimeOffset.UtcNow);
                    writer.WriteNumber("temp", 42);
                    writer.WriteEndObject();
                }

                string json = Encoding.UTF8.GetString(stream.ToArray());
                Console.WriteLine(json);
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="json"></param>
        void Read(string json)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);
            Utf8JsonReader reader = new Utf8JsonReader(data, isFinalBlock: true, state: default);

            while (reader.Read())
            {
                Console.Write(reader.TokenType);

                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                    case JsonTokenType.String:
                        {
                            string text = reader.GetString();
                            Console.Write(" ");
                            Console.Write(text);
                            break;
                        }

                    case JsonTokenType.Number:
                        {
                            int value = reader.GetInt32();
                            Console.Write(" ");
                            Console.Write(value);
                            break;
                        }

                        // Other token types elided for brevity
                }

                Console.WriteLine();
            }
        }
    }

    class WeatherForecast
    {
        public DateTimeOffset Date { get; set; }

        // 总是使用摄氏度
        [JsonPropertyName("temp")]
        public int TemperatureC { get; set; }

        // 不序列化这个属性
        [JsonIgnore]
        public string Summary { get; set; }
    }
}