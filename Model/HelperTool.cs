using System.Text;
using System.Security.Cryptography;
namespace LogCap.Model;
/// <summary>
/// Provides methods which are called from numerous places within the WebAPI
/// </summary>
public static class HelperTool{
  public static string Hash(string value) 
  { 
     var sha = SHA256.Create();
     byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(value)); 
     return String.Concat(Array.ConvertAll(hash, x => x.ToString("X2"))); 
  }
}
