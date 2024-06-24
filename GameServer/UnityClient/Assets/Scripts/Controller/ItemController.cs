using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemController : CreatureController
{
    protected override void Init()
    {
    State = CreatureState.Moving;
    base.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActiveItem(other.gameObject);
        }
    }

    protected override void UpdateAnimation()
    {

    }
    
    protected void ActiveItem(GameObject obj)
    {
        MyPlayerController mc = obj.GetComponent<MyPlayerController>();

        C_ItemGet item = new C_ItemGet() { Iteminfo = new ItemInfo() };
        Debug.Log($"Creature {Id} Item Get {gameObject.name}");

        item.Iteminfo.ItemId = Id;
        item.Iteminfo.Name = "Coin";
        item.Iteminfo.PosInfo = PosInfo;
        item.Player = mc.Id;
        Managers.Network.Send(item);

        Debug.Log($"Creature {mc.Id} Item Get");
    }

}



