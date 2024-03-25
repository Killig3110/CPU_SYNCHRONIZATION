using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Readers_and_Writers_Problem
{
    public class ThreadInfo
    {
        public string type;
        public int index;

        public ThreadInfo(string type, int index)
        {
            this.type = type;
            this.index = index;
        }
    }
    public class Program
    {
        static Semaphore readLock = new Semaphore(1, 1); // Semaphore cho việc đọc, ban đầu là 1
        static Semaphore writeLock = new Semaphore(1, 1); // Semaphore cho việc ghi, ban đầu là 1
        static int readCount = 0; // Số lượng người đọc

        static ManualResetEvent startEvent = new ManualResetEvent(false); // Sự kiện bắt đầu

        static Random random = new Random();

        static void Main(string[] args)
        {
            int n; // Số lượng người đọc
            int m; // Số lượng người ghi

            Console.Write("Enter the number of readers: ");
            n = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the number of writers: ");
            m = Convert.ToInt32(Console.ReadLine());

            // Khởi tạo và khởi động các luồng cho người đọc
            for (int i = 0; i < n; i++)
            {
                Thread readerThread = new Thread(new ParameterizedThreadStart(ReaderWriter));
                readerThread.Start(new ThreadInfo("Reader", i));
            }

            // Khởi tạo và khởi động các luồng cho người viết
            for (int i = 0; i < m; i++)
            {
                Thread writerThread = new Thread(new ParameterizedThreadStart(ReaderWriter));
                writerThread.Start(new ThreadInfo("Writer", i));
            }

            startEvent.Set();

            Console.ReadLine();
        }
        static void ReaderWriter(object parameter)
        {
            startEvent.WaitOne(); // Đợi cho đến khi sự kiện bắt đầu được kích hoạt
            ThreadInfo info = (ThreadInfo)parameter; // Lấy thông tin của luồng từ tham số

            if (info.type == "Reader")
            {
                Reader(info.index); // Gọi phương thức Reader với index của luồng
            }
            else if (info.type == "Writer")
            {
                Writer(info.index); // Gọi phương thức Writer với index của luồng
            }
        }

        static void Writer(int index)
        {
            int waitTime = random.Next(3000); // Sử dụng một giá trị ngẫu nhiên để xác định thời gian chờ
            Thread.Sleep(waitTime); // Đợi một khoảng thời gian ngẫu nhiên

            writeLock.WaitOne(); // Đợi cho đến khi có thể ghi
            Console.WriteLine("Writer "+ index +" start writing ...");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Writer " + index + " is writing ...");
                Thread.Sleep(1000);
                if (i == 4)
                {
                    Console.WriteLine("Writer " + index + " has finished writing.");
                    Console.WriteLine();
                }
            }
            writeLock.Release(); // Thông báo đã ghi xong
        }

        static void Reader(int index)
        {
            int waitTime = random.Next(1000); // Sử dụng một giá trị ngẫu nhiên để xác định thời gian chờ
            Thread.Sleep(waitTime); // Đợi một khoảng thời gian ngẫu nhiên

            readLock.WaitOne(); // Đợi cho đến khi có thể đọc
            readCount++; // Tăng số lượng người đọc
            if (readCount == 1)
            {
                writeLock.WaitOne(); // Nếu là người đọc đầu tiên thì chặn việc ghi
            }
            readLock.Release(); // Thông báo đã đọc xong

            Console.WriteLine("Reader " + index + " start reading..."); // Sử dụng giá trị index thay cho i
            Console.WriteLine();
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Reader "+ index+ " is reading...");
                Thread.Sleep(1000);
                if (i == 2)
                {
                    Console.WriteLine("Reader " + index + " has finished reading.");
                    Console.WriteLine();
                }
            }
            

            readLock.WaitOne(); // Đợi cho đến khi có thể đọc
            readCount--; // Giảm số lượng người đọc
            if (readCount == 0)
            {
                writeLock.Release(); // Nếu là người đọc cuối cùng thì mở khóa cho việc ghi
            }
            readLock.Release(); // Thông báo đã đọc xong
        }
    }
}
