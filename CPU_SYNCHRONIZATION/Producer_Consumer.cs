using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_SYNCHRONIZATION
{
    public class Producer_Consumer
    {
        public static int mutex = 1,  // cơ chế đồng bộ hóa đảm bảo rằng chỉ có một tiến trình có thể truy cập vào Critical Section tại một thời điểm
                    full = 0,  // kiểm tra buffer có đầy hay không, full = 0 nghĩa là buffer đang trống
                    empty = 3, // buffer được thiết lập ban đầu với khả năng chứa tối đa 3 mục
                    x = 0; // đếm số mục trong buffer
        public static void Main(String[] args )
        {
            int n;
            producer();
            consumer();
            Console.WriteLine("MENU");
            Console.WriteLine("1. Producer");
            Console.WriteLine("2. Consumer");
            Console.WriteLine("3. Exit");

            while (true)
            {
                Console.Write("Enter your choice: ");
                n = Convert.ToInt32(Console.ReadLine());
                switch (n)
                {
                    case 1:
                        if ((mutex == 1) && (empty != 0))
                            producer();
                        else
                            Console.WriteLine("Buffer is full");
                        break;
                    case 2:
                        if ((mutex == 1) && (full != 0))
                            consumer();
                        else
                            Console.WriteLine("Buffer is empty");
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static int wait(int s)
        {
            return (--s);
        }

        private static int signal(int s)
        {
            return (++s);
        }

        private static void producer()
        {
            mutex = wait(mutex);
            full = signal(full);
            empty = wait(empty);
            x++;
            Console.WriteLine("Producer produces the item " + x);
            mutex = signal(mutex);
        }

        private static void consumer()
        {
            mutex = wait(mutex);
            full = wait(full);
            empty = signal(empty);
            Console.WriteLine("Consumer consumes item " + x);
            x--;
            mutex = signal(mutex);
        }
    }
}
