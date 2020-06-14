using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Jose;


namespace FindTrip_Web.Security
{
    public class JwtAuthUtil
    {
        public string GenerateToken(int id, string Email)
        {
            
            string secret = "myJwtAuthDemo";//加解密的key,如果不一樣會無法成功解密
            Dictionary<string, Object> payload = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            payload.Add("id", id);
            payload.Add("Email", Email);
            payload.Add("Exp", DateTime.Now.AddSeconds(Convert.ToInt32("10000")).ToString());//Token 時效設定100秒
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256);//產生token
            return token;
        }

        public string GetId(string Token)
        {
            string secret = "myJwtAuthDemo";//加解密的key,如果不一樣會無法成功解密
            var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                Token,
                Encoding.UTF8.GetBytes(secret),
                JwsAlgorithm.HS256);
            string id = jwtObject["id"].ToString();
            return id;
        }

    }
}