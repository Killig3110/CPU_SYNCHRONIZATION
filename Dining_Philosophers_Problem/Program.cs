using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dining_Philosophers_Problem
{
    public class Program
    {
        private static object[] chopsticks = new object[5]; // Mảng đũa, mỗi phần tử là một đối tượng monitor
        private static Random random = new Random();

        static void Main(string[] args)
        {
            // Khởi tạo các đối tượng monitor cho đũa
            for (int i = 0; i < 5; i++)
            {
                chopsticks[i] = new object();
            }

            // Khởi tạo và bắt đầu các triết gia
            Thread[] philosophers = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                philosophers[i] = new Thread(Philosopher);
                philosophers[i].Start(i); // Truyền index của triết gia
            }

            Console.ReadLine();
        }

        static void Philosopher(object index)
        {
            int philosopherIndex = (int)index;
            int leftChopstick = philosopherIndex;
            int rightChopstick = (philosopherIndex + 1) % 5; // Lấy phần dư để xác định đũa bên phải

            while (true)
            {
                // Thinking
                Console.WriteLine("Philosopher " + philosopherIndex + " is thinking.");
                Console.WriteLine();
                Thread.Sleep(random.Next(3000, 5000)); // Ngẫu nhiên thời gian suy nghĩ

                // Attempt to pick up chopsticks
                Console.WriteLine("Philosopher " + philosopherIndex + " is hungry and attempting to pick up chopsticks.");
                Console.WriteLine();

                lock (chopsticks[leftChopstick]) // Lock đũa bên trái
                {
                    Console.WriteLine("Philosopher " + philosopherIndex + " picked up left chopstick.");
                    Console.WriteLine();
                    lock (chopsticks[rightChopstick]) // Lock đũa bên phải
                    {
                        // Eating
                        Console.WriteLine("Philosopher " + philosopherIndex + " is eating.");
                        Console.WriteLine();
                        Thread.Sleep(random.Next(3000, 5000)); // Ngẫu nhiên thời gian ăn
                        Console.WriteLine("Philosopher " + philosopherIndex + " finished eating.");
                        Console.WriteLine();

                    } // Unlock đũa bên phải
                    Console.WriteLine("Philosopher " + philosopherIndex + " put down right chopstick.");
                    Console.WriteLine();
                } // Unlock đũa bên trái
                Console.WriteLine("Philosopher " + philosopherIndex + " put down left chopstick.");
                Console.WriteLine();

                // Back to thinking
            }
        }
    }
}
