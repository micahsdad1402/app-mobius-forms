using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// Helper to do Recaptcha server-validation
// based on http://stackoverflow.com/questions/27764692/validating-recaptcha-2-no-captcha-recaptcha-in-asp-nets-server-side
// shouldn't really need any modifications, just leave this as is
public class Recaptcha
{
  public bool Validate(string EncodedResponse, string PrivateKey)
  {
    if(!(EncodedResponse is string) || String.IsNullOrEmpty(EncodedResponse as string)) 
      throw new Exception("recaptcha is empty");

    var client = new System.Net.WebClient();
    var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));
    var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Recaptcha>(GoogleReply);

    var status = captchaResponse.Success;

    if(!status)
      throw new Exception("bad recaptcha '" + status + "'" );

    return status;
  }

  [JsonProperty("success")]
  public bool Success
  {
    get { return m_Success; }
    set { m_Success = value; }
  }

  private bool m_Success;
  [JsonProperty("error-codes")]
  public List<string> ErrorCodes
  {
    get { return m_ErrorCodes; }
    set { m_ErrorCodes = value; }
  }


  private List<string> m_ErrorCodes;
}