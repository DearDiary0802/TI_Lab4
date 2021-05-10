using System;
using System.IO;

namespace TI_Lab4
{
    class Program
    {
        public static ulong key;
        public static string SourceText;
        public static string ResultOfCrypting;
        public static ulong Key()
        {
            key = ((((key >> 0) ^ (key >> 5) ^ (key >> 23)) & 1) << 31) | (key >> 1);
            return key & 1;
        }
        public static void LFSR1_Crypt(ulong inputKey)
        {
            const int BUFFER_SIZE = 1024;
            key = inputKey;
            BinaryReader binaryReader;
            try
            {
                binaryReader = new BinaryReader(File.Open(SourceText, FileMode.Open));
            }
            catch
            {
                throw new Exception("Возникли проблемы с открытие файла с исходным текстом");
            }
            BinaryWriter binaryWriter = new BinaryWriter(File.Open(ResultOfCrypting, FileMode.Create));
            
            byte[] data = new byte[BUFFER_SIZE];
            int readBytes;
            try
            {
                do
                {
                    readBytes = binaryReader.Read(data, 0, BUFFER_SIZE);
                    for (int i = 0; i < readBytes; i++)
                        for (int j = 0; j < 8; j++)
                        {
                            data[i] = (byte)(data[i] ^ (Key() << (8 - j)));
                        }
                    binaryWriter.Write(data, 0, readBytes);
                } while (readBytes > 0);
            }
            catch
            {
                throw;
            }
            finally
            {
                binaryReader.Close();
                binaryWriter.Close();
            }
        }
        static void Main(string[] args)
        {
            do
            {
                bool isCorrectKey = false;
                ulong inputKey = 0;
                while (!isCorrectKey)
                {
                    Console.WriteLine("Введите ключ: ");
                    try
                    {
                        inputKey = Convert.ToUInt64(Console.ReadLine());
                        isCorrectKey = true;
                    }
                    catch
                    {
                        Console.WriteLine("Некорректный ключ, попробуйте снова");
                        isCorrectKey = false;
                    }
                    Console.WriteLine("Введите имя файла с исходным текстом");
                    SourceText = Console.ReadLine();
                    Console.WriteLine("Введите имя файла, куда будет сохранен результат");
                    ResultOfCrypting = Console.ReadLine();
                }
                try
                {
                    LFSR1_Crypt(inputKey);
                    Console.WriteLine("Шифрование выполнено");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}
