using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Timers;

namespace aes
{
    internal class Program
    {
        private static void Main()
        {
            Console.Title = "Creado por Ilusir                                                                                   AES Encryptor";

        Inicio1:
            Console.Clear();
            Console.WriteLine("¿Qué quieres hacer?");
            Console.WriteLine("\t1 = Encriptar\n\t2 = Desencriptar\n");
            Console.Write("Introduce un número: "); string? respNum = Console.ReadLine();
            switch (respNum)
            {
                case "1":
                    Option1();
                    break;

                case "2":
                    Option2();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Introduce un número válido.\n");
                    goto Inicio1;
            }
        }

        private static void Option1()
        {
            Console.Title = "                                                                                                          AES Encryptor";

            Inicio2:
            Console.Clear();
            Console.WriteLine("¿Qué quieres encriptar?");
            Console.WriteLine("\t1 = Carpeta\n\t2 = Archivo\n\t3 = Volver\n");
            Console.Write("Introduce un número: "); string? respNum = Console.ReadLine();

            switch (respNum)
            {
                case "1":
                    OptionCarpeta();
                    break;
                case "2":
                    OptionArchivo();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Introduce un número válido.\n");
                    goto Inicio2;
            }
        }

        private static void Option2()
        {

            Console.Title = "                                                                                                          AES Encryptor";

        Inicio2:
            Console.Clear();
            Console.WriteLine("¿Qué quieres desencriptar?");
            Console.WriteLine("\t1 = Carpeta\n\t2 = Archivo\n\t3 = Volver\n");
            Console.Write("Introduce un número: "); string? respNum = Console.ReadLine();

            switch (respNum)
            {
                case "1":
                    OptionCarpetaDesencriptar();
                    break;
                case "2":
                    OptionArchivoDesencriptar();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Introduce un número válido.\n");
                    goto Inicio2;
            }
        }

        private static void OptionCarpeta()
        {
            Console.Clear();
            Console.WriteLine("Introduce la clave de encriptación y luego la ubicación de la carpeta (sin comillas)");

            Console.Write("\nClave (dejar en blanco para dejar al azar): "); string? respClave = Console.ReadLine();
            Ubicacion:
            Console.Write("\nUbicación: "); string? respUbi = Console.ReadLine();
            Console.Clear();

            try
            {
                string[] files = Directory.GetFiles(respUbi, "*.*", SearchOption.AllDirectories); 
                
                string path = new DirectoryInfo(respUbi).Name;
                string pathEncrypted = $"{path} (Encriptado)";
                Directory.CreateDirectory($"Archivos/{pathEncrypted}");

                if (respClave == string.Empty)
                {
                    respClave = RandomPassword(32);
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Tu contraseña es: {respClave}");
                Console.ForegroundColor = ConsoleColor.White;

                foreach (var item in files)
                {
                    FileInfo info = new(item);
                    var processInfo = new ProcessStartInfo("cmd.exe", $"/c aescrypt -e -p {respClave} -o encrypted_{info.Name} \"{info.FullName}")
                    {
                        UseShellExecute = false,
                        WorkingDirectory = $"Archivos/{pathEncrypted}"
                    };

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Archivo ({info.Name}) de la carpeta ({path}/) ha sido encriptado con éxito.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Process.Start(processInfo);
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Terminado, pulsa una tecla para volver al inicio.");
                Console.ReadKey();
                Main();
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("La carpeta no se encontró, presiona una tecla para volver a intentarlo.");
                Console.ReadKey();
                goto Ubicacion;
            }
        }

        private static void OptionCarpetaDesencriptar()
        {
            Console.Clear();
            Console.WriteLine("Introduce la clave de encriptación y luego la ubicación de la carpeta (sin comillas)");

            Console.Write("\nClave: "); string? respClave = Console.ReadLine();
        Ubicacion:
            Console.Write("\nUbicación: "); string? respUbi = Console.ReadLine();
            Console.Clear();

            try
            {
                string[] files = Directory.GetFiles(respUbi, "*.*", SearchOption.AllDirectories);
                string path = new DirectoryInfo(respUbi).Name;
                string path2 = path.Replace("(Encriptado)","(Desencriptado)");
                Directory.CreateDirectory($"Archivos/{path2}");

                foreach (var item in files)
                {
                    FileInfo info = new(item);
                    string archivo = info.Name.Replace("encrypted", "");
                    var processInfo = new ProcessStartInfo("cmd.exe", $"/c aescrypt -d -p {respClave} -o {archivo} \"{info.FullName}")
                    {
                        UseShellExecute = false,
                        WorkingDirectory = $"Archivos/{path2}"
                    };

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Archivo ({info.Name}) de la carpeta ({path}/) ha sido desencriptado con éxito.");
                    Process.Start(processInfo);
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Terminado, pulsa una tecla para volver al inicio.");
                Console.ReadKey();
                Main();
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("La carpeta no se encontró, presiona una tecla para volver a intentarlo.");
                Console.ReadKey();
                goto Ubicacion;
            }
        }

        private static void OptionArchivo()
        {
            Console.Clear();
            Console.WriteLine("Introduce la clave de encriptación y luego la ubicación del archivo (sin comillas)");

            Console.Write("\nClave: "); string? respClave = Console.ReadLine();
        Ubicacion:
            Console.Write("\nUbicación del archivo: "); string? respUbi = Console.ReadLine();
            Console.Clear();

            try
            {
                FileInfo info = new(respUbi);

                var processInfo = new ProcessStartInfo("cmd.exe", $"/c aescrypt -e -p {respClave} -o encrypted_{info.Name} \"{info.FullName}")
                {
                    UseShellExecute = false,
                    WorkingDirectory = $"Archivos/"
                };

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Archivo ({info.Name}) ha sido creado con éxito.");
                Console.ForegroundColor = ConsoleColor.White;
                Process.Start(processInfo);

                Console.WriteLine("Terminado, pulsa una tecla para volver al inicio.");
                Console.ReadKey();
                Main();
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("El archivo no se encontró, presiona una tecla para volver a intentarlo.");
                Console.ReadKey();
                goto Ubicacion;
            }
        }

        private static void OptionArchivoDesencriptar()
        {
            Console.Clear();
            Console.WriteLine("Introduce la clave de encriptación y luego la ubicación del archivo (sin comillas)");

            Console.Write("\nClave: "); string? respClave = Console.ReadLine();
        Ubicacion:
            Console.Write("\nUbicación del archivo: "); string? respUbi = Console.ReadLine();
            Console.Clear();

            try
            {
                FileInfo info = new(respUbi);

                string ex = $"{Path.GetFileNameWithoutExtension(info.Name)} (Desencriptado)";
                var processInfo = new ProcessStartInfo("cmd.exe", $"/c aescrypt -d -p {respClave} -o {ex} \"{info.FullName}")
                {
                    UseShellExecute = false,
                    WorkingDirectory = $"Archivos/"
                };

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Archivo ({info.Name}) ha sido creado con éxito.");
                Console.ForegroundColor = ConsoleColor.White;
                Process.Start(processInfo);

                Console.WriteLine("Terminado, pulsa una tecla para volver al inicio.");
                Console.ReadKey();
                Main();
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("El archivo no se encontró, presiona una tecla para volver a intentarlo.");
                Console.ReadKey();
                goto Ubicacion;
            }
        }

        private static string RandomPassword(int size = 0)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new();
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
    }
}