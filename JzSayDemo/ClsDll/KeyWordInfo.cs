using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JzSayDemo.ClsDll
{
    /*
        string introText = this.GetPostStr("introText");
        if (introText.IsNullOrEmpty()) return;

        List<KeyWordsLib> dits;
        using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
        {
            db.ObjectTrackingEnabled = false;
            dits = db.KeyWordsLib.OrderByDescending(x => x.KeyWeight).ThenByDescending(x => x.KeyLength).ToList();
        }

        string t = new KeyWordInfo().xxx(introText, dits);
        Response.Write(t);
     */

    /// <summary>
    /// 
    /// </summary>
    public class KeyWordInfo
    {
        public string KeyWord { get; set; }
        public Int32 PosStart { get; set; }
        public Int32 PosEnd { get; set; }

        List<KeyWordInfo> FindKeys = new List<KeyWordInfo>();

        Int32 FindPos(string txt, string keyWord)
        {
            Int32 wmx = 0;
            Int32 ins = 0, idy = 0, ixy = 0;
            ins = txt.IndexOf(keyWord);
            while (-1 != ins && wmx++ < 500)
            {
                idy = txt.IndexOf(">", ins); // <br/>kw<img src='' alt="kw" />kw<br/>
                if (idy == -1) //没有大于认为不在标签里面
                {
                    if (this.FindKeys.Any(x => x.PosStart <= ins && x.PosEnd >= ins) == false) //没有在已有关键词范围内
                    {
                        return ins;
                    }
                    else //在已有关键词范围内了
                    {
                        ins = txt.IndexOf(keyWord, ins + keyWord.Length);
                        continue;
                    }
                }
                else
                {
                    ixy = txt.IndexOf("<", ins);
                    if (ixy == -1) //有大于没小于认为在标签里面
                    {
                        return -1;
                    }
                    else
                    {
                        if (idy > ixy)
                        {
                            if (this.FindKeys.Any(x => x.PosStart <= ins && x.PosEnd >= ins) == false) //没有在已有关键词范围内
                            {
                                return ins;
                            }
                            else //在已有关键词范围内了
                            {
                                ins = txt.IndexOf(keyWord, idy);
                                continue;
                            }
                        }
                        else
                        {
                            ins = txt.IndexOf(keyWord, idy);
                            continue;
                        }
                    }
                }
            }
            return -1;
        }

        List<KeyWordInfo> Parse(string txt, List<KeyWordsLib> dits)
        {
            this.FindKeys.Clear();
            Int32 ins = -1;
            foreach (var kw in dits)
            {
                ins = this.FindPos(txt, kw.KeyWord);
                if (-1 == ins) continue;
                this.FindKeys.Add(new KeyWordInfo() { KeyWord = kw.KeyWord, PosStart = ins, PosEnd = ins + kw.KeyLength });
            }
            return this.FindKeys;
        }

        public string xxx(string txt, List<KeyWordsLib> dits)
        {
            List<KeyWordInfo> fkws = this.Parse(txt, dits).OrderBy(x => x.PosStart).ToList();
            if (fkws.Count == 0) return txt;

            List<string> js = new List<string>();
            if (fkws[0].PosStart > 0) js.Add(txt.Substring(0, fkws[0].PosStart) + ""); //头
            for (int i = 0, j = fkws.Count - 1; i < j; i++)
            {
                var c = fkws[i];
                var h = fkws[i + 1];
                js.Add("[" + c.KeyWord + "]" + txt.Substring(c.PosStart, h.PosStart - c.PosStart).Substring(c.KeyWord.Length) + "");
            }
            js.Add("[" + fkws[fkws.Count - 1].KeyWord + "]" + txt.Substring(fkws[fkws.Count - 1].PosStart).Substring(fkws[fkws.Count - 1].KeyWord.Length));

            return string.Join("", js);
        }

    }

}