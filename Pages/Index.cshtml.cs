using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;
using s3cr3tx.Models;
using System.Text.Json;

namespace s3cr3tx.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        public s3cr3tx.Models.S3cr3tx S3Cr3Tx;
        public s3cr3tx.Models.S3cr3txDbContext _s3cr3tx;
        public string _output;
        public IndexModel(s3cr3tx.Models.S3cr3txDbContext s3Cr3tx)
        {
            _s3cr3tx = s3Cr3tx;
            S3Cr3Tx = new S3cr3tx();
            _output = S3Cr3Tx.Output;
        }
        //public s3cr3tx.S3cr3tx S3Cr3Tx;

        public void OnGet([Bind("Email", "AuthCode", "Token", "EoD", "Input", "Output")] s3cr3tx.Models.S3cr3tx _S3Cr3Tx)
        {
            S3Cr3Tx = _S3Cr3Tx;
            WebClient wc = new WebClient();
            //wc.Credentials.GetCredential();


            wc.BaseAddress = @"https://localhost:7191/Values";
            WebHeaderCollection webHeader = new WebHeaderCollection();
            webHeader.Add(@"Email:" + S3Cr3Tx.Email);
            webHeader.Add(@"AuthCode:" + S3Cr3Tx.AuthCode);
            webHeader.Add(@"APIToken:" + S3Cr3Tx.Token);
            webHeader.Add(@"Input:" + S3Cr3Tx.Input);
            webHeader.Add(@"EorD:" + S3Cr3Tx.EoD);
            webHeader.Add(@"Def:" + @"z");

            wc.Headers = webHeader;
            string result = @"";
            if (S3Cr3Tx.AuthCode != S3Cr3Tx.Token)
            {
                result = wc.DownloadString(@"https://localhost:7191/Values");
            }
            S3Cr3Tx.Output = result;

        }
        public string Message { get; set; } = "";

        public class NewK
        {
            public string email { get; set; }
            public string pd { get; set; }
            public string pd2 { get; set; }
        }
        public void OnPostView([Bind("Email", "AuthCode", "Token", "EoD", "Input", "Output")] s3cr3tx.Models.S3cr3tx _S3Cr3Tx)
        {
            HttpRequest Request = HttpContext.Request;
            if (Request.Form.TryGetValue("S3Cr3Tx.Email", out Microsoft.Extensions.Primitives.StringValues vEmail))
            {
                if (Request.Form.TryGetValue("S3Cr3Tx.AuthCode", out Microsoft.Extensions.Primitives.StringValues vCode))
                {
                    if (Request.Form.TryGetValue("S3Cr3Tx.Token", out Microsoft.Extensions.Primitives.StringValues vToken))
                    {
                        if (Request.Form.TryGetValue("S3Cr3Tx.EoD", out Microsoft.Extensions.Primitives.StringValues vEoD))
                        {
                            if (Request.Form.TryGetValue("S3Cr3Tx.Input", out Microsoft.Extensions.Primitives.StringValues vInput))
                            {
                                WebClient wc = new WebClient();
                                //wc.Credentials.GetCredential();
                                wc.BaseAddress = @"https://localhost:7192/Values";
                                WebHeaderCollection webHeader = new WebHeaderCollection();
                                //webHeader.Add(@"content-type:text/plain; charset=utf-8");
                                webHeader.Add(@"accept:text/plain; charset=utf-8");
                                webHeader.Add(@"Email:" + vEmail[0]);
                                webHeader.Add(@"AuthCode:" + vCode[0]);
                                webHeader.Add(@"APIToken:" + vToken[0]);
                                webHeader.Add(@"Input:" + vInput[0]);
                                webHeader.Add(@"EorD:" + vEoD[0]);
                                webHeader.Add(@"Def:" + @"z");
                                
                                wc.Headers = webHeader;
                                string result = @"";
                                if (vCode[0] == vToken[0])
                                {
                                    webHeader.Add(@"content-type:application/json");
                                    NewK nk = new NewK();
                                    nk.email = vEmail[0];
                                    nk.pd = vToken[0];
                                    nk.pd2 = vCode[0];
                                    string strNk = JsonSerializer.Serialize<NewK>(nk);
                                    //string strUTF8 = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.GetEncoding(0), System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0).GetBytes(strNk)));
                                    //wc.upload
                                    result = wc.UploadString(@"https://localhost:7192/Values",strNk);
                                }
                                else
                                {
                                    result = wc.DownloadString(@"https://localhost:7192/Values");
                                }
                                    Message = result;
                                S3Cr3Tx.Output = result;
                                
                            }
                        }
                    }

                }
             }

            

        }
    }
}
