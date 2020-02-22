using SyndriaServer.Models;

namespace SyndriaServer.Interface
{
    public interface IAiObject : IAttackableObject
    {
        HeroData heroData { get; }
        //List<SpellData> spells { get; }

        void OnKill(IAttackableObject killed);
    }
}
