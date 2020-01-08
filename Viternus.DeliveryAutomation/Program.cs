using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using Viternus.Data;

namespace Viternus.DeliveryAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (DataConnector.Context = new ViternusEntities())
            //{
                DeliveryProcess delivery = new DeliveryProcess();
                delivery.Run();                
            //} 
        }
    }
}
