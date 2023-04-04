public static class Data
{
    private struct Info
    {
        public static int SCORE, COIN;
    }
    private struct EnemyKilled
    {
        public static int BOAR, SNAIL, BEE;
    }

    public static void Reset()
    {
        Info.SCORE = 0;
        Info.COIN = 0;
        EnemyKilled.BEE = 0;
        EnemyKilled.BOAR = 0;
        EnemyKilled.SNAIL = 0;
    }

    public static void UpdateData(bool isInfo, string field, int val)
    {
        string _field = field.ToLower();
        if (isInfo)
        {
            switch (_field)
            {
                case TagConst.NameInfo.SCORE:
                    Info.SCORE += val;
                    break;
                case TagConst.NameInfo.COIN :
                    Info.COIN += val;
                    break;
            }
        }
        else
        {
            switch (_field)
            {
                case TagConst.NameEnemy.BOAR:
                    EnemyKilled.BOAR++;
                    break;
                case TagConst.NameEnemy.BEE:
                    EnemyKilled.BEE++;
                    break;
                case TagConst.NameEnemy.SNAIL :
                    EnemyKilled.SNAIL ++;
                    break;
            }

            Info.SCORE += val;
        }
    }

    public static int GetData(bool isInfo, string field)
    {
        int val = 0;
        string _field = field.ToLower();
        if (isInfo)
        {
            switch (_field)
            {
                case TagConst.NameInfo.SCORE:
                    val = Info.SCORE;
                    break;
                case TagConst.NameInfo.COIN :
                    val = Info.COIN;
                    break;
            }
        }
        else
        {
            switch (_field)
            {
                case TagConst.NameEnemy.BOAR:
                    val = EnemyKilled.BOAR;
                    break;
                case TagConst.NameEnemy.BEE:
                    val = EnemyKilled.BEE;
                    break;
                case TagConst.NameEnemy.SNAIL :
                    val = EnemyKilled.SNAIL;
                    break;
            }
        }
        return val;
    }
}