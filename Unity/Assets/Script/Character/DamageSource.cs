using System.Collections.Generic;

namespace Game
{
    public class DamageSource
    {
        public List<IDamageSource> Sources { get; set; } = new List<IDamageSource>();
    }
}
