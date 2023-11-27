
// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;

class Program
{
    static bool cortar = false;
    static bool contraseñaEncontrada = false;
    static void Main(string[] args)
    {
        // Apuntes de clase: 

        // using (StreamWriter escritor = new StreamWriter("Archivoprueba.txt")) ; Se crea un archivo para saber donde esta la ruta del mismo
        // List<string> allLinesText = File.TeadAllLines("passwords.txt");
        // foreach(String line in allLinesText) Console.WriteLine(line);
        // var random = new Random();
        // var itamRandom = random.Next(allLinesText.Count);
        // var passordString = allLinesText[itemRandom];
        // Console.WriteLine(password);
        
        string archivo = "passwords.txt";
        List<string> listaContraseñas = new List<string>();
        string contra = "";
        

        try
        {
           
            using StreamReader lector = new StreamReader(archivo);


            // Lee el contenido del archivo línea por línea
            string linea;

            while ((linea = lector.ReadLine()) != null)
            {
                listaContraseñas.Add(linea);
            }
           
            Console.WriteLine(listaContraseñas.Count);

            if(listaContraseñas.Count > 0)
            {
                Random random = new Random();

                int indice = random.Next(0, listaContraseñas.Count);

                string contraseniaAleatoria = listaContraseñas[indice];

                Console.WriteLine(contraseniaAleatoria);

                EncriptarContraseña(contraseniaAleatoria);

                contra = EncriptarContraseña(contraseniaAleatoria);
            }
            else {
                Console.WriteLine("El archivo está vacio."); 
            }

            int division = listaContraseñas.Count / 10;

            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                int inicio = i * division;
                threads[i] = new Thread(() => ComprobarContrasenia(inicio, division, contra, listaContraseñas));
                if (cortar) break;
                threads[i].Start();
                threads[i].Join();
            }

            
            // Hace que el hilo actual espere que se ejecute cada hilo en la colección.
            /*
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            */
            /*
            int parte2 = division * 2;
            int parte3 = division * 3;
            int parte4 = division * 4;
            int parte5 = division * 5;
            int parte6 = division * 6;
            int parte7 = division * 7;
            int parte8 = division * 8;
            int parte9 = division * 9;

            Thread t1 = new Thread(() => ComprobarContrasenia(0, division, contra, listaContraseñas));
            t1.Start();
            Thread t2 = new Thread(() => ComprobarContrasenia(division, division, contra, listaContraseñas));
            t2.Start();
            Thread t3 = new Thread(() => ComprobarContrasenia(parte2, division, contra, listaContraseñas));
            t3.Start();
            Thread t4 = new Thread(() => ComprobarContrasenia(parte3, division, contra, listaContraseñas));
            t4.Start();
            Thread t5 = new Thread(() => ComprobarContrasenia(parte4, division, contra, listaContraseñas));
            t5.Start();
            Thread t6 = new Thread(() => ComprobarContrasenia(parte5, division, contra, listaContraseñas));
            t6.Start();
            Thread t7 = new Thread(() => ComprobarContrasenia(parte6, division, contra, listaContraseñas));
            t7.Start();
            Thread t8 = new Thread(() => ComprobarContrasenia(parte7, division, contra, listaContraseñas));
            t8.Start();
            Thread t9 = new Thread(() => ComprobarContrasenia(parte8, division, contra, listaContraseñas));
            t9.Start();
            Thread t10 = new Thread(() => ComprobarContrasenia(parte9, division, contra, listaContraseñas));
            t10.Start();
            */

        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error al leer el archivo: {ex.Message}");
        }

        Console.ReadLine(); // Para mantener la consola abierta
    }


    static string EncriptarContraseña(string contraseña)
    {
        using (var sha256 = SHA256.Create())
        {
            // Aplicar SHA-256 a la contraseña
            byte[] bytes = Encoding.UTF8.GetBytes(contraseña);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            // Convertir el hash a una cadena hexadecimal
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }


    static void ComprobarContrasenia(int inicio, int final, string contrase, List<string> listaContraseñas)

    {
        object objeto = new object();
        Stopwatch stopwatch = new Stopwatch();
        List<string> listaHashesSHA256 = new List<string>();

        stopwatch.Start();
        using (SHA256 sha256 = SHA256.Create())
        {
            
                foreach (string texto in listaContraseñas.GetRange(inicio, final))
                {

                    lock (objeto)
                    {
                        String nuevaEncriptada = EncriptarContraseña(texto);


                        if (nuevaEncriptada == contrase)
                        {
                            contraseñaEncontrada = true;
                            Console.WriteLine($"El hilo {Thread.CurrentThread.ManagedThreadId} encotró la contraseña");
                            stopwatch.Stop();
                            Console.WriteLine("La contraseña es: " + texto);
                            cortar = true;
                            break;
                        }

                        if (contraseñaEncontrada)
                        {
                            break; // Sale del bucle al encontrar la contraseña
                        }
                    }
                }
            
            

        }

        TimeSpan tiempoTranscurrido = stopwatch.Elapsed;

        // Mostrar el tiempo que tardó el hilo en ejecutarse
        Console.WriteLine($"El hilo {Thread.CurrentThread.ManagedThreadId} tardó: {tiempoTranscurrido}");

    }
}
