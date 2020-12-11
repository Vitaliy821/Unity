using System.Collections.Generic;

public class WanderingTrader : VillagerController
{
    private List<ItemBaseController> Items { get; set; }

    public WanderingTrader(string _name) : base(_name)
    {
        this.Health = 100;
        this.Messages = new List<string> { "Hello", "Wanna trade?" };
        this.Name = _name;
        this.Role = "WanderingTrader";
    }

    private void Trade()
    {
        // trade with player
    }


    protected override void Attack() 
    {
    
    }
}
