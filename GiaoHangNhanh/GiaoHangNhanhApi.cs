using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GiaoHangNhanh
{
    class GiaoHangNhanhApi
    {
        private const string url = @"https://console.ghn.vn/api/v1/apiv3/";
        private const string url_dev = @"https://dev-online-gateway.ghn.vn/apiv3-api/api/v1/apiv3/";
        private const string token = "5eda1fa700e71a0f1b68d271";
        private const string jsonToken = "{\"token\": \"" + token + "\"}";
        private const string email = "duong.thuanvietsoft@gmail.com";
        private const string password = "Duong1998";
        //parse Json to model
        public static T parseJson<T>(string _data)
        {
            JsonSerializer s = new JsonSerializer();
            return s.Deserialize<T>(new JsonTextReader(new StringReader(_data)));
        }

        //create request
        public static string httpRequest(string _url, string _data)
        {
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(url + _url);
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            using (StreamWriter streamWriter = new StreamWriter(http.GetRequestStream()))
            {
                streamWriter.Write(_data);
            }
            try
            {
                using (StreamReader streamReader = new StreamReader(http.GetResponse().GetResponseStream()))
                {
                    _data = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _data = "";
            }
            return _data;
        }

        //check login
        public static bool CheckLogin(ref string error)
        {
            LoginRequest model = new LoginRequest();
            model.token = token;
            model.Email = email;
            model.Password = password;
            String dataJson = JsonConvert.SerializeObject(model);
            dataJson = httpRequest("SignIn", dataJson);
            if (dataJson == "")
            {
                error = "Thông tin đăng nhập sai";
                return false;
            }
            LoginResponse rs = parseJson<LoginResponse>(dataJson);
            error = "";
            if (rs.code == 1) return true;
            else
            {
                error = rs.msg;
                return false;
            }
        }

        //get district
        public static List<District> getDistricts(ref string error)
        {
            List<District> lstResult = new List<District>();
            string dataJson = httpRequest("GetDistricts", jsonToken);
            if (dataJson == "")
            {
                error = "Token không hợp lệ";
                return lstResult;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
            }
            else
            {
                JArray jArray = (JArray)jObject["data"];
                foreach (JToken item in jArray)
                {
                    District model = parseJson<District>(item.ToString());
                    lstResult.Add(model);
                }
            }
            return lstResult;
        }

        //get wards
        public static List<Ward> getWardsP(decimal districtId, ref string error)
        {
            List<Ward> lstResult = new List<Ward>();
            WardRequest modelRequest = new WardRequest();
            modelRequest.token = token;
            modelRequest.DistrictID = districtId;
            string dataJson = httpRequest("GetWards", JsonConvert.SerializeObject(modelRequest));
            if (dataJson == "")
            {
                error = "Token không hợp lệ";
                return lstResult;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
            }
            else
            {
                JArray jArray = (JArray)jObject["data"];
                foreach (JToken item in jArray)
                {
                    Ward model = parseJson<Ward>(item.ToString());
                    lstResult.Add(model);
                }
            }
            return lstResult;
        }

        //get hubs
        public static List<Hub> getHubs(ref string error)
        {
            List<Hub> lstResult = new List<Hub>();
            string dataJson = httpRequest("GetHubs", jsonToken);
            if (dataJson == "")
            {
                error = "Token không hợp lệ";
                return lstResult;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
            }
            else
            {
                JArray jArray = (JArray)jObject["data"];
                foreach (JToken item in jArray)
                {
                    Hub model = parseJson<Hub>(item.ToString());
                    lstResult.Add(model);
                }
            }
            return lstResult;
        }

        //Calculate fee
        public static FeeResponse calculateFee(CalculateFeeRequest cal, ref string error)
        {
            string dataJson = JsonConvert.SerializeObject(cal);
            dataJson = httpRequest("CalculateFee", dataJson);
            if (dataJson == "")
            {
                error = "[GHN-ERR80] Giá trị truyền vào không hợp lệ";
                return null;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
                return null;
            }
            else
            {
                FeeResponse model = new FeeResponse();
                model = parseJson<FeeResponse>(jObject["data"].ToString());
                return model;
            }
        }

        //Find Available Services
        public static List<AvailableServices> findAvailableServices(AvailableServicesRequest param, ref string error)
        {
            List<AvailableServices> lstResult = new List<AvailableServices>();
            string dataJson = JsonConvert.SerializeObject(param);
            dataJson = httpRequest("FindAvailableServices", dataJson);
            if (dataJson == "")
            {
                error = "[GHN-ERR80] Giá trị truyền vào không hợp lệ";
                return lstResult;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
            }
            else
            {
                AvailableServices model = new AvailableServices();
                model = parseJson<AvailableServices>(jObject["data"].ToString());
                lstResult.Add(model);
            }
            return lstResult;
        }

        public static OrderCreateResponse createOrder(OrderCreateRequest param, ref string error)
        {
            string dataJson = JsonConvert.SerializeObject(param);
            dataJson = httpRequest("CreateOrder", dataJson);
            if (dataJson == "")
            {
                error = "[GHN-ERR80] Giá trị truyền vào không hợp lệ";
                return null;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
                return null;
            }
            else
            {
                OrderCreateResponse model = new OrderCreateResponse();
                model = parseJson<OrderCreateResponse>(jObject["data"].ToString());
                return model;
            }
        }

        public static OrderInfo orderInfo(string OrderCode, ref string error)
        {
            string dataJson = "{\"token\": \"" + token + "\", \"OrderCode\": \"" + OrderCode + "\"}";
            dataJson = httpRequest("OrderInfo", dataJson);
            if (dataJson == "")
            {
                error = "Hệ thống đã xảy ra lỗi (Mã lỗi [Business_Error][8a8affb9-592e-4767-9a36-eb2cf14648e0]Nullable object must have a value";
                return null;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
                return null;
            }
            else
            {
                OrderInfo model = new OrderInfo();
                model = parseJson<OrderInfo>(jObject["data"].ToString());
                return model;
            }
        }

        public static OrderUpdateResponse orderUpdate(OrderUpdateRequest param, ref string error)
        {
            string dataJson = JsonConvert.SerializeObject(param);
            dataJson = httpRequest("UpdateOrder", dataJson);
            if (dataJson == "")
            {
                error = "Tham số truyền vào lỗi!";
                return null;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
                return null;
            }
            else
            {
                OrderUpdateResponse model = new OrderUpdateResponse();
                model = parseJson<OrderUpdateResponse>(jObject["data"].ToString());
                return model;
            }
        }

        public static bool orderCancel(string OrderCode, ref string error)
        {
            string dataJson = "{\"token\": \"" + token + "\", \"OrderCode\": \"" + OrderCode + "\"}";
            dataJson = httpRequest("CancelOrder", dataJson);
            if (dataJson == "")
            {
                error = "Tham số truyền vào lỗi!";
                return false;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool orderReturn(string OrderCode, ref string error)
        {
            string dataJson = "{\"token\": \"" + token + "\", \"OrderCode\": \"" + OrderCode + "\"}";
            dataJson = httpRequest("CancelOrder", dataJson);
            if (dataJson == "")
            {
                error = "Tham số truyền vào lỗi!";
                return false;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
                return false;
            }
            else
            {
                return true;
            }
        }

        public static decimal hubAdd(HubAddRequest param, ref string error)
        {
            string dataJson = JsonConvert.SerializeObject(param);
            dataJson = httpRequest("AddHubs", dataJson);
            if (dataJson == "")
            {
                error = "Token không đúng";
                return 0;
            }
            decimal rs = 0;
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
            }
            else
            {
                jObject = JObject.Parse(jObject["data"].ToString());
                rs = Convert.ToDecimal(jObject["HubID"].ToString());
            }
            return rs;
        }

        public static bool hubUpdate(HubUpdateRequest param, ref string error)
        {
            string dataJson = JsonConvert.SerializeObject(param);
            dataJson = httpRequest("UpdateHubs", dataJson);
            if (dataJson == "")
            {
                error = "Token không đúng";
                return false;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
                return false;
            }
            else
            {
                return true;
            }
        }

        public static List<Log> GetOrderLogs(OrderLogsRequest param, ref string error)
        {
            List<Log> lstResult = new List<Log>();
            string dataJson = JsonConvert.SerializeObject(param);
            dataJson = httpRequest("UpdateHubs", dataJson);
            if (dataJson == "")
            {
                error = "Token không đúng";
                return lstResult;
            }
            JObject jObject = JObject.Parse(dataJson);
            if (Convert.ToDecimal(jObject["code"].ToString()) == 0)
            {
                error = jObject["msg"].ToString();
            }
            else
            {
                JArray jArray = (JArray)jObject["data"].ToString();
                foreach (JToken it in jArray)
                {
                    Log model = parseJson<Log>(it.ToString());
                    lstResult.Add(model);
                }
            }
            return lstResult;
        }
    }
}