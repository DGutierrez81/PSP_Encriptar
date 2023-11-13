// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string archivo = "passwordsPrueba.txt";

        try
        {
            List<string> listaContraseñas = new List<string>();
            using StreamReader lector = new StreamReader(archivo);


            // Lee el contenido del archivo línea por línea
            string linea;

            while ((linea = lector.ReadLine()) != null)
            {
                listaContraseñas.Add(linea);
            }

            if(listaContraseñas.Count > 0)
            {
                Random random = new Random();

                int indice = random.Next(0, listaContraseñas.Count);

                string contraseniaAleatoria = listaContraseñas[indice];

                Console.WriteLine(contraseniaAleatoria);
            }
            else {
                Console.WriteLine("El archivo está vacio."); 
            }

            

        }

        catch (IOException ex)
        {
            Console.WriteLine($"Error al leer el archivo: {ex.Message}");
        }
        Console.ReadLine(); // Para mantener la consola abierta
    }
    
}