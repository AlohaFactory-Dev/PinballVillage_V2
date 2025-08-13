// ActionContext 정의

public class ActionContext
{
    public IChanger Changer { get; private set; }
    public int Value { get; private set; }

    public ActionContext()
    {
    }

    public ActionContext(IChanger changer, int value)
    {
        Changer = changer;
        Value = value;
    }

    public ActionContext(IChanger changer)
    {
        Changer = changer;
        Value = 0; // 기본값으로 0 설정
    }

    public ActionContext(int value)
    {
        Changer = null; // 기본값으로 null 설정
        Value = value;
    }
}