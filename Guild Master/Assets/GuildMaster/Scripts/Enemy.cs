public class Enemy
{
    public enum EnemyType { SKELETON, BANDIT, ORC, TOTAL };

    public string name;
    public uint lvl;
    public EnemyType type;
    public bool countered = false;

    public Enemy(EnemyType type, uint lvl = 1)
    {
        this.type = type;
        this.lvl = lvl;

        switch (type)
        {
            case EnemyType.SKELETON:
                name = "Skeleton";
                break;
            case EnemyType.BANDIT:
                name = "Bandit";
                break;
            case EnemyType.ORC:
                name = "Orc";
                break;
        }
    }
}
