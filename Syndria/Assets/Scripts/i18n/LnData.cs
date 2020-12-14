using System.Collections.Generic;

public class LnData
{
    private Dictionary<string, string> data;
    private Language lang;

    public Language Lang => this.lang;

    public LnData(Language lang)
    {
        this.data = new Dictionary<string, string>();
        this.lang = lang;
    }

    public void Marge(string key, string value)
    {
        if (this.data.ContainsKey(key))
            this.data[key] = value;
        else
            this.data.Add(key, value);
    }

    public string GetString(string key) => this.data.ContainsKey(key) ? this.data[key] : (string)null;
}