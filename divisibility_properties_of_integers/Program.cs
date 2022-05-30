using System;
using System.IO;
using System.IO.Compression;

namespace divisibility_properties_of_integers
{
    class Program
    {
        /// <summary>
        /// Метод для создания файла с числом N
        /// и возвращающий переменную для проверки существования директории
        /// </summary>
        public static string InputData()
        {
            Console.Write("Укажите директорию и расширение для создания файла:");
            string directory = Console.ReadLine();

            using (StreamWriter writerFile = new StreamWriter(directory))
            {
                string dataN = string.Empty;

                Console.Clear();
                Console.Write("Введите число: ");
                dataN = $"{Console.ReadLine()}";

                writerFile.WriteLine(dataN);
            }
            return directory;
        }

        /// <summary>
        /// Метод для проверки существования файла
        /// </summary>
        /// <param name="directory"></param>
        public static void CheckDirectory(string directory)
        {
            if (File.Exists(directory) == false)
            {
                Console.WriteLine("Ошибка! Файл не найден");
            }                      
        }

        /// <summary>
        /// Метот считывающий данные из файла 
        /// и возвращающий число для создания массива
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static uint ReadData(string directory)
        {
            using (StreamReader readFile = new StreamReader(directory))
            {
                string dataN;
                uint number = 0;
                if ((dataN = readFile.ReadLine()) != string.Empty)
                {
                    number = Convert.ToUInt32(dataN);                    
                }
                else
                {
                    Console.WriteLine("Ошибка! Файл пуст");
                }
                return number;
            }
        }                         

        /// <summary>
        /// Метод, который я нашел в интернете=)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int[][] GroupsNumbersArr(uint number)
        {

            /// Если переданное число ноль, то возвращается пустой список групп
            if (number == 0)
                return Array.Empty<int[]>();

            /// Если переданное число единица, то возвращается список групп с одной группой - единицей
            if (number == 1)
                return new int[][] { new int[] { 1 } };

            /// Создание массива для групп
            int[][] groups = new int[(uint)Math.Log(number, 2) + 1][];
            groups[0] = new int[] { 1 };
            int indexGroup = 1; // Индекс добавляемой группы

            /// Создание массива чисел содержащего все числа от 1 до заданного
            /// Единица используется как маркер
            /// Вместо удаления элеменов их значение будет приравниваться нулю
            /// После сортировки 1 будет разделять удалённые элементы и оставшиеся
            int[] numbers = new int[number];
            for (int i = 0; i < number; i++)
                numbers[i] = i + 1;

            /// Массив с промежуточными данными
            int[] group = new int[number];

            /// Цикл пока в массиве индекс единицы не последений
            int index1;
            while ((index1 = Array.BinarySearch(numbers, 1)) != number - 1) /// Проверка индекса единицы
            {
                /// Копия элементов в массив группы
                Array.Copy(numbers, group, number);

                int countGroup = 0; /// Количество элементов в группе
                                    /// Перебор элементов группы. i - индекс проверяемого элемента
                for (int i = index1 + 1; i < number; i++)
                {
                    if (group[i] != 0) /// Пропуск удалённых элементов
                    {
                        /// Удаление из группы всех элементов кратных проверяемому, кроме его самого
                        for (int j = i + 1; j < number; j++)
                            if (group[j] % group[i] == 0)
                                group[j] = 0;

                        /// Удаление элемента из массива чисел
                        numbers[i] = 0;
                        /// Счётчик группы увеличивется
                        countGroup++;
                    }

                }
                /// Сортировка массивов после удаления элементов
                Array.Sort(group);
                Array.Sort(numbers);

                /// Создание массива для добавления в группы
                /// и копирование в него значений старше 1
                int[] _gr = new int[countGroup];
                Array.Copy(group, Array.BinarySearch(group, 1) + 1, _gr, 0, countGroup);

                /// Добавление группы в массив групп
                groups[indexGroup] = _gr;
                indexGroup++;

            }
            /// Возврат списка групп
            return groups;            
        }

        /// <summary>
        /// Метод печатающий в консоль подсчитанные группы
        /// </summary>
        /// <param name="groups"></param>
        public static void WriteGroups(int[][]groups)
        {
            for (int i = 0; i < groups.Length; i++)
            {
                for (int j = 0; j < groups[i].Length; j++)
                {
                    Console.Write($"{groups[i][j]} ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Метод печатающий в консоль кол-во подсчитаных групп
        /// </summary>
        /// <param name="groups"></param>
        public static void WriteNumbGroups(int[][] groups)
        {
            DateTime date;
            date = DateTime.Now;

            int indexGroup = 0;

            for (int i = 0; i < groups.Length; i++)
            {
                indexGroup++;
            }
            Console.WriteLine($"Кол-во групп:{indexGroup}");

            TimeSpan timeSpan = DateTime.Now.Subtract(date);
            Console.WriteLine($"Затраченное время на обработку: {Math.Round(timeSpan.TotalSeconds, 1)} sec {timeSpan.TotalMilliseconds} ms");
        }

        /// <summary>
        /// Метод, сохраняющий результат вычисления в директорию test.txt
        /// </summary>
        /// <param name="array"></param>
        public static void SaveResult(int[][]array)
        {
            DateTime date;
            date = DateTime.Now;

            StreamWriter streamWriter = new StreamWriter("test.txt");

            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    streamWriter.Write($"{array[i][j]} ");
                }
                streamWriter.WriteLine();
            }

            streamWriter.Flush();
            streamWriter.Close();

            TimeSpan timeSpan = DateTime.Now.Subtract(date);
            Console.WriteLine($"Затраченное время на обработку: {Math.Round(timeSpan.TotalSeconds, 1)} sec {timeSpan.TotalMilliseconds} ms");
        }

        /// <summary>
        /// Метод для компрессии файла
        /// </summary>
        public static void CompressedFile()
        {
            string source = "test.txt";
            string compressed = "test.zip";
            using (FileStream ss = new FileStream(source, FileMode.OpenOrCreate))
            {
                using (FileStream ts = File.Create(compressed))   // поток для записи сжатого файла
                {
                    // поток архивации
                    using (GZipStream cs = new GZipStream(ts, CompressionMode.Compress))
                    {
                        ss.CopyTo(cs); // копируем байты из одного потока в другой
                        Console.WriteLine("Сжатие файла {0} завершено. Было: {1}  стало: {2}.",
                                          source,
                                          ss.Length,
                                          ts.Length);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            string directory = InputData();
            CheckDirectory(directory);
            uint numb = ReadData(directory);
            int[][] array = GroupsNumbersArr(numb);

            Console.WriteLine("Чтобы вывести количество групп введите 1 \nЧтобы сохранить полученные группы введите 2");
            string send = Console.ReadLine();
            if (Convert.ToInt32(send) == 1)
            {
                WriteNumbGroups(array);
            }
            if (Convert.ToInt32(send) == 2)
            {                
                SaveResult(array);

                Console.WriteLine("Для архивации файла введите 1 \nДля продолжения нажмити 2");
                send = Console.ReadLine();
                if (Convert.ToInt32(send) == 1)
                {
                    CompressedFile();
                }
                if (Convert.ToInt32(send) == 2)
                {
                    Console.WriteLine();
                }
            }            
            Console.ReadKey();            
        }
    }
}
