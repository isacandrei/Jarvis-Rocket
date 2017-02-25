using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeatherAssignment.DataClasses;

namespace WeatherAssignment
{

   //Singleton
  public class WeatherDataServiceFactory: IWeatherDataService
    {
        private static WeatherDataServiceFactory instance;

        private WeatherDataServiceFactory() { }

        public static WeatherDataServiceFactory Instance
        {
            get
            {
                if (instance == null)                                   //return new instance if not created yet
                {
                    instance = new WeatherDataServiceFactory();
                }

                return instance;
            }
        }

        public CityWeather GetWeatherDataService(Location location)
        {

            WebClient client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;

            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + location.city + "&appid=9a5691304ab424cb92980af75b143725";        //url with location & app id

            string json;

            try
            {
                json = client.DownloadString(url);                                          //download json according to url
            }catch(Exception e)
            {
                throw new WeatherDataServiceException(e + "Internet Connection failed.");
            }

            JObject jobject = JObject.Parse(json);
           // System.Console.WriteLine(jobject);                                            //all the details from the api
            var obj = JsonConvert.DeserializeObject<CityWeather>(json);
            return obj;                                                                     //return weather data object
        }
    }
}
