using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;

namespace weatherAppW
{
    public partial class Form1 : Form
    {
        
        const string APPID = "c78a2ff8f1d97904231712eb72361506";
        const string APPID2 = "542ffd081e67f4512b705f89d2a611b2";
        const string APPID3 = "6a7373153emsh663e8a64cb86476p186ed3jsn51fdaa953970";
        const string API_URL = "http://api.openweathermap.org/data/2.5/";
        const string LOCALE = "fr";
        const string ICONURL = "http://openweathermap.org/img/w/{0}.png";
        const string APIGEO_URL = "https://wft-geo-db.p.rapidapi.com";
        private Cities cities = null;
        private CultureInfo cultureInfo = new CultureInfo("fr-FR");
        public Form1()

        {
            InitializeComponent();
            Directory.CreateDirectory(@"c:\temp\weatherappimg");
       
        }

        private void GetWeather(string city, string regionCode, string countryCode)
        {
            using (WebClient web = new WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = string.Format("{0}/weather?q={1},{2},{3}&appid={4}&units=metric&cnt=20&lang={5}", API_URL, city, regionCode, countryCode, APPID, LOCALE);

                string json = web.DownloadString(url);

                WeatherInfo.Root outPut = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);

                lbl_cityName.Text = string.Format("{0}", outPut.Name);
                lbl_country.Text = string.Format("{0}", outPut.Sys.Country);
                lbl_temp.Text = string.Format("{0}\u00B0 C", (int)outPut.Main.Temp);

                //afichage du jour
                DateTimeOffset dt = DateTimeOffset.FromUnixTimeSeconds(outPut.Dt);
                string lbl_day_name = "lbl_day";
                Label lbl_day = Controls.Find(lbl_day_name, true).FirstOrDefault() as Label;
                lbl_day.Text = string.Format("{0} {1}", cultureInfo.DateTimeFormat.GetDayName(dt.ToLocalTime().DayOfWeek), dt.ToLocalTime().ToString("dd/MM/yyyy")); // timestamp, temps en millisecondes
                string img_path = string.Format(@"c:\temp\weatherappimg\{0}.png", outPut.Weather[0].Icon);

                //affiche la description météo
                string lbl_des_name = $"lbl_des";
                Label lbl_des = Controls.Find(lbl_des_name, true).FirstOrDefault() as Label;
                lbl_des.Text = string.Format("{0}", outPut.Weather[0].Description); // description météo

                //affiche la vitesse du vent
                string lbl_wind_name = $"lbl_wind";
                Label lbl_wind = Controls.Find(lbl_wind_name, true).FirstOrDefault() as Label;
                lbl_wind.Text = string.Format("{0} km/h", (int)(outPut.Wind.Speed * 3.6)); // vitesse du vent

                //affiche le % d'humidité
                string lbl_humid_name = $"lbl_humid";
                Label lbl_humid = Controls.Find(lbl_humid_name, true).FirstOrDefault() as Label;
                lbl_humid.Text = string.Format("{0} %", (int)outPut.Main.Humidity); // vitesse du vent

                //afichage icone de temps
                if (!File.Exists(img_path))
                {
                    web.DownloadFile(string.Format(ICONURL, outPut.Weather[0].Icon), img_path); // dl et enregitre l'image du temps
                }
                Bitmap image = new Bitmap(img_path);
                string img_icon_name = $"img_temp";
                PictureBox icon = Controls.Find(img_icon_name, true).FirstOrDefault() as PictureBox;
                icon.Image = image;
            }
        }

