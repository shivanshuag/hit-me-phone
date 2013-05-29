using System;
using System.Collections.Generic;

namespace SocketsXO_Server
{
    public static class GameLogic
    {
        static Random _rnd;
        public static string Play(string incoming)
        {
            if (!String.IsNullOrWhiteSpace(incoming))
            {
                incoming = incoming.Trim("\0".ToCharArray());
                string[] pairs = incoming.Split('|');

                string playAs = pairs[0];
                List<string> list = new List<string>();
                for (int i = 1; i < pairs.Length; i++)
                {
                    string[] pair = pairs[i].Split('*');

                    if (String.IsNullOrWhiteSpace(pair[1]))
                    {
                        list.Add(pair[0]);
                    }

                }

                if (_rnd == null)
                    _rnd = new Random(DateTime.Now.Millisecond);


                return list.Count > 0 ? list[_rnd.Next(0, list.Count)] : String.Empty;
            }

            return String.Empty;


        }
    }
}
