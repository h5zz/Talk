using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCommand : ICommand
{
    public virtual void Execute(IMessage message)
    {
       // AppStartUpCommand.Instance.AddManager<GameManager>("GameManager");


    }
   
}
