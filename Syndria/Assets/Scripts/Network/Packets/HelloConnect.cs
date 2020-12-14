using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HelloConnect : PacketBase
{
    public string msgHeader = "HC";

    public string google_id = "";
    public string version = "";

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
}