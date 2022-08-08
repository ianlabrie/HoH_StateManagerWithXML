using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public static class MinionLoaderXml
    {
        public static List<MinionXml> LoadData()
        {
            List<MinionXml> allStats = new List<MinionXml>();
            TextAsset minionStats = Resources.Load<TextAsset>("MinionStats");

            string[] data = minionStats.text.Split(new char[] { '\n' });
            for (int i = 0; i < data.Length - 1; i++)
            {
                string[] row = data[i].Split(new char[] { ',' });
                if (row[0] != "")
                {
                    MinionXml stats = new MinionXml();
                    stats.MinionType = row[0];
                    int.TryParse(row[1], out stats.Health);
                    int.TryParse(row[2], out stats.AttackRange);
                    int.TryParse(row[3], out stats.Damage);
                    allStats.Add(stats);
                }
            }

            return allStats;
        }
    }
}
