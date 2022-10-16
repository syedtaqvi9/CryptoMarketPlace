using Newtonsoft.Json;
using NftCardano.HelpingClasses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NftCardano.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Parity()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Explore()
        {
            List<GetNfts> nftlist = new List<GetNfts>();
            /*  string APIKey = "e8cf75f7924f4409a774df65673ceea4";*/  // 84e5e33730e64884994f3bbe9015c254
            string APIKey = "783498ac7b3f451ca430c544f5848853";
            string url = " https://api.nft-maker.io/ListProjects";
            string URL = url + "/" + APIKey;

            var client = new RestClient(URL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);
            List<ListProject> myDeserializedClass = JsonConvert.DeserializeObject<List<ListProject>>(response.Content.ToString());  // return 3 project 
            if (myDeserializedClass != null)
            {
                foreach (var lp in myDeserializedClass)
                {
                    string APIKeys = "783498ac7b3f451ca430c544f5848853";  //"e8cf75f7924f4409a774df65673ceea4";
                    string urls = "https://api.nft-maker.io/GetNfts";
                    string URLs = urls + "/" + APIKeys + "/" + lp.id + "/" + "sold"; // get list of all nfts from particular nft Id and state is free / Sold

                    var clients = new RestClient(URLs);
                    var requests = new RestRequest(Method.GET);
                    requests.AddHeader("Accept", "application/json");
                    IRestResponse nftresponse = clients.Execute(requests);
                    List<GetNfts> myGetNft = JsonConvert.DeserializeObject<List<GetNfts>>(nftresponse.Content.ToString());  // return 3 project 
                    for (int i = 0; i < myGetNft.Count(); i++)
                    {
                        myGetNft[i].parentid = lp.id;
                    }
                    if (myGetNft.Count() > 0)
                    {
                        //nftlist = myGetNft.ToList();
                        nftlist.AddRange(myGetNft);
                    }
                }
                ViewBag.NFTcount = nftlist.Count();
                ViewBag.NFT = nftlist;
            }
            else
            {
                ViewBag.NFTcount = 0;
            }
            return View();

        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Owner(string State = "", string hasToPay = "", string nftid = "", string nftlibId = "", string nftlibName = "", string nftlibImage = "", string paymentAddress = "", string expires = "", string adaToSend = "", string debug = "", string resultState = "", string errorMessage = "", string errorCode = "")
        {
            ViewBag.paymentAddress = paymentAddress;
            ViewBag.expires = expires;
            ViewBag.adaToSend = adaToSend;
            ViewBag.debug = debug;

            ViewBag.resultState = resultState;
            ViewBag.errorMessage = errorMessage;
            ViewBag.errorCode = errorCode;

            ViewBag.Nftid = nftid;
            ViewBag.NftlibId = nftlibId;
            ViewBag.NftlibName = nftlibName;
            ViewBag.NftlibImage = nftlibImage;

            ViewBag.State = State;
            ViewBag.hasToPay = hasToPay;

            return View(new RECaptcha());
        }

        [HttpPost]
        public ActionResult PostOwner(string nftlibId, string nftlibName, string nftlibImage, string paymentAddress, string button, int ada = -1, string nftprojectid = "17379", string countnft = "1")
        {
            if (button == "1")
            {
                // /GetAddressForSpecificNftSale/{apikey}/{nftprojectid}/{nftid}/{tokencount}/{lovelace}
                int lovelace = ada * (1000000);
                string APIKey = "783498ac7b3f451ca430c544f5848853";  //"84e5e33730e64884994f3bbe9015c254";  // 84e5e33730e64884994f3bbe9015c254
                string NFTProjectId = nftprojectid; // 19343
                string NftlibId = nftlibId; // 19343
                string tokencount = countnft;       // 1
                string Lovelace = Convert.ToString(lovelace);      // 7000000 and 7000000 Lovelace = 7 Ada
                string url = "https://api.nft-maker.io/GetAddressForSpecificNftSale";
                string URL = url + "/" + APIKey + "/" + NFTProjectId + "/" + NftlibId + "/" + tokencount + "/" + Lovelace;

                var client = new RestClient(URL);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                IRestResponse response = client.Execute(request);
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content.ToString());

                if (myDeserializedClass.resultState != null && myDeserializedClass.errorMessage != null && myDeserializedClass.errorCode != null)
                {
                    return RedirectToAction("Owner", "home", new { nftid = NFTProjectId, nftlibId = NftlibId, nftlibName = nftlibName, nftlibImage = nftlibImage, paymentAddress = paymentAddress, resultState = myDeserializedClass.resultState, errorMessage = myDeserializedClass.errorMessage, errorCode = myDeserializedClass.errorCode });
                }
                else
                {
                    return RedirectToAction("Owner", "home", new { nftid = NFTProjectId, nftlibId = NftlibId, nftlibName = nftlibName, nftlibImage = nftlibImage, paymentAddress = myDeserializedClass.paymentAddress, expires = myDeserializedClass.expires, adaToSend = myDeserializedClass.adaToSend, debug = myDeserializedClass.debug });
                }
            }
            else
            {  // /CheckAddress/{apikey}/{nftprojectid}/{address}
                string APIKey = "783498ac7b3f451ca430c544f5848853";  //"84e5e33730e64884994f3bbe9015c254";  // 84e5e33730e64884994f3bbe9015c254
                string Address = paymentAddress; // "addr1v8vnsrf46t2s0xe53fx9fvec4hafhzssezswkpt2ymazxcqsaewvu";
                string NFTProjectId = nftprojectid; // 19343
                string NftlibId = nftlibId; // 19343
                string url = "https://api.nft-maker.io/CheckAddress";
                string URL = url + "/" + APIKey + "/" + NFTProjectId + "/" + Address;

                var client = new RestClient(URL);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                IRestResponse response = client.Execute(request);
                Check myDeserializedClass = JsonConvert.DeserializeObject<Check>(response.Content.ToString());
                return RedirectToAction("Owner", "home", new { State = myDeserializedClass.state, hasToPay = myDeserializedClass.hasToPay, nftid = NFTProjectId, nftlibId = NftlibId, nftlibName = nftlibName, nftlibImage = nftlibImage, paymentAddress = paymentAddress });
            }
        }

        [HttpPost]
        public ActionResult buynft(int Price)
        {
            // /GetAddressForRandomNftSale/{apikey}/{nftprojectid}/{countnft}/{lovelace}
            int lovelace = Price * (1000000);
            string APIKey = "783498ac7b3f451ca430c544f5848853"; //"84e5e33730e64884994f3bbe9015c254";  // 84e5e33730e64884994f3bbe9015c254
            string NFTProjectId = "17379"; //"19994"; // 19343
            string tokencount = "1";       // 1
            string Lovelace = Convert.ToString(lovelace);      // 7000000 and 7000000 Lovelace = 7 Ada
            string url = "https://api.nft-maker.io/GetAddressForRandomNftSale";
            string URL = url + "/" + APIKey + "/" + NFTProjectId + "/" + tokencount + "/" + lovelace;

            var client = new RestClient(URL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content.ToString());

            Root obj = new Root()
            {
                adaToSend = myDeserializedClass.adaToSend,
                debug = myDeserializedClass.debug,
                errorCode = myDeserializedClass.errorCode,
                errorMessage = myDeserializedClass.errorMessage,
                expires = myDeserializedClass.expires.Date,
                paymentAddress = myDeserializedClass.paymentAddress,
                resultState = myDeserializedClass.resultState
            };

            return new JsonResult()
            {
                Data = obj,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [HttpPost]
        public ActionResult checknft(string PaymentAddress)
        {
            // / CheckAddress /{ apikey}/{ nftprojectid}/{ address}
            string APIKey = "783498ac7b3f451ca430c544f5848853";  //"84e5e33730e64884994f3bbe9015c254";  // 84e5e33730e64884994f3bbe9015c254
            string NFTProjectId = "17379"; //"19994"; // 19343
            string paymentAddress = PaymentAddress;       // 1
            string url = "https://api.nft-maker.io/CheckAddress";
            string URL = url + "/" + APIKey + "/" + NFTProjectId + "/" + paymentAddress;

            var client = new RestClient(URL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);
            Check myDeserializedClass = JsonConvert.DeserializeObject<Check>(response.Content.ToString());

            Check obj = new Check()
            {
                expiresDateTime = myDeserializedClass.expiresDateTime.Date,
                hasToPay = myDeserializedClass.hasToPay/ (1000000),
                errorCode = myDeserializedClass.errorCode,
                errorMessage = myDeserializedClass.errorMessage,
                resultState = myDeserializedClass.resultState,
                lovelace = myDeserializedClass.hasToPay,
                payDateTime = myDeserializedClass.resultState,
                reservedNft = myDeserializedClass.reservedNft,
                senderAddress = myDeserializedClass.senderAddress,
                state = myDeserializedClass.state,
                transaction = myDeserializedClass.transaction,
            };

            return new JsonResult()
            {
                Data = obj,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [HttpPost]
        public JsonResult AjaxMethod(string response)  //The following Action method handles AJAX calls and hence the return type is set to JsonResult.
        {
            RECaptcha recaptcha = new RECaptcha();
            string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + recaptcha.Secret + "&response=" + response;
            recaptcha.Response = (new WebClient()).DownloadString(url);
            return Json(recaptcha);
        }
    }
}
