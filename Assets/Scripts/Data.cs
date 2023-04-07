public static class Data
{
    private struct Info
    {
        public static int SCORE;
    }
    private struct EnemyKilled
    {
        public static int BOAR, SNAIL, BEE;
    }

    public static void Reset()
    {
        Info.SCORE = 0;
        EnemyKilled.BEE = 0;
        EnemyKilled.BOAR = 0;
        EnemyKilled.SNAIL = 0;
        UIManager.Ins.UpdateScore(Info.SCORE);
    }

    public static void UpdateData(string field, int val)
    {
        string _field = field.ToLower();
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
        UIManager.Ins.UpdateScore(Info.SCORE);
    }

    public static int GetData(bool isInfo, string field)
    {
        int val = 0;
        string _field = field.ToLower();
        if (isInfo) val = Info.SCORE;
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