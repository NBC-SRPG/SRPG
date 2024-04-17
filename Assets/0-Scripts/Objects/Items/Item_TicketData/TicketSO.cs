using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/TicketData", fileName = "ItemSO_")]
public class TicketSO : ItemSO
{
    //public GachaType gachaType;
    public TicketSO()
    {
        itemType = Constants.ItemType.Ticket;
    }
}
