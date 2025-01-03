using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class TimeManager : Manager<TimeManager>
    {
        private Dictionary<object, float> timeScales = new Dictionary<object, float>();

        public float TimeScale => timeScales.Values.Aggregate(1f, (a, b) => a * b);

        public void SetTimeScale(object key, float timeScale)
        {
            timeScales[key] = timeScale;
            Refresh();
        }

        public void ClearTimeScale(object key)
        {
            timeScales.Remove(key);
            Refresh();
        }

        public void Refresh()
        {
            Time.timeScale = TimeScale;
        }
    }
}
