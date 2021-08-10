using System;

namespace Simplified_driver_test_task
{
    class Program
    {

        static void Main(string[] args)
        {
            Driver d = new Driver();
            while (true)
            {
                d.executePacket(d.readPacket());
            }

        }

    }
}