        private void GetForecast(string city, string regionCode, string countryCode)
        {
            using (WebClient web = new WebClient() { Encoding = Encoding.UTF8 })
            {
                int day = 6;
                string url = string.Format("{0}forecast/daily?q={1},{2},{3}&units=metric&cnt={4}&APPID={5}&lang={6}", API_URL, city, regionCode, countryCode, day, APPID2, LOCALE);
                string json = web.DownloadString(url);
                WeatherForecast forecast = JsonConvert.DeserializeObject<WeatherForecast>(json);

                
                for (int i = 1; i < forecast.List.Count; i++)
                {
                    DateTimeOffset dt = DateTimeOffset.FromUnixTimeSeconds(forecast.List[i].Dt);

                    string lbl_day_name = $"lbl_day_{i}";
                    Label lbl_day = Controls.Find(lbl_day_name, true).FirstOrDefault() as Label;
                    lbl_day.Text = string.Format("{0}", cultureInfo.DateTimeFormat.GetDayName(dt.ToLocalTime().DayOfWeek)); // timestamp, temps en millisecondes

                    string lbl_des_name = $"lbl_des_{i}";
                    Label lbl_des = Controls.Find(lbl_des_name, true).FirstOrDefault() as Label;
                    lbl_des.Text = string.Format("{0}", forecast.List[i].Weather[0].Description); // description météo

                    string lbl_temp_name = $"lbl_temp_{i}";
                    Label lbl_temp = Controls.Find(lbl_temp_name, true).FirstOrDefault() as Label;
                    lbl_temp.Text = string.Format("min : {0}\u00B0 C  max : {1}\u00B0 C", (int)forecast.List[i].Temp.Min, (int)forecast.List[i].Temp.Max); // température météo

                    string lbl_wind_name = $"lbl_wind_{i}";
                    Label lbl_wind = Controls.Find(lbl_wind_name, true).FirstOrDefault() as Label;
                    lbl_wind.Text = string.Format("{0} km/h", (int)(forecast.List[i].Speed * 3.6)); // vitesse du vent

                    string img_path = string.Format(@"c:\temp\weatherappimg\{0}.png", forecast.List[i].Weather[0].Icon);
                    if (!File.Exists(img_path))
                    {
                        web.DownloadFile(string.Format(ICONURL, forecast.List[i].Weather[0].Icon), img_path); // dl et enregitre l'image du temps
                    }
                    Bitmap image = new Bitmap(img_path);
                    string img_icon_name = $"img_temp{i}";
                    PictureBox icon = Controls.Find(img_icon_name, true).FirstOrDefault() as PictureBox;
                    icon.Image = image;

                }
                


            }
        }

        private void Btn_src_Click(object sender, EventArgs e)
        {
            using (WebClient web = new WebClient() { Encoding = Encoding.UTF8 })
            {


                TextBox textBox = Controls.Find("txb_1", true).FirstOrDefault() as TextBox;
                string url = string.Format("{0}/v1/geo/cities?limit=5&offset=0&namePrefix={1}", APIGEO_URL, textBox.Text);

                web.Headers.Add("x-rapidapi-key", APPID3);
                web.Headers.Add("x-rapidapi-host", "wft-geo-db.p.rapidapi.com");
                string json = web.DownloadString(url);
                cities = JsonConvert.DeserializeObject<Cities>(json);


                ListBox listbox = Controls.Find("lstb_1", true).FirstOrDefault() as ListBox;
                listbox.Items.Clear();
                for (int i = 0; i < cities.Data.Count; i++)
                {
                    listbox.Items.Add(cities.Data[i].City + ", " + cities.Data[i].Region + ", " + cities.Data[i].Country);
                }
            }
        }

        private void Btn_dsp_Click(object sender, EventArgs e)
        {
            ListBox listbox = Controls.Find("lstb_1", true).FirstOrDefault() as ListBox;
            if (listbox.SelectedItems.Count > 0)
            {
                string cityName = cities.Data[listbox.SelectedIndex].City;
                string regionCode = cities.Data[listbox.SelectedIndex].RegionCode;
                string countryCode = cities.Data[listbox.SelectedIndex].CountryCode;
                GetWeather(cityName, regionCode, countryCode); // meteo 1 jour
                GetForecast(cityName, regionCode, countryCode); // météo suivants
            }
        }
    }
}
 