// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
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

        }

        catch (IOException ex)
        {
            Console.WriteLine($"Error al leer el archivo: {ex.Message}");
        }
        Console.ReadLine(); // Para mantener la consola abierta

        ComprobarContrasenia(contra, listaContraseñas);


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

    static void ComprobarContrasenia(string contrase, List<string> listaContraseñas)

    {

        List<string> listaHashesSHA256 = new List<string>();

        using (SHA256 sha256 = SHA256.Create())
        {
            foreach (string texto in listaContraseñas)
            {
                // Convertimos el texto a bytes
                byte[] textoBytes = Encoding.UTF8.GetBytes(texto);

                // Calculamos el hash SHA-256
                byte[] hashBytes = sha256.ComputeHash(textoBytes);

                // Convertimos el hash a una representación hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" para formato hexadecimal
                }

                listaHashesSHA256.Add(sb.ToString());
            }
        }

        for(int i = 0; i<listaHashesSHA256.Count; i++)
        {
            if (listaHashesSHA256[i] == contrase)
            {
                Console.WriteLine("La contraseña es: " + listaContraseñas[i]);
            }
        }
        /*
        Console.WriteLine("listaContraseñas:");
        foreach (var texto in listaContraseñas)
        {
            Console.WriteLine(texto);
        }

        Console.WriteLine("\nLista de hashes SHA-256:");
        foreach (var hash in listaHashesSHA256)
        {
            Console.WriteLine(hash);
        }
        */

    }

}