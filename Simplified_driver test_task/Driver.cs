using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Simplified_driver_test_task
{
    enum Status
    {
        OK,
        IncompleteStream,
        UknownCommand,
        IncAmntOfParams
    }
    enum Commands
    {
        Text = 1,
        Sound = 2,

    }

    //Custom data type to define packet
    class Packet
    {
        //Enum of valid commands and amount of parameters
        


        public bool isValid { get; private set; }
        public Status status { get; private set; }
        public string packetStream { get; } // Whole packet start to end
        public string command { get; }
        public string[] parameters { get; }
        public Packet(string packetStream)
        {
            isValid = false;
            this.packetStream = packetStream;
            string[] words = packetStream.Trim('P', 'E').Split(':', ',');
            command = words[0];
            parameters = new string[words.Length - 1];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = words[i + 1];
            }
            isValid = validate();
        }
        public bool validate()   //Method checks if command is valid and correct amount of params is passed
        {
            if (!Regex.IsMatch(packetStream, "(^P[\x20-\x7e]*E$)"))
            {
                status = Status.IncompleteStream;
                return false;
            }
            else if (!Enum.IsDefined(typeof(Commands), command))
            {
                status = Status.UknownCommand;
                return false;
            }
            else if (parameters.Length != (int)Enum.Parse(typeof(Commands), command))
            {
                status = Status.IncAmntOfParams;
                return false;
            }
            else 
            {
                status = Status.OK;
                return true;
            } 
            

        }

    }
    class Driver
    {
        
        public Packet readPacket() //Method to parse packet. Although returns packet anyway, it warns about bad packets.
        {
            char c;
            string input = "";
            do//Read cahrs until 'E' is entered
            {
               c=Console.ReadKey().KeyChar;
                input += c;
            } while (c!='E');
            Packet p = new Packet(input);
            if (p.status == Status.IncompleteStream)  // Check packet for completion
            {
                Console.WriteLine("Packet is incomplete");

            }else
                Console.WriteLine();
            Console.WriteLine(p.isValid?"ACK":"NACK");
            return p;
        }
        public void executePacket(Packet p)
        {
            if (p.status == Status.OK) // make sure the packet is OK before execution
            { 
            GetType().GetMethod(p.command.ToLower()).Invoke(this,new[] { p }); //call method corresponding to command name
            }
            else
            {
                Console.WriteLine(p.status);
            }

        }
        public void text(Packet p)
        {
            Console.WriteLine(p.parameters[0]);
        }
        public void sound(Packet p)
        {
            Console.Beep(Convert.ToInt32(p.parameters[0]), Convert.ToInt32(p.parameters[1]));
        }
    }

}
