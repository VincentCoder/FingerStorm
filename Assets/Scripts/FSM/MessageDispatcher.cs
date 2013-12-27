using UnityEngine;

public class Telegram
{
    public BaseGameEntity Sender;

    public BaseGameEntity Reciver;

    public int Msg;

    public float DispatchTime;

    public MonoBehaviour _Behaviour;

    public Telegram ( float atime, BaseGameEntity asender, BaseGameEntity areceiver, int amsg, MonoBehaviour _Be )
    {
        this.Sender = asender;
        this.Reciver = areceiver;
        this.Msg = amsg;
        this.DispatchTime = atime;
        this._Behaviour = _Be;
    }

}

public class MessageDispatcher
{
    private static MessageDispatcher instance;
    private System.Collections.Generic.List<Telegram> PriorityQ = new System.Collections.Generic.List<Telegram>();

    private void Discharge ( BaseGameEntity pReceiver, Telegram telegram )
    {
        if (!pReceiver.HandleMessage(telegram))
        {
            Debug.LogError("Invalid Message!");
        }
    }

    public void DispatchMessage ( float delay, BaseGameEntity sender, BaseGameEntity receiver, int msg, MonoBehaviour _be )
    {
        if (sender == null || receiver == null)
        {
            Debug.LogError("Invalid Sender or Receiver!");
            return;
        }

        Telegram telegram = new Telegram(delay, sender, receiver, msg, _be);

        if (delay <= 0.0f)
        {
            this.Discharge(receiver, telegram);
        }
        else
        {
            float currentTime = Time.realtimeSinceStartup;
            telegram.DispatchTime = currentTime + delay;

            this.PriorityQ.ForEach(val =>
            {
                if (val.Sender == sender && val.Reciver == receiver && val.Msg == msg)
                {
                    //return;
                }
            });
            this.PriorityQ.Add(telegram);
        }
    }

    public void DispatchDelayedMessage ()
    {
        float currentTime = Time.realtimeSinceStartup;

        for (int i = 0; i < this.PriorityQ.Count; i++)
        {
            Telegram val = this.PriorityQ[i];
            if (val.DispatchTime < currentTime && val.DispatchTime > 0f)
            {
                this.Discharge(val.Reciver, val);
                this.PriorityQ.RemoveAt(i);
            }
        }
    }

    public static MessageDispatcher Instance ()
    {
        if (instance == null)
        {
            instance = new MessageDispatcher();
        }
        return instance;
    }
}

