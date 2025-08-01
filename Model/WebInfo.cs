namespace LogCap.Model;

public class WebInfo{
   public Int64 ID{get; set;}
   public String SiteDesc{get; set;} // allos.dev, cyapass.com, newlibre.com, etc.
   public String IpAddress{get; set;}
   public String RefUrl{get; set;}
   public String Info{get;set;}
   public DateTime Created {get; set;}

   public WebInfo(String siteDesc, String ipAddress, String refUrl=null, String info=null){
      SiteDesc = siteDesc;
      IpAddress = ipAddress;
      RefUrl = refUrl;
      Info = info;
      Created = DateTime.Now;
   }

}

