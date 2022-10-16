using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NftCardano.Controllers
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Root
    {
        public string paymentAddress { get; set; }
        public DateTime expires { get; set; }
        public string adaToSend { get; set; }
        public string debug { get; set; }
        public string resultState { get; set; }
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
    }
    
    public class Check
    {
        public string state { get; set; }
        public int lovelace { get; set; }
        public int hasToPay { get; set; }
        public object payDateTime { get; set; }
        public DateTime expiresDateTime { get; set; }
        public object transaction { get; set; }
        public object senderAddress { get; set; }
        public List<object> reservedNft { get; set; }
        public string resultState { get; set; }
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
    }

    public class ListProject
    {
        public int id { get; set; }
        public string projectname { get; set; }
        public string projecturl { get; set; }
        public string state { get; set; }
        public int free { get; set; }
        public int sold { get; set; }
        public int reserved { get; set; }
        public int total { get; set; }
    }

    public class GetNfts
    {
        public int id { get; set; }
        public int parentid { get; set; }
        public string name { get; set; }
        public object displayname { get; set; }
        public string ipfsLink { get; set; }
        public string gatewayLink { get; set; }
        public string state { get; set; }
        public bool minted { get; set; }
        public string policyId { get; set; }
        public string assetId { get; set; }
        public object assetname { get; set; }
        public object fingerprint { get; set; }
        public object initialMintTxHash { get; set; }
        public object series { get; set; }
        public int tokenamount { get; set; }
        public object price { get; set; }
    }

}