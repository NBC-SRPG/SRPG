using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/TicketData", fileName = "Ticket_")]
public class TicketSO : ItemData
{
    public GachaType gachaType;
    public TicketSO()
    {
        itemType = Constants.ItemType.Ticket;
    }
}
