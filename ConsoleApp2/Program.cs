using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a queue
            // Using Queue class
            Queue my_queue = new Queue();

            // Adding elements in Queue
            // Using Enqueue() method
            my_queue.Enqueue("GFG");
            my_queue.Enqueue("Geeks");
            my_queue.Enqueue("lsbin");
            my_queue.Enqueue("geeks");
            my_queue.Enqueue("Geeks123");

            // Checking if the element is
            // present in the Queue or not
            if (my_queue.Contains("lsbin") == true)
            {
                Console.WriteLine("Element available...!!");
            }
            else
            {
                Console.WriteLine("Element not available...!!");
            }
        }
    }
}
