using System.Collections.Generic;

public class AnimalController : NPCBaseController
{
    public AnimalController(string _name)
    {
        this.Health = 100;
        this.Name = _name;
        this.Role = "Animal";
    }

    protected override void Move()
    {
        
    }
    
    protected override void Attack() 
    {
    
    }
  
}
